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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
// ReSharper disable All

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract partial class FileBase : TreeNodeBase, ICSharpFile
  {
    private readonly ICSharpFile _file;

    protected FileBase (ICSharpFile file)
        : base(file)
    {
      _file = file;
    }

    public IFile ReParse (TreeTextRange modifiedRange, string text)
    {
      return _file.ReParse(modifiedRange, text);
    }

    public PsiFileModificationInfo GetReParseResult (TreeTextRange modifiedRange, string text)
    {
      return _file.GetReParseResult(modifiedRange, text);
    }

    public bool IsInjected ()
    {
      return _file.IsInjected();
    }

    public CachingLexer CachingLexer
    {
      get { return _file.CachingLexer; }
    }

    public int ModificationCounter
    {
      get { return _file.ModificationCounter; }
    }

    public bool CanContainCaseInsensitiveReferences
    {
      get { return _file.CanContainCaseInsensitiveReferences; }
    }

    public TreeNodeEnumerable<ICSharpNamespaceDeclaration> NamespaceDeclarationsEnumerable
    {
      get { return _file.NamespaceDeclarationsEnumerable; }
    }

    TreeNodeCollection<ICSharpTypeDeclaration> ICSharpTypeAndNamespaceHolderDeclaration.TypeDeclarations
    {
      get { return _file.TypeDeclarations; }
    }

    public TreeNodeEnumerable<ICSharpTypeDeclaration> TypeDeclarationsEnumerable
    {
      get { return _file.TypeDeclarationsEnumerable; }
    }

    IList<ITypeDeclaration> ITypeDeclarationHolder.TypeDeclarations
    {
      get { return ((ITypeDeclarationHolder) _file).TypeDeclarations; }
    }

    public IUsingList ImportsList
    {
      get { return _file.ImportsList; }
    }

    TreeNodeCollection<ICSharpNamespaceDeclaration> ICSharpTypeAndNamespaceHolderDeclaration.NamespaceDeclarations
    {
      get { return _file.NamespaceDeclarations; }
    }

    IList<INamespaceDeclaration> INamespaceDeclarationHolder.NamespaceDeclarations
    {
      get { return ((INamespaceDeclarationHolder) _file).NamespaceDeclarations; }
    }

    public IDeclarationsRange GetAllDeclarationsRange ()
    {
      return _file.GetAllDeclarationsRange();
    }

    public IDeclarationsRange GetDeclarationsRange (TreeTextRange range)
    {
      return _file.GetDeclarationsRange(range);
    }

    public IDeclarationsRange GetDeclarationsRange (IDeclaration first, IDeclaration last)
    {
      return _file.GetDeclarationsRange(first, last);
    }

    public void RemoveDeclarationsRange (IDeclarationsRange range)
    {
      _file.RemoveDeclarationsRange(range);
    }

    public IDeclarationsRange AddDeclarationsRangeAfter (IDeclarationsRange range, ITreeNode anchor)
    {
      return _file.AddDeclarationsRangeAfter(range, anchor);
    }

    public IDeclarationsRange AddDeclarationsRangeBefore (IDeclarationsRange range, ITreeNode anchor)
    {
      return _file.AddDeclarationsRangeBefore(range, anchor);
    }

    public IUsingDirective AddImportAfter (IUsingDirective param, IUsingDirective anchor)
    {
      return _file.AddImportAfter(param, anchor);
    }

    public IUsingDirective AddImportBefore (IUsingDirective param, IUsingDirective anchor)
    {
      return _file.AddImportBefore(param, anchor);
    }

    public IUsingDirective AddImport (IUsingDirective param, bool saveUsingListPosition = false)
    {
      return _file.AddImport(param, saveUsingListPosition);
    }

    public void RemoveImport (IUsingDirective param)
    {
      _file.RemoveImport(param);
    }

    public ICSharpTypeDeclaration AddTypeDeclarationBefore (ICSharpTypeDeclaration param, ICSharpTypeDeclaration anchor)
    {
      return _file.AddTypeDeclarationBefore(param, anchor);
    }

    public ICSharpTypeDeclaration AddTypeDeclarationAfter (ICSharpTypeDeclaration param, ICSharpTypeDeclaration anchor)
    {
      return _file.AddTypeDeclarationAfter(param, anchor);
    }

    public void RemoveTypeDeclaration (ICSharpTypeDeclaration param)
    {
      _file.RemoveTypeDeclaration(param);
    }

    public ICSharpNamespaceDeclaration AddNamespaceDeclarationBefore (ICSharpNamespaceDeclaration param, ICSharpNamespaceDeclaration anchor)
    {
      return _file.AddNamespaceDeclarationBefore(param, anchor);
    }

    public ICSharpNamespaceDeclaration AddNamespaceDeclarationAfter (ICSharpNamespaceDeclaration param, ICSharpNamespaceDeclaration anchor)
    {
      return _file.AddNamespaceDeclarationAfter(param, anchor);
    }

    public void RemoveNamespaceDeclaration (ICSharpNamespaceDeclaration param)
    {
      _file.RemoveNamespaceDeclaration(param);
    }

    public IUsingList SetImportsList (IUsingList param)
    {
      return _file.SetImportsList(param);
    }

    public ICSharpTypeAndNamespaceHolderDeclaration ContainingTypeAndNamespaceHolder
    {
      get { return _file.ContainingTypeAndNamespaceHolder; }
    }

    public TreeNodeCollection<IExternAliasDirective> ExternAliases
    {
      get { return _file.ExternAliases; }
    }

    public TreeNodeEnumerable<IExternAliasDirective> ExternAliasesEnumerable
    {
      get { return _file.ExternAliasesEnumerable; }
    }

    public TreeNodeCollection<IUsingDirective> Imports
    {
      get { return _file.Imports; }
    }

    public TreeNodeEnumerable<IUsingDirective> ImportsEnumerable
    {
      get { return _file.ImportsEnumerable; }
    }

    ITypeAndNamespaceHolderDeclaration ITypeAndNamespaceHolderDeclaration.ContainingTypeAndNamespaceHolder
    {
      get { return _file.ContainingTypeAndNamespaceHolder; }
    }

    public PreProcessingDirectivesInFile GetPreprocessorConditionals ()
    {
      return _file.GetPreprocessorConditionals();
    }

    public void RemoveAttribute (IAttribute attribute)
    {
      _file.RemoveAttribute(attribute);
    }

    public void AddAttribueBefore (IAttribute attribute, IAttribute tag)
    {
      _file.AddAttribueBefore(attribute, tag);
    }

    public TreeNodeCollection<ICSharpNamespaceDeclaration> NamespaceDeclarationNodes
    {
      get { return _file.NamespaceDeclarationNodes; }
    }

    public TreeNodeEnumerable<ICSharpNamespaceDeclaration> NamespaceDeclarationNodesEnumerable
    {
      get { return _file.NamespaceDeclarationNodesEnumerable; }
    }

    public TreeNodeCollection<IAttributeSection> Sections
    {
      get { return _file.Sections; }
    }

    public TreeNodeEnumerable<IAttributeSection> SectionsEnumerable
    {
      get { return _file.SectionsEnumerable; }
    }

    public TreeNodeCollection<IAttribute> Attributes
    {
      get { return _file.Attributes; }
    }

    public TreeNodeEnumerable<IAttribute> AttributesEnumerable
    {
      get { return _file.AttributesEnumerable; }
    }
  }
}