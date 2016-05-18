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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class FieldDeclarationBase : DeclarationBase, IFieldDeclaration
  {
    private readonly IFieldDeclaration _fieldDeclaration;

    public FieldDeclarationBase (IFieldDeclaration fieldDeclaration)
      : base(fieldDeclaration)
    {
      _fieldDeclaration = fieldDeclaration;
    }

    public ITreeNode Initializer
    {
      get { return _fieldDeclaration.Initializer; }
    }

    public ITypeDeclaration GetContainingTypeDeclaration ()
    {
      return ((ITypeMemberDeclaration) this).GetContainingTypeDeclaration();
    }

    public IField DeclaredElement
    {
      get { return _fieldDeclaration.DeclaredElement; }
    }

    public ICSharpExpression FixedBufferSizeExpression
    {
      get { return _fieldDeclaration.FixedBufferSizeExpression; }
    }

    public IVariableInitializer Initial
    {
      get { return _fieldDeclaration.Initial; }
    }

    public ITokenNode LBracket
    {
      get { return _fieldDeclaration.LBracket; }
    }

    public ITokenNode RBracket
    {
      get { return _fieldDeclaration.RBracket; }
    }

    public IReferenceName ScalarTypeName
    {
      get { return _fieldDeclaration.ScalarTypeName; }
    }

    public ITypeUsage TypeUsage
    {
      get { return _fieldDeclaration.TypeUsage; }
    }

    public ICSharpExpression SetFixedBufferSizeExpression (ICSharpExpression param)
    {
      return _fieldDeclaration.SetFixedBufferSizeExpression(param);
    }

    public IVariableInitializer SetInitial (IVariableInitializer param)
    {
      return _fieldDeclaration.SetInitial(param);
    }

    public IReferenceName SetScalarTypeName (IReferenceName param)
    {
      return _fieldDeclaration.SetScalarTypeName(param);
    }

    public ITypeUsage SetTypeUsage (ITypeUsage param)
    {
      return _fieldDeclaration.SetTypeUsage(param);
    }

    ITypeMember ICSharpTypeMemberDeclaration.DeclaredElement
    {
      get { return ((ICSharpTypeMemberDeclaration) this).DeclaredElement; }
    }

    public void SetAbstract (bool value)
    {
      _fieldDeclaration.SetAbstract(value);
    }

    public void SetSealed (bool value)
    {
      _fieldDeclaration.SetSealed(value);
    }

    public void SetVirtual (bool value)
    {
      _fieldDeclaration.SetVirtual(value);
    }

    public void SetOverride (bool value)
    {
      _fieldDeclaration.SetOverride(value);
    }

    public void SetStatic (bool value)
    {
      _fieldDeclaration.SetStatic(value);
    }

    public void SetReadonly (bool value)
    {
      _fieldDeclaration.SetReadonly(value);
    }

    public void SetExtern (bool value)
    {
      _fieldDeclaration.SetExtern(value);
    }

    public void SetUnsafe (bool value)
    {
      _fieldDeclaration.SetUnsafe(value);
    }

    public void SetVolatile (bool value)
    {
      _fieldDeclaration.SetVolatile(value);
    }

    IModifiersOwner IModifiersOwnerDeclaration.DeclaredElement
    {
      get { return ((IModifiersOwnerDeclaration) this).DeclaredElement; }
    }

    ITypeMember ITypeMemberDeclaration.DeclaredElement
    {
      get { return ((ITypeMemberDeclaration) this).DeclaredElement; }
    }

    public AccessRights GetAccessRights ()
    {
      return _fieldDeclaration.GetAccessRights();
    }

    public bool IsAbstract
    {
      get { return _fieldDeclaration.IsAbstract; }
    }

    public bool IsSealed
    {
      get { return _fieldDeclaration.IsSealed; }
    }

    public bool IsVirtual
    {
      get { return _fieldDeclaration.IsVirtual; }
    }

    public bool IsOverride
    {
      get { return _fieldDeclaration.IsOverride; }
    }

    public bool IsStatic
    {
      get { return _fieldDeclaration.IsStatic; }
    }

    public bool IsReadonly
    {
      get { return _fieldDeclaration.IsReadonly; }
    }

    public bool IsExtern
    {
      get { return _fieldDeclaration.IsExtern; }
    }

    public bool IsUnsafe
    {
      get { return _fieldDeclaration.IsUnsafe; }
    }

    public bool IsVolatile
    {
      get { return _fieldDeclaration.IsVolatile; }
    }

    public void SetAccessRights (AccessRights rights)
    {
      _fieldDeclaration.SetAccessRights(rights);
    }

    public bool HasAccessRights
    {
      get { return _fieldDeclaration.HasAccessRights; }
    }

    public IAttribute AddAttributeBefore (IAttribute attribute, IAttribute anchor)
    {
      return _fieldDeclaration.AddAttributeBefore(attribute, anchor);
    }

    public IAttribute AddAttributeAfter (IAttribute attribute, IAttribute anchor)
    {
      return _fieldDeclaration.AddAttributeAfter(attribute, anchor);
    }

    public IAttribute ReplaceAttribute (IAttribute attribute, IAttribute newAttribute)
    {
      return _fieldDeclaration.ReplaceAttribute(attribute, newAttribute);
    }

    public void RemoveAttribute (IAttribute attribute)
    {
      _fieldDeclaration.RemoveAttribute(attribute);
    }

    public TreeNodeCollection<IAttribute> Attributes
    {
      get { return _fieldDeclaration.Attributes; }
    }

    public TreeNodeEnumerable<IAttribute> AttributesEnumerable
    {
      get { return _fieldDeclaration.AttributesEnumerable; }
    }

    public IModifiersList SetModifiersList (IModifiersList param)
    {
      return _fieldDeclaration.SetModifiersList(param);
    }

    IModifiersList IClassMemberDeclaration.ModifiersList
    {
      get { return _fieldDeclaration.ModifiersList; }
    }

    IModifiersList ICSharpModifiersOwnerDeclaration.ModifiersList
    {
      get { return ((ICSharpModifiersOwnerDeclaration) this).ModifiersList; }
    }

    public void SetType (IType type)
    {
      _fieldDeclaration.SetType(type);
    }

    public IType Type
    {
      get { return _fieldDeclaration.Type; }
    }

    public ICSharpIdentifier SetNameIdentifier (ICSharpIdentifier param)
    {
      return _fieldDeclaration.SetNameIdentifier(param);
    }

    public ITokenNode EquivalenceSign
    {
      get { return _fieldDeclaration.EquivalenceSign; }
    }

    public IMultipleDeclaration MultipleDeclaration
    {
      get { return _fieldDeclaration.MultipleDeclaration; }
    }
  }
}