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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Model.Tree;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.UnitTesting.Explorers.Tree
{
  public interface IFileAggregator
  {
    [CanBeNull]
    ITestFile Aggregate (ICSharpFile file, [CanBeNull] Func<bool> notInterrupted);
  }

  [PsiComponent]
  internal class FileAggregator : IFileAggregator
  {
    private readonly List<ITestDeclarationProviderFactory> _testDeclarationProviderFactories;

    public FileAggregator (IEnumerable<ITestDeclarationProviderFactory> testDeclarationProviderFactories)
    {
      _testDeclarationProviderFactories = testDeclarationProviderFactories.ToList();
    }

    [CanBeNull]
    public ITestFile Aggregate (ICSharpFile file, [CanBeNull] Func<bool> notInterrupted)
    {
      notInterrupted = notInterrupted ?? (() => true);

      var project = file.GetProject();
      if (project == null)
        return null;

      var assemblyIdentity = new Identity(project.GetOutputFilePath().FullPath);
      var testDeclarationProviders = _testDeclarationProviderFactories.Select(x => x.CreateTestDeclarationProvider(assemblyIdentity, project, notInterrupted)).ToList();
      var classDeclarations = GetClassDeclarations(file).ToList();
      var testDeclarations = GetTestDeclarations(testDeclarationProviders, classDeclarations).TakeWhile(notInterrupted).WhereNotNull().ToList();

      if (testDeclarations.Count == 0)
        return null;

      return new TestFile(testDeclarations, file);
    }

    private IEnumerable<IClassDeclaration> GetClassDeclarations(ICSharpFile csharpFile)
    {
      var namespaceDeclarations = csharpFile.NamespaceDeclarations.SelectMany(x => x.DescendantsAndSelf(y => y.NamespaceDeclarations));
      var classDeclarations = namespaceDeclarations.Cast<ITypeDeclarationHolder>().SelectMany(x => x.TypeDeclarations)
          .SelectMany(x => x.DescendantsAndSelf(y => y.TypeDeclarations)).OfType<IClassDeclaration>();
      return classDeclarations;
    }

    private IEnumerable<ITestDeclaration> GetTestDeclarations (
        IList<ITestDeclarationProvider> testDeclarationProviders,
        IList<IClassDeclaration> classDeclarations)
    {
      return
        from testDeclarationProvider in testDeclarationProviders
        from classDeclaration in classDeclarations
        select testDeclarationProvider.GetTestDeclaration(classDeclaration);
    }
  }
}