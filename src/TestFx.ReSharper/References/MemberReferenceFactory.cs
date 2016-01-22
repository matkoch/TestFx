// Copyright 2015, 2014 Matthias Koch
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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;

namespace TestFx.ReSharper.References
{
  public class MemberReferenceFactory : IReferenceFactory
  {
    private static readonly ClrTypeName s_memberReferenceAttribute = new ClrTypeName("JetBrains.Annotations.MemberReferenceAttribute");

    public IReference[] GetReferences (ITreeNode element, IReference[] oldReferences)
    {
      var literalExpression = element as ICSharpLiteralExpression;
      if (literalExpression == null || !(literalExpression.ConstantValue.Value is string) || literalExpression.ConstantValue.IsBadValue())
        return EmptyArray<IReference>.Instance;

      if (IsValid(literalExpression, oldReferences))
        return oldReferences;

      var type = GetRelatedTypeElementFromAttributeUsage(literalExpression);
      if (type == null)
        return EmptyArray<IReference>.Instance;

      return new IReference[] { new MemberReference(literalExpression, type) };
    }

    public bool HasReference (ITreeNode element, IReferenceNameContainer names)
    {
      var literalExpression = element as ILiteralExpression;
      if (literalExpression == null)
        return false;

      var referenceName = literalExpression.ConstantValue.Value as string;
      return referenceName != null && names.Contains(referenceName);
    }

    [ContractAnnotation ("oldReferences: null => false")]
    private bool IsValid (ICSharpLiteralExpression literalExpression, [CanBeNull] IReference[] oldReferences)
    {
      if (oldReferences == null)
        return false;

      var oldReference = oldReferences.Cast<MemberReference>().SingleOrDefault();
      if (oldReference == null)
        return false;

      var referenceName = literalExpression.ConstantValue.Value as string;
      return oldReference.IsValid() && oldReference.GetName() == referenceName;
    }

    [CanBeNull]
    private ITypeElement GetRelatedTypeElementFromAttributeUsage (ICSharpLiteralExpression literalExpression)
    {
      var attribute = AttributeNavigator.GetByConstructorArgumentExpression(literalExpression);
      if (attribute == null)
        return null;

      var argument = (ICSharpArgument) literalExpression.Parent.NotNull();
      if (argument.MatchingParameter == null)
        return null;

      var parameter = argument.MatchingParameter.Element;
      if (parameter.HasAttributeInstance(s_memberReferenceAttribute, false))
        return null;

      var constructor = attribute.ConstructorReference.GetResolved<IConstructor>().NotNull();
      var literalArgumentIndex = constructor.Parameters.IndexOf(parameter);
      if (literalArgumentIndex == 0)
        return null;

      var typeofExpression = attribute.Arguments[literalArgumentIndex - 1].Expression as ITypeofExpression;
      if (typeofExpression == null)
        return null;

      return ((IDeclaredType) typeofExpression.ArgumentType).GetTypeElement();
    }
  }
}