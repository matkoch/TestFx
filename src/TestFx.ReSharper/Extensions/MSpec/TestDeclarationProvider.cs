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

      var identity = _assemblyIdentity.CreateChildIdentity(classDeclaration.CLRName);
      var categories = clazz.GetAttributeData<CategoriesAttribute>()
          .GetValueOrDefault(
              x => x.PositionParameter(paramIndex: 0).ArrayValue.NotNull().Select(y => (string) y.ConstantValue.Value),
              () => new string[0]).NotNull();
      var text = GetText(clazz);
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

    private bool IsSuite (IClass clazz)
    {
      if (clazz.GetAttributeData(MSpecUtility.BehaviorsAttributeFullName) != null)
        return false;

      return clazz.Fields.Select(x => x.Type.GetTypeElement()).WhereNotNull().Select(x => x.GetClrName().FullName)
          .Any(x => x == MSpecUtility.ItDelegateFullName || x == MSpecUtility.BehavesLikeDelegateFullName);
    }

    private string GetText (IClass clazz)
    {
      var subjectAttribute = clazz.DescendantsAndSelf(x => x.GetContainingType() as IClass)
          .Select(x => x.GetAttributeData(MSpecUtility.SubjectAttributeFullName)).WhereNotNull().First();

      var subjectTypes = subjectAttribute.PositionParameters().Select(x => x.TypeValue).WhereNotNull();
      var subjectText = subjectAttribute.PositionParameters().Select(x => x.ConstantValue.Value as string).WhereNotNull().FirstOrDefault();

      return MSpecUtility.CreateText(clazz.ToCommon(), subjectTypes.SingleOrDefault()?.ToCommon(), subjectText);
    }

    [CanBeNull]
    private ITestDeclaration GetFieldTest (IFieldDeclaration fieldDeclaration, IIdentity parentIdentity)
    {
      if (fieldDeclaration.Type.GetTypeElement().NotNull().GetClrName().FullName != MSpecUtility.ItDelegateFullName)
        return null;

      var identity = parentIdentity.CreateChildIdentity(fieldDeclaration.DeclaredName);
      var text = fieldDeclaration.DeclaredName.Replace(oldChar: '_', newChar: ' ');
      return new FieldTestDeclaration(identity, _project, text, fieldDeclaration);
    }

    #endregion
  }
}