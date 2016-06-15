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
using JetBrains.ReSharper.Psi.Util;
using TestFx.Extensibility;
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
      var hasBecauseField = clazz.Fields.Any(x => x.Type.GetTypeElement()?.GetClrName().FullName == "Machine.Specifications.Because");
      if (!hasBecauseField)
        return null;

      var text = clazz.DescendantsAndSelf(x => x.GetContainingType() as IClass)
          .Select(
              x =>
              {
                var subjectAttributeData = x.GetAttributeData("Machine.Specifications.SubjectAttribute");
                if (subjectAttributeData == null)
                  return null;

                var subjectType = subjectAttributeData.PositionParameter(paramIndex: 0).TypeValue.NotNull().ToCommon();

                return subjectType.Name + ", " + clazz.ToCommon().Name.Replace(oldChar: '_', newChar: ' ');
              })
          .WhereNotNull().FirstOrDefault();
      if (text == null)
        return null;

      var identity = _assemblyIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var categories = clazz.GetAttributeData<CategoriesAttribute>()
          .GetValueOrDefault(
              x => x.PositionParameter(0).ArrayValue.NotNull().Select(y => (string) y.ConstantValue.Value),
              () => new string[0]).NotNull();
      var fieldTests = TreeNodeEnumerable.Create(
          () =>
          {
            return classDeclaration.FieldDeclarations.SelectMany(Flatten)
                .TakeWhile(_notInterrupted)
                .Select(x => GetFieldTest(x, identity))
                .WhereNotNull();
          });

      return new ClassTestDeclaration(identity, _project, categories, text, fieldTests, classDeclaration);
    }

    private IEnumerable<IFieldDeclaration> Flatten (IFieldDeclaration fieldDeclaration)
    {
      var declaredType = fieldDeclaration.DeclaredElement.Type as IDeclaredType;
      if (declaredType == null)
        yield break;

      var typeElement = declaredType.GetTypeElement().NotNull();
      if (typeElement.GetClrName().FullName != "Machine.Specifications.Behaves_like`1")
      {
        yield return fieldDeclaration;
        yield break;
      }

      var substitution = declaredType.GetSubstitution();
      var typeArguments = substitution.Domain;
      var behaviorTypes = typeArguments.SelectMany(x => substitution[x].GetTypeElement().NotNull().GetDeclarations()).OfType<IClassDeclaration>();
      foreach (var nestedField in behaviorTypes.SelectMany(x => x.FieldDeclarations.SelectMany(Flatten)))
        yield return nestedField;
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