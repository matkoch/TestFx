// Copyright 2014, 2013 Matthias Koch
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

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class FunctionDeclarationBase : DeclarationBase, ICSharpFunctionDeclaration
  {
    private readonly ICSharpFunctionDeclaration _functionDeclaration;

    protected FunctionDeclarationBase (ICSharpFunctionDeclaration functionDeclaration)
        : base(functionDeclaration)
    {
      _functionDeclaration = functionDeclaration;
    }

    public void SetAbstract (bool value)
    {
      _functionDeclaration.SetAbstract(value);
    }

    public void SetSealed (bool value)
    {
      _functionDeclaration.SetSealed(value);
    }

    public void SetVirtual (bool value)
    {
      _functionDeclaration.SetVirtual(value);
    }

    public void SetOverride (bool value)
    {
      _functionDeclaration.SetOverride(value);
    }

    public void SetStatic (bool value)
    {
      _functionDeclaration.SetStatic(value);
    }

    public void SetReadonly (bool value)
    {
      _functionDeclaration.SetReadonly(value);
    }

    public void SetExtern (bool value)
    {
      _functionDeclaration.SetExtern(value);
    }

    public void SetUnsafe (bool value)
    {
      _functionDeclaration.SetUnsafe(value);
    }

    public void SetVolatile (bool value)
    {
      _functionDeclaration.SetVolatile(value);
    }

    IFunction ICSharpFunctionDeclaration.DeclaredElement
    {
      get { return _functionDeclaration.DeclaredElement; }
    }

    public IBlock Body
    {
      get { return _functionDeclaration.Body; }
    }

    public ITokenNode Semicolon
    {
      get { return _functionDeclaration.Semicolon; }
    }

    public IBlock SetBody (IBlock param)
    {
      return _functionDeclaration.SetBody(param);
    }

    public bool IsIterator
    {
      get { return _functionDeclaration.IsIterator; }
    }

    public bool IsAsync
    {
      get { return _functionDeclaration.IsAsync; }
    }

    IModifiersOwner IModifiersOwnerDeclaration.DeclaredElement
    {
      get { return _functionDeclaration.DeclaredElement; }
    }

    IFunction IFunctionDeclaration.DeclaredElement
    {
      get { return _functionDeclaration.DeclaredElement; }
    }

    public AccessRights GetAccessRights ()
    {
      return _functionDeclaration.GetAccessRights();
    }

    public bool IsAbstract
    {
      get { return _functionDeclaration.IsAbstract; }
    }

    public bool IsSealed
    {
      get { return _functionDeclaration.IsSealed; }
    }

    public bool IsVirtual
    {
      get { return _functionDeclaration.IsVirtual; }
    }

    public bool IsOverride
    {
      get { return _functionDeclaration.IsOverride; }
    }

    public bool IsStatic
    {
      get { return _functionDeclaration.IsStatic; }
    }

    public bool IsReadonly
    {
      get { return _functionDeclaration.IsReadonly; }
    }

    public bool IsExtern
    {
      get { return _functionDeclaration.IsExtern; }
    }

    public bool IsUnsafe
    {
      get { return _functionDeclaration.IsUnsafe; }
    }

    public bool IsVolatile
    {
      get { return _functionDeclaration.IsVolatile; }
    }

    public void SetAccessRights (AccessRights rights)
    {
      _functionDeclaration.SetAccessRights(rights);
    }

    public bool HasAccessRights
    {
      get { return _functionDeclaration.HasAccessRights; }
    }

    public IAttribute AddAttributeBefore (IAttribute param, IAttribute anchor)
    {
      return _functionDeclaration.AddAttributeBefore(param, anchor);
    }

    public IAttribute AddAttributeAfter (IAttribute param, IAttribute anchor)
    {
      return _functionDeclaration.AddAttributeAfter(param, anchor);
    }

    public void RemoveAttribute (IAttribute param)
    {
      _functionDeclaration.RemoveAttribute(param);
    }

    public TreeNodeCollection<IAttribute> Attributes
    {
      get { return _functionDeclaration.Attributes; }
    }

    public TreeNodeEnumerable<IAttribute> AttributesEnumerable
    {
      get { return _functionDeclaration.AttributesEnumerable; }
    }

    public IModifiersList ModifiersList
    {
      get { return _functionDeclaration.ModifiersList; }
    }
  }
}