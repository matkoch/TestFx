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
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Model.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Model.Tree.Aggregation
{
  public interface IFileAggregator
  {
    ITestFile GetTestFile (ICSharpFile file);
  }

  public class FileAggregator : IFileAggregator
  {
    private readonly ITreePresenter _treePresenter;
    private readonly IProject _project;
    private readonly Func<bool> _notInterrupted;

    public FileAggregator (ITreePresenter treePresenter, IProject project, Func<bool> notInterrupted)
    {
      _treePresenter = treePresenter;
      _project = project;
      _notInterrupted = notInterrupted;
    }

    public ITestFile GetTestFile (ICSharpFile csharpFile)
    {
      var assemblyIdentity = new Identity(_project.GetOutputFilePath().FullPath);
      var classDeclarations = GetClassDeclarations(csharpFile);
      var classTests = TreeNodeCollection.Create(classDeclarations, x => GetClassTest(x, assemblyIdentity), _notInterrupted);

      return new TestFile(classTests, csharpFile);
    }

    private ITestDeclaration GetClassTest (IClassDeclaration classDeclaration, IIdentity parentIdentity)
    {
      var constructorDeclaration = classDeclaration.ConstructorDeclarations.SingleOrDefault(x => !x.IsStatic && x.ParameterDeclarations.Count == 0);
      if (constructorDeclaration == null)
        return null;

      var text = _treePresenter.Present(classDeclaration);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var invocationExpressions = GetInvocationExpressions(constructorDeclaration);
      var expressionTests = TreeNodeCollection.Create(invocationExpressions, x => GetInvocationTest(x, identity), _notInterrupted);

      return new ClassTestDeclaration(identity, _project, text, expressionTests, classDeclaration);
    }

    private ITestDeclaration GetInvocationTest (IInvocationExpression invocationExpression, IIdentity parentIdentity)
    {
      var text = _treePresenter.Present(invocationExpression);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(text);
      return new InvocationTestDeclaration(identity, _project, text, invocationExpression);
    }

    private IEnumerable<IClassDeclaration> GetClassDeclarations (ICSharpFile csharpFile)
    {
      var namespaceDeclarations = csharpFile.NamespaceDeclarations.SelectMany(x => x.DescendantsAndSelf(y => y.NamespaceDeclarations));
      var classDeclarations = namespaceDeclarations.Cast<ITypeDeclarationHolder>().SelectMany(x => x.TypeDeclarations)
          .SelectMany(x => x.DescendantsAndSelf(y => y.TypeDeclarations)).OfType<IClassDeclaration>();
      return classDeclarations;
    }

    private IEnumerable<IInvocationExpression> GetInvocationExpressions (IConstructorDeclaration constructorDeclaration)
    {
      var statementExpressions = constructorDeclaration.Body.Children<IExpressionStatement>().Select(x => x.Expression);
      var invocationExpressions = statementExpressions.OfType<IInvocationExpression>()
          .SelectMany(z => z.DescendantsAndSelf(x => x.InvokedExpression.FirstChild as IInvocationExpression).Reverse());
      return invocationExpressions;
    }
  }
}