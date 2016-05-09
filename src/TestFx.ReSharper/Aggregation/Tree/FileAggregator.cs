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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Model.Tree.Aggregation
{
  public interface IFileAggregator
  {
    ITestFile GetTestFile (ICSharpFile file);
  }

  internal class FileAggregator : IFileAggregator
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
      var classTests = TreeNodeEnumerable.Create(
          () =>
          {
            return GetClassDeclarations(csharpFile)
                .TakeWhile(_notInterrupted)
                .Select(x => GetClassTest(x, assemblyIdentity))
                .WhereNotNull();
          });

      return new TestFile(classTests, csharpFile);
    }

    [CanBeNull]
    private ITestDeclaration GetClassTest (IClassDeclaration classDeclaration, IIdentity parentIdentity)
    {
      var text = _treePresenter.Present(classDeclaration);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var clazz = classDeclaration.DeclaredElement.NotNull<IClass>();
      var categories = clazz.GetAttributeData<CategoriesAttribute>()
          .GetValueOrDefault(
              x => x.PositionParameter(0).ArrayValue.NotNull().Select(y => (string) y.ConstantValue.Value),
              () => new string[0]);
      var constructorDeclaration = classDeclaration.ConstructorDeclarations.SingleOrDefault(x => !x.IsStatic && x.ParameterDeclarations.Count == 0);
      var expressionTests = TreeNodeEnumerable.Create(
          () =>
          {
            return GetInvocationExpressions(constructorDeclaration)
                .TakeWhile(_notInterrupted)
                .Select(x => GetInvocationTest(x, identity))
                .WhereNotNull();
          });

      return new ClassTestDeclaration(identity, _project, categories, text, expressionTests, classDeclaration);
    }

    [CanBeNull]
    private ITestDeclaration GetInvocationTest(IInvocationExpression invocationExpression, IIdentity parentIdentity)
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

    private IEnumerable<IInvocationExpression> GetInvocationExpressions ([CanBeNull] IConstructorDeclaration constructorDeclaration)
    {
      if (constructorDeclaration == null)
        return new IInvocationExpression[0];

      var statementExpressions = constructorDeclaration.Body.Children<IExpressionStatement>().Select(x => x.Expression);
      var invocationExpressions = statementExpressions.OfType<IInvocationExpression>()
          .SelectMany(z => z.DescendantsAndSelf(x => x.InvokedExpression.FirstChild as IInvocationExpression).Reverse());
      return invocationExpressions;
    }
  }
}