// Copyright 2016, 2015, 2014 Matthias Koch
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using TestFx.ReSharper.Utilities.Psi.Resolve;

namespace TestFx.ReSharper.References
{
  internal class MemberReference : TreeReferenceBase<ILiteralExpression>, ICompletableReference, IReferenceFromStringLiteral
  {
    private readonly ICSharpLiteralExpression _literalExpression;
    private readonly ITypeElement _typeElement;

    public MemberReference (ICSharpLiteralExpression literalExpression, ITypeElement typeElement)
        : base(literalExpression)
    {
      _literalExpression = literalExpression;
      _typeElement = typeElement;
    }

    public override ResolveResultWithInfo ResolveWithoutCache ()
    {
      return GetReferenceSymbolTable(true).GetResolveResult(GetName());
    }

    public override string GetName ()
    {
      return _literalExpression.ConstantValue.Value as string ?? "???";
    }

    public override ISymbolTable GetReferenceSymbolTable (bool useReferenceName)
    {
      var symbolTable = ResolveUtil.GetSymbolTableByTypeElement(_typeElement, SymbolTableMode.FULL, _typeElement.Module)
          .Distinct(SymbolInfoComparer.Ordinal)
          .Filter(IsDeclaredTypeMember);

      return useReferenceName
          ? symbolTable.Filter(x => x.ShortName == GetName())
          : symbolTable;
    }

    private bool IsDeclaredTypeMember (ISymbolInfo symbol)
    {
      var typeMember = symbol.GetDeclaredElement() as ITypeMember;
      return typeMember != null && !typeMember.GetContainingType().IsObjectClass()
             && !typeMember.HasAttributeInstance(PredefinedType.COMPILER_GENERATED_ATTRIBUTE_CLASS, false);
    }

    public override TreeTextRange GetTreeTextRange ()
    {
      var treeRange = _literalExpression.GetStringLiteralContentTreeRange();
      return treeRange.Length > 0 ? treeRange : _literalExpression.GetTreeTextRange();
    }

    public override IReference BindTo (IDeclaredElement element)
    {
      var literalAlterer = StringLiteralAltererUtil.CreateStringLiteralByExpression(_literalExpression);
      literalAlterer.Replace(GetName(), element.ShortName, _literalExpression.GetPsiModule());
      var newLiteralExpression = literalAlterer.Expression;
      if (_literalExpression.Equals(newLiteralExpression))
        return this;

      return newLiteralExpression.FindReference<MemberReference>() ?? this;
    }

    public override IReference BindTo (IDeclaredElement element, ISubstitution substitution)
    {
      return BindTo(element);
    }

    public override IAccessContext GetAccessContext ()
    {
      return new ElementAccessContext(_literalExpression);
    }

    public ISymbolTable GetCompletionSymbolTable ()
    {
      return GetReferenceSymbolTable(false);
    }

    public IEnumerable<DeclaredElementType> ExpecteDeclaredElementTypes
    {
      get { yield return CLRDeclaredElementType.METHOD; }
    }
  }
}