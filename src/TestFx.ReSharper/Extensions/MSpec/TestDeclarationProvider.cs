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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using TestFx.ReSharper.Model.Tree;
using TestFx.ReSharper.UnitTesting.Explorers.Tree;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Extensions.MSpec
{
  public class TestDeclarationProvider : ITestDeclarationProvider
  {
    private readonly ITreePresenter _treePresenter;
    private readonly IProject _project;
    private readonly IIdentity _assemblyIdentity;
    private readonly Func<bool> _notInterrupted;

    public TestDeclarationProvider (ITreePresenter treePresenter, IProject project, IIdentity assemblyIdentity, Func<bool> notInterrupted)
    {
      _treePresenter = treePresenter;
      _project = project;
      _assemblyIdentity = assemblyIdentity;
      _notInterrupted = notInterrupted;
    }

    #region ITestDeclarationProvider

    [CanBeNull]
    public ITestDeclaration GetTestDeclaration (IClassDeclaration classDeclaration)
    {
      var clazz = classDeclaration.DeclaredElement.NotNull<IClass>();
      if (!IsSuite(clazz))
        return null;

      var subjectType = clazz.DescendantsAndSelf(x => x.GetContainingType() as IClass)
          .Select(
              x =>
              {
                var subjectAttributeData = x.GetAttributeData("Machine.Specifications.SubjectAttribute");
                if (subjectAttributeData == null)
                  return null;

                return subjectAttributeData.PositionParameter(paramIndex: 0).TypeValue.NotNull().ToCommon();
              })
          .WhereNotNull().FirstOrDefault();
      
      var concern = clazz.ToCommon().Name.Replace(oldChar: '_', newChar: ' ');
      var text = subjectType == null
          ? concern
          : subjectType.Name + ", " + concern;

      var identity = _assemblyIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var categories = clazz.GetAttributeData<CategoriesAttribute>()
          .GetValueOrDefault(
              x => x.PositionParameter(0).ArrayValue.NotNull().Select(y => (string) y.ConstantValue.Value),
              () => new string[0]).NotNull();
      var fieldTests = TreeNodeEnumerable.Create(
          () =>
          {
            return classDeclaration.FieldDeclarations
                .TakeWhile(_notInterrupted)
                .Select(x => GetFieldTest(x, identity))
                .WhereNotNull();
          });

      return new ClassTestDeclaration(identity, _project, categories, text, fieldTests, classDeclaration);
    }

    private static bool IsSuite (IClass clazz)
    {
      if (clazz.GetAttributeData("Machine.Specifications.BehaviorsAttribute") != null)
        return false;

      var fields = clazz.Fields;
      foreach (var field in fields)
      {
        var typeElement = field.Type.GetTypeElement();
        if (typeElement == null)
          continue;

        var fullName = typeElement.GetClrName().FullName;
        if (fullName == "Machine.Specifications.It" ||
            fullName == "Machine.Specifications.Behaves_like`1")
          return true;
      }
      return false;
    }

    [CanBeNull]
    private ITestDeclaration GetFieldTest (IFieldDeclaration fieldDeclaration, IIdentity parentIdentity)
    {
      if (fieldDeclaration.Type.GetTypeElement().NotNull().GetClrName().FullName != "Machine.Specifications.It")
        return null;

      var text = fieldDeclaration.DeclaredName.Replace(oldChar: '_', newChar: ' ');
      var identity = parentIdentity.CreateChildIdentity(fieldDeclaration.DeclaredName);
      return new FieldTestDeclaration(identity, _project, text, fieldDeclaration);
    }

    #endregion
  }
}