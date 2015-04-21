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
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract partial class ClassDeclarationBase
  {
    public void SetDocCommentBlock (IDocCommentBlock block)
    {
      _classDeclaration.SetDocCommentBlock(block);
    }

    public IDocCommentBlock DocCommentBlock
    {
      get { return _classDeclaration.DocCommentBlock; }
    }

    public IPrimaryConstructorDeclaration AddPrimaryConstructorDeclaration (IPrimaryConstructorDeclaration declaration)
    {
      return _classDeclaration.AddPrimaryConstructorDeclaration(declaration);
    }

    public void RemovePrimaryConstructorDeclaration ()
    {
      _classDeclaration.RemovePrimaryConstructorDeclaration();
    }

    public IBlock SetPrimaryConstructorBody (IBlock bodyBlock)
    {
      return _classDeclaration.SetPrimaryConstructorBody(bodyBlock);
    }

    public IPrimaryConstructorDeclaration SetPrimaryConstructor (IPrimaryConstructorDeclaration param)
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