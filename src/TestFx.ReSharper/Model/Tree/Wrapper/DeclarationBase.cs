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
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class DeclarationBase : TreeNodeBase, ICSharpDeclaration
  {
    private readonly ICSharpDeclaration _declaration;

    protected DeclarationBase (ICSharpDeclaration declaration)
        : base(declaration)
    {
      _declaration = declaration;
    }

    public XmlNode GetXMLDoc (bool inherit)
    {
      return _declaration.GetXMLDoc(inherit);
    }

    public void SetName (string name)
    {
      _declaration.SetName(name);
    }

    public TreeTextRange GetNameRange ()
    {
      return _declaration.GetNameRange();
    }

    public bool IsSynthetic ()
    {
      return _declaration.IsSynthetic();
    }

    public IDeclaredElement DeclaredElement
    {
      get { return _declaration.DeclaredElement; }
    }

    public string DeclaredName
    {
      get { return _declaration.DeclaredName; }
    }

    public ICSharpIdentifier NameIdentifier
    {
      get { return _declaration.NameIdentifier; }
    }
  }
}