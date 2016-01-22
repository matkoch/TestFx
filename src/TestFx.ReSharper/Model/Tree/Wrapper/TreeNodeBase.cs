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
using System.Collections.Generic;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract class TreeNodeBase : ICSharpTreeNode
  {
    private readonly ICSharpTreeNode _treeNode;

    protected TreeNodeBase (ICSharpTreeNode treeNode)
    {
      _treeNode = treeNode;
    }

    public IPsiServices GetPsiServices ()
    {
      return _treeNode.GetPsiServices();
    }

    public IPsiModule GetPsiModule ()
    {
      return _treeNode.GetPsiModule();
    }

    public IPsiSourceFile GetSourceFile ()
    {
      return _treeNode.GetSourceFile();
    }

    public ReferenceCollection GetFirstClassReferences ()
    {
      return _treeNode.GetFirstClassReferences();
    }

    public void ProcessDescendantsForResolve (IRecursiveElementProcessor processor)
    {
      _treeNode.ProcessDescendantsForResolve(processor);
    }

    public T GetContainingNode<T> (bool returnThis = false) where T : ITreeNode
    {
      return _treeNode.GetContainingNode<T>(returnThis);
    }

    public bool Contains (ITreeNode other)
    {
      return _treeNode.Contains(other);
    }

    public bool IsPhysical ()
    {
      return _treeNode.IsPhysical();
    }

    public bool IsValid ()
    {
      return _treeNode.IsValid();
    }

    public bool IsFiltered ()
    {
      return _treeNode.IsFiltered();
    }

    public DocumentRange GetNavigationRange ()
    {
      return _treeNode.GetNavigationRange();
    }

    public TreeOffset GetTreeStartOffset ()
    {
      return _treeNode.GetTreeStartOffset();
    }

    public int GetTextLength ()
    {
      return _treeNode.GetTextLength();
    }

    public StringBuilder GetText (StringBuilder to)
    {
      return _treeNode.GetText(to);
    }

    public IBuffer GetTextAsBuffer ()
    {
      return _treeNode.GetTextAsBuffer();
    }

    public string GetText ()
    {
      return _treeNode.GetText();
    }

    public ITreeNode FindNodeAt (TreeTextRange treeTextRange)
    {
      return _treeNode.FindNodeAt(treeTextRange);
    }

    public ICollection<ITreeNode> FindNodesAt (TreeOffset treeTextOffset)
    {
      return _treeNode.FindNodesAt(treeTextOffset);
    }

    public ITreeNode FindTokenAt (TreeOffset treeTextOffset)
    {
      return _treeNode.FindTokenAt(treeTextOffset);
    }

    public ITreeNode Parent
    {
      get { return _treeNode.Parent; }
    }

    public ITreeNode FirstChild
    {
      get { return _treeNode.FirstChild; }
    }

    public ITreeNode LastChild
    {
      get { return _treeNode.LastChild; }
    }

    public ITreeNode NextSibling
    {
      get { return _treeNode.NextSibling; }
    }

    public ITreeNode PrevSibling
    {
      get { return _treeNode.PrevSibling; }
    }

    public NodeType NodeType
    {
      get { return _treeNode.NodeType; }
    }

    public PsiLanguageType Language
    {
      get { return _treeNode.Language; }
    }

    public NodeUserData UserData
    {
      get { return _treeNode.UserData; }
    }

    public NodeUserData PersistentUserData
    {
      get { return _treeNode.PersistentUserData; }
    }

    public void Accept (TreeNodeVisitor visitor)
    {
      _treeNode.Accept(visitor);
    }

    public void Accept<TContext> (TreeNodeVisitor<TContext> visitor, TContext context)
    {
      _treeNode.Accept(visitor, context);
    }

    public TReturn Accept<TContext, TReturn> (TreeNodeVisitor<TContext, TReturn> visitor, TContext context)
    {
      return _treeNode.Accept(visitor, context);
    }

    public ICSharpNamespaceDeclaration GetContainingNamespaceDeclaration ()
    {
      return _treeNode.GetContainingNamespaceDeclaration();
    }

    public ICSharpTypeMemberDeclaration GetContainingTypeMemberDeclaration ()
    {
      return _treeNode.GetContainingTypeMemberDeclaration();
    }

    public ICSharpTypeDeclaration GetContainingTypeDeclaration ()
    {
      return _treeNode.GetContainingTypeDeclaration();
    }
  }
}