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
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Model.Utilities;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Tree.Aggregation
{
  public interface IFileAggregator
  {
    ISuiteFile GetSuiteFile (ICSharpFile file);
  }

  public class FileAggregator : IFileAggregator
  {
    private readonly ITreePresenter _treePresenter;
    private readonly IIdentityProvider _identityProvider;
    private readonly IProject _project;
    private readonly Func<bool> _notInterrupted;

    public FileAggregator (ITreePresenter treePresenter, IIdentityProvider identityProvider, IProject project, Func<bool> notInterrupted)
    {
      _treePresenter = treePresenter;
      _identityProvider = identityProvider;
      _project = project;
      _notInterrupted = notInterrupted;
    }

    public ISuiteFile GetSuiteFile (ICSharpFile csharpFile)
    {
      var assemblyIdentity = new Identity(_project.GetOutputFilePath().FullPath);
      var namespaceDeclarations = csharpFile.NamespaceDeclarations.SelectMany(x => x.Flatten(y => y.NamespaceDeclarations));
      var classDeclarations = namespaceDeclarations.Cast<ITypeDeclarationHolder>().SelectMany(x => x.TypeDeclarations)
          .SelectMany(x => x.Flatten(y => y.TypeDeclarations)).OfType<IClassDeclaration>();

      var classSuites = TreeNodeCollection.Create(classDeclarations, x => GetClassSuite(x, assemblyIdentity), _notInterrupted);

      return new SuiteFile(classSuites, csharpFile);
    }

    private ISuiteDeclaration GetClassSuite (IClassDeclaration classDeclaration, IIdentity parentIdentity)
    {
      var constructorDeclarations = classDeclaration.ConstructorDeclarations.Where(x => !x.IsStatic && x.ParameterDeclarations.Count == 0).ToList();
      if (constructorDeclarations.Count == 0)
        return null;

      var text = _treePresenter.Present(classDeclaration);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var expressionStatements = constructorDeclarations.SelectMany(x => x.Body.Children<IExpressionStatement>());
      var statementSuites = TreeNodeCollection.Create(expressionStatements, x => GetStatementSuite(x, identity), _notInterrupted);

      return new ClassSuiteDeclaration(identity, _project, text, statementSuites, new ITestDeclaration[0], classDeclaration);
    }

    private ISuiteDeclaration GetStatementSuite (IExpressionStatement expressionStatement, IIdentity parentIdentity)
    {
      var invocation = expressionStatement.Expression.As<IInvocationExpression>();
      if (invocation == null)
        return null;

      var invocationExpressions = invocation.Follow(x => x.InvokedExpression.FirstChild.As<IInvocationExpression>()).Reverse().ToList();
      var text = _treePresenter.Present(invocationExpressions.First());
      if (text == null)
        return null;

      var identity = _identityProvider.Next(parentIdentity);
      var invocationTests = TreeNodeCollection.Create(invocationExpressions.Skip(1), x => GetInvocationSuite(x, identity), _notInterrupted);

      return new StatementSuiteDeclaration(identity, _project, text, new ISuiteDeclaration[0], invocationTests, expressionStatement);
    }

    private ITestDeclaration GetInvocationSuite (IInvocationExpression invocationExpression, IIdentity parentIdentity)
    {
      var text = _treePresenter.Present(invocationExpression);
      if (text == null)
        return null;

      text = text.Trim('"');
      var identity = _identityProvider.Next(parentIdentity);
      return new InvocationTestDeclaration(identity, _project, text, invocationExpression);
    }
  }
}