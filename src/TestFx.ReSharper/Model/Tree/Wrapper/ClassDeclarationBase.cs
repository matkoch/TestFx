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
using JetBrains.ReSharper.Psi.Tree;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class ClassDeclarationBase : DeclarationBase, IClassDeclaration
  {
    private readonly IClassDeclaration _classDeclaration;

    protected ClassDeclarationBase (IClassDeclaration classDeclaration)
        : base(classDeclaration)
    {
      _classDeclaration = classDeclaration;
    }

    public IList<ITypeDeclaration> TypeDeclarations
    {
      get { return _classDeclaration.TypeDeclarations; }
    }

    public string CLRName
    {
      get { return _classDeclaration.CLRName; }
    }

    public IEnumerable<IDeclaredType> SuperTypes
    {
      get { return _classDeclaration.SuperTypes; }
    }

    public void SetAbstract (bool value)
    {
      _classDeclaration.SetAbstract(value);
    }

    public void SetSealed (bool value)
    {
      _classDeclaration.SetSealed(value);
    }

    public void SetVirtual (bool value)
    {
      _classDeclaration.SetVirtual(value);
    }

    public void SetOverride (bool value)
    {
      _classDeclaration.SetOverride(value);
    }

    public void SetStatic (bool value)
    {
      _classDeclaration.SetStatic(value);
    }

    public void SetReadonly (bool value)
    {
      _classDeclaration.SetReadonly(value);
    }

    public void SetExtern (bool value)
    {
      _classDeclaration.SetExtern(value);
    }

    public void SetUnsafe (bool value)
    {
      _classDeclaration.SetUnsafe(value);
    }

    public void SetVolatile (bool value)
    {
      _classDeclaration.SetVolatile(value);
    }

    TreeNodeCollection<IOperatorDeclaration> IClassDeclaration.OperatorDeclarations
    {
      get { return _classDeclaration.OperatorDeclarations; }
    }

    public TreeNodeEnumerable<IOperatorDeclaration> OperatorDeclarationsEnumerable
    {
      get { return _classDeclaration.OperatorDeclarationsEnumerable; }
    }

    public TreeNodeCollection<ITypeParameterConstraint> TypeParameterConstraints
    {
      get { return _classDeclaration.TypeParameterConstraints; }
    }

    public TreeNodeEnumerable<ITypeParameterConstraint> TypeParameterConstraintsEnumerable
    {
      get { return _classDeclaration.TypeParameterConstraintsEnumerable; }
    }

    ITypeElement IClassLikeDeclaration.DeclaredElement
    {
      get { return _classDeclaration.DeclaredElement; }
    }

    TreeNodeCollection<ICSharpTypeDeclaration> IClassLikeDeclaration.NestedTypeDeclarations
    {
      get { return _classDeclaration.NestedTypeDeclarations; }
    }

    public TreeNodeEnumerable<ICSharpTypeDeclaration> NestedTypeDeclarationsEnumerable
    {
      get { return _classDeclaration.NestedTypeDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IPropertyDeclaration> PropertyDeclarations
    {
      get { return _classDeclaration.PropertyDeclarations; }
    }

    public TreeNodeEnumerable<IPropertyDeclaration> PropertyDeclarationsEnumerable
    {
      get { return _classDeclaration.PropertyDeclarationsEnumerable; }
    }

    public IClassBody Body
    {
      get { return _classDeclaration.Body; }
    }

    public TreeNodeCollection<IEventDeclaration> EventDeclarations
    {
      get { return _classDeclaration.EventDeclarations; }
    }

    public TreeNodeEnumerable<IEventDeclaration> EventDeclarationsEnumerable
    {
      get { return _classDeclaration.EventDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IIndexerDeclaration> IndexerDeclarations
    {
      get { return _classDeclaration.IndexerDeclarations; }
    }

    public TreeNodeEnumerable<IIndexerDeclaration> IndexerDeclarationsEnumerable
    {
      get { return _classDeclaration.IndexerDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IMethodDeclaration> MethodDeclarations
    {
      get { return _classDeclaration.MethodDeclarations; }
    }

    public TreeNodeEnumerable<IMethodDeclaration> MethodDeclarationsEnumerable
    {
      get { return _classDeclaration.MethodDeclarationsEnumerable; }
    }

    TreeNodeCollection<IConstructorDeclaration> IClassDeclaration.ConstructorDeclarations
    {
      get { return _classDeclaration.ConstructorDeclarations; }
    }

    public TreeNodeEnumerable<IConstructorDeclaration> ConstructorDeclarationsEnumerable
    {
      get { return _classDeclaration.ConstructorDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IDestructorDeclaration> DestructorDeclarations
    {
      get { return _classDeclaration.DestructorDeclarations; }
    }

    public TreeNodeEnumerable<IDestructorDeclaration> DestructorDeclarationsEnumerable
    {
      get { return _classDeclaration.DestructorDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IFieldDeclaration> FieldDeclarations
    {
      get { return _classDeclaration.FieldDeclarations; }
    }

    public TreeNodeEnumerable<IFieldDeclaration> FieldDeclarationsEnumerable
    {
      get { return _classDeclaration.FieldDeclarationsEnumerable; }
    }

    public TreeNodeCollection<IDeclaredTypeUsage> InheritedTypeUsages
    {
      get { return _classDeclaration.InheritedTypeUsages; }
    }

    public TreeNodeEnumerable<IDeclaredTypeUsage> InheritedTypeUsagesEnumerable
    {
      get { return _classDeclaration.InheritedTypeUsagesEnumerable; }
    }

    TreeNodeCollection<IOperatorDeclaration> IClassLikeDeclaration.OperatorDeclarations
    {
      get { return _classDeclaration.OperatorDeclarations; }
    }

    ITypeElement IMemberOwnerDeclaration.DeclaredElement
    {
      get { return _classDeclaration.DeclaredElement; }
    }

    public ITokenNode LBrace
    {
      get { return _classDeclaration.LBrace; }
    }

    public ITokenNode RBrace
    {
      get { return _classDeclaration.RBrace; }
    }

    TreeNodeCollection<ICSharpTypeMemberDeclaration> IMemberOwnerDeclaration.MemberDeclarations
    {
      get { return _classDeclaration.MemberDeclarations; }
    }

    ITypeElement IProperTypeDeclaration.DeclaredElement
    {
      get { return _classDeclaration.DeclaredElement; }
    }

    public IClassLikeDeclaration GetContainingClassLikeDeclaration ()
    {
      return _classDeclaration.GetContainingClassLikeDeclaration();
    }

    ITypeMember ICSharpTypeMemberDeclaration.DeclaredElement
    {
      get { return ((ICSharpTypeMemberDeclaration) _classDeclaration).DeclaredElement; }
    }

    public new ITypeDeclaration GetContainingTypeDeclaration ()
    {
      return ((ITypeMemberDeclaration) _classDeclaration).GetContainingTypeDeclaration();
    }

    ITypeMember ITypeMemberDeclaration.DeclaredElement
    {
      get { return ((ITypeMemberDeclaration) _classDeclaration).DeclaredElement; }
    }

    ITypeElement ICSharpTypeDeclaration.DeclaredElement
    {
      get { return _classDeclaration.DeclaredElement; }
    }

    public ITokenNode Semicolon
    {
      get { return _classDeclaration.Semicolon; }
    }

    public TreeNodeCollection<ITypeParameterConstraintsClause> TypeParameterConstraintsClauses
    {
      get { return _classDeclaration.TypeParameterConstraintsClauses; }
    }

    public TreeNodeEnumerable<ITypeParameterConstraintsClause> TypeParameterConstraintsClausesEnumerable
    {
      get { return _classDeclaration.TypeParameterConstraintsClausesEnumerable; }
    }

    public ITypeParameterOfTypeList TypeParameterList
    {
      get { return _classDeclaration.TypeParameterList; }
    }

    TreeNodeCollection<ICSharpTypeMemberDeclaration> ICSharpTypeDeclaration.MemberDeclarations
    {
      get { return _classDeclaration.MemberDeclarations; }
    }

    IModifiersOwner IModifiersOwnerDeclaration.DeclaredElement
    {
      get { return ((IModifiersOwnerDeclaration) _classDeclaration).DeclaredElement; }
    }

    ITypeElement ITypeDeclaration.DeclaredElement
    {
      get { return _classDeclaration.DeclaredElement; }
    }

    TreeNodeCollection<ITypeDeclaration> ITypeDeclaration.NestedTypeDeclarations
    {
      get { return ((ITypeDeclaration) _classDeclaration).NestedTypeDeclarations; }
    }

    TreeNodeCollection<ITypeMemberDeclaration> ITypeDeclaration.MemberDeclarations
    {
      get { return ((ITypeDeclaration) _classDeclaration).MemberDeclarations; }
    }

    public IDeclarationsRange GetAllDeclarationsRange ()
    {
      return _classDeclaration.GetAllDeclarationsRange();
    }

    public IDeclarationsRange GetDeclarationsRange (TreeTextRange range)
    {
      return _classDeclaration.GetDeclarationsRange(range);
    }

    public IDeclarationsRange GetDeclarationsRange (IDeclaration first, IDeclaration last)
    {
      return _classDeclaration.GetDeclarationsRange(first, last);
    }

    public void RemoveDeclarationsRange (IDeclarationsRange range)
    {
      _classDeclaration.RemoveDeclarationsRange(range);
    }

    public IDeclarationsRange AddDeclarationsRangeAfter (IDeclarationsRange range, ITreeNode anchor)
    {
      return _classDeclaration.AddDeclarationsRangeAfter(range, anchor);
    }

    public IDeclarationsRange AddDeclarationsRangeBefore (IDeclarationsRange range, ITreeNode anchor)
    {
      return _classDeclaration.AddDeclarationsRangeBefore(range, anchor);
    }

    public void SetPartial (bool value)
    {
      _classDeclaration.SetPartial(value);
    }

    public ITypeParameterOfTypeDeclaration AddTypeParameterBefore (ITypeParameterOfTypeDeclaration param, ITypeParameterOfTypeDeclaration anchor)
    {
      return _classDeclaration.AddTypeParameterBefore(param, anchor);
    }

    public ITypeParameterOfTypeDeclaration AddTypeParameterAfter (ITypeParameterOfTypeDeclaration param, ITypeParameterOfTypeDeclaration anchor)
    {
      return _classDeclaration.AddTypeParameterAfter(param, anchor);
    }

    public void RemoveTypeParameter (ITypeParameterOfTypeDeclaration param)
    {
      _classDeclaration.RemoveTypeParameter(param);
    }

    public ITypeParameterConstraintsClause AddTypeParameterConstraintsClauseBefore (
        ITypeParameterConstraintsClause param,
        ITypeParameterConstraintsClause anchor)
    {
      return _classDeclaration.AddTypeParameterConstraintsClauseBefore(param, anchor);
    }

    public ITypeParameterConstraintsClause AddTypeParameterConstraintsClauseAfter (
        ITypeParameterConstraintsClause param,
        ITypeParameterConstraintsClause anchor)
    {
      return _classDeclaration.AddTypeParameterConstraintsClauseAfter(param, anchor);
    }

    public void RemoveTypeParameterConstraintsClause (ITypeParameterConstraintsClause param)
    {
      _classDeclaration.RemoveTypeParameterConstraintsClause(param);
    }

    public bool CanBindTo (ITypeElement typeElement)
    {
      return _classDeclaration.CanBindTo(typeElement);
    }

    IModifiersList IClassLikeDeclaration.SetModifiersList (IModifiersList param)
    {
      return _classDeclaration.SetModifiersList(param);
    }

    IModifiersList IClassDeclaration.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    public ITokenNode ClassKeyword
    {
      get { return _classDeclaration.ClassKeyword; }
    }

    public IExtendsList ExtendsList
    {
      get { return _classDeclaration.ExtendsList; }
    }

    public TreeNodeCollection<IConstantDeclaration> ConstantDeclarations
    {
      get { return _classDeclaration.ConstantDeclarations; }
    }

    public TreeNodeEnumerable<IConstantDeclaration> ConstantDeclarationsEnumerable
    {
      get { return _classDeclaration.ConstantDeclarationsEnumerable; }
    }

    public void SetSuperClass (IDeclaredType classType)
    {
      _classDeclaration.SetSuperClass(classType);
    }

    public IExtendsList SetExtendsList (IExtendsList param)
    {
      return _classDeclaration.SetExtendsList(param);
    }

    IModifiersList IClassLikeDeclaration.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    public TreeNodeCollection<IDeclaredTypeUsage> SuperTypeUsageNodes
    {
      get { return _classDeclaration.SuperTypeUsageNodes; }
    }

    public TreeNodeCollection<IClassMemberDeclaration> ClassMemberDeclarations
    {
      get { return _classDeclaration.ClassMemberDeclarations; }
    }

    TreeNodeCollection<IConstructorDeclaration> IClassLikeDeclaration.ConstructorDeclarations
    {
      get { return _classDeclaration.ConstructorDeclarations; }
    }

    public T AddClassMemberDeclaration<T> (T param) where T : IClassMemberDeclaration
    {
      return _classDeclaration.AddClassMemberDeclaration(param);
    }

    public T AddClassMemberDeclarationAfter<T> (T param, IClassMemberDeclaration anchor) where T : IClassMemberDeclaration
    {
      return _classDeclaration.AddClassMemberDeclarationAfter(param, anchor);
    }

    public T AddClassMemberDeclarationBefore<T> (T param, IClassMemberDeclaration anchor) where T : IClassMemberDeclaration
    {
      return _classDeclaration.AddClassMemberDeclarationBefore(param, anchor);
    }

    public T ReplaceClassMemberDeclaration<T> (IClassMemberDeclaration oldDeclaration, T newDeclaration) where T : IClassMemberDeclaration
    {
      return _classDeclaration.ReplaceClassMemberDeclaration(oldDeclaration, newDeclaration);
    }

    public void RemoveClassMemberDeclaration (IClassMemberDeclaration param)
    {
      _classDeclaration.RemoveClassMemberDeclaration(param);
    }

    public T InsertAtSpecificPosition<T> (T param, ITreeNode anchor) where T : IClassMemberDeclaration
    {
      return _classDeclaration.InsertAtSpecificPosition(param, anchor);
    }

    public void AddSuperInterface (IDeclaredType interfaceType, bool before)
    {
      _classDeclaration.AddSuperInterface(interfaceType, before);
    }

    public void RemoveSuperInterface (IDeclaredType interfaceType)
    {
      _classDeclaration.RemoveSuperInterface(interfaceType);
    }

    public IClassBody SetBody (IClassBody param)
    {
      return _classDeclaration.SetBody(param);
    }

    IModifiersList ICSharpTypeDeclaration.SetModifiersList (IModifiersList param)
    {
      return ((ICSharpTypeDeclaration) _classDeclaration).SetModifiersList(param);
    }

    IModifiersList IClassMemberDeclaration.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    IModifiersList IClassMemberDeclaration.SetModifiersList (IModifiersList param)
    {
      return ((IClassMemberDeclaration) _classDeclaration).SetModifiersList(param);
    }

    IModifiersList ICSharpTypeDeclaration.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    public ICSharpNamespaceDeclaration OwnerNamespaceDeclaration
    {
      get { return _classDeclaration.OwnerNamespaceDeclaration; }
    }

    public TreeNodeCollection<ITypeParameterOfTypeDeclaration> TypeParameters
    {
      get { return _classDeclaration.TypeParameters; }
    }

    public TreeNodeEnumerable<ITypeParameterOfTypeDeclaration> TypeParametersEnumerable
    {
      get { return _classDeclaration.TypeParametersEnumerable; }
    }

    public ICSharpIdentifier SetNameIdentifier (ICSharpIdentifier param)
    {
      return _classDeclaration.SetNameIdentifier(param);
    }

    public ITypeParameterOfTypeList SetTypeParameterList (ITypeParameterOfTypeList param)
    {
      return _classDeclaration.SetTypeParameterList(param);
    }

    public bool IsPartial
    {
      get { return _classDeclaration.IsPartial; }
    }

    IModifiersList IModifiersListOwner.SetModifiersList (IModifiersList param)
    {
      return ((IModifiersListOwner) _classDeclaration).SetModifiersList(param);
    }

    IModifiersList IModifiersListOwner.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    public AccessRights GetAccessRights ()
    {
      return _classDeclaration.GetAccessRights();
    }

    public bool IsAbstract
    {
      get { return _classDeclaration.IsAbstract; }
    }

    public bool IsSealed
    {
      get { return _classDeclaration.IsSealed; }
    }

    public bool IsVirtual
    {
      get { return _classDeclaration.IsVirtual; }
    }

    public bool IsOverride
    {
      get { return _classDeclaration.IsOverride; }
    }

    public bool IsStatic
    {
      get { return _classDeclaration.IsStatic; }
    }

    public bool IsReadonly
    {
      get { return _classDeclaration.IsReadonly; }
    }

    public bool IsExtern
    {
      get { return _classDeclaration.IsExtern; }
    }

    public bool IsUnsafe
    {
      get { return _classDeclaration.IsUnsafe; }
    }

    public bool IsVolatile
    {
      get { return _classDeclaration.IsVolatile; }
    }

    public void SetAccessRights (AccessRights rights)
    {
      _classDeclaration.SetAccessRights(rights);
    }

    public bool HasAccessRights
    {
      get { return _classDeclaration.HasAccessRights; }
    }

    public IAttribute AddAttributeBefore (IAttribute param, IAttribute anchor)
    {
      return _classDeclaration.AddAttributeBefore(param, anchor);
    }

    public IAttribute AddAttributeAfter (IAttribute param, IAttribute anchor)
    {
      return _classDeclaration.AddAttributeAfter(param, anchor);
    }

    public IAttribute ReplaceAttribute (IAttribute attribute, IAttribute newAttribute)
    {
      return _classDeclaration.ReplaceAttribute(attribute, newAttribute);
    }

    public void RemoveAttribute (IAttribute param)
    {
      _classDeclaration.RemoveAttribute(param);
    }

    public TreeNodeCollection<IAttribute> Attributes
    {
      get { return _classDeclaration.Attributes; }
    }

    public TreeNodeEnumerable<IAttribute> AttributesEnumerable
    {
      get { return _classDeclaration.AttributesEnumerable; }
    }

    IModifiersList ICSharpModifiersOwnerDeclaration.ModifiersList
    {
      get { return _classDeclaration.ModifiersList; }
    }

    public void SetDocCommentBlock(IDocCommentBlock block)
    {
      _classDeclaration.SetDocCommentBlock(block);
    }

    public IDocCommentBlock DocCommentBlock
    {
      get { return _classDeclaration.DocCommentBlock; }
    }

    public IPrimaryConstructorDeclaration AddPrimaryConstructorDeclaration(IPrimaryConstructorDeclaration declaration)
    {
      return _classDeclaration.AddPrimaryConstructorDeclaration(declaration);
    }

    public void RemovePrimaryConstructorDeclaration()
    {
      _classDeclaration.RemovePrimaryConstructorDeclaration();
    }

    public IBlock SetPrimaryConstructorBody(IBlock bodyBlock)
    {
      return _classDeclaration.SetPrimaryConstructorBody(bodyBlock);
    }

    public IPrimaryConstructorDeclaration SetPrimaryConstructor(IPrimaryConstructorDeclaration param)
    {
      return _classDeclaration.SetPrimaryConstructor(param);
    }

    public IPrimaryConstructorDeclaration PrimaryConstructor
    {
      get { return _classDeclaration.PrimaryConstructor; }
    }

    public TreeNodeCollection<IBlock> PrimaryConstructorBodies
    {
      get { return _classDeclaration.PrimaryConstructorBodies; }
    }

    public TreeNodeEnumerable<IBlock> PrimaryConstructorBodiesEnumerable
    {
      get { return _classDeclaration.PrimaryConstructorBodiesEnumerable; }
    }
  }
}