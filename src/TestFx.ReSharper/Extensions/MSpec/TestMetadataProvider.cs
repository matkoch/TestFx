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
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.Extensibility;
using TestFx.ReSharper.Model.Metadata;
using TestFx.ReSharper.UnitTesting.Explorers.Metadata;
using TestFx.ReSharper.Utilities.Metadata;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Extensions.MSpec
{
  public class TestMetadataProvider : ITestMetadataProvider
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;
    private readonly IMetadataPresenter _metadataPresenter;
    private readonly IProject _project;
    private readonly IIdentity _assemblyIdentity;
    private readonly Func<bool> _notInterrupted;

    public TestMetadataProvider (IMetadataPresenter metadataPresenter, IProject project, IIdentity assemblyIdentity, Func<bool> notInterrupted)
    {
      _introspectionPresenter = new IntrospectionPresenter();
      _metadataPresenter = metadataPresenter;
      _project = project;
      _assemblyIdentity = assemblyIdentity;
      _notInterrupted = notInterrupted;
    }

    #region ITestMetadataProvider

    [CanBeNull]
    public ITestMetadata GetTestMetadata (IMetadataTypeInfo type)
    {
      var isCompilerGenerated = type.GetAttributeData<CompilerGeneratedAttribute>() != null;
      if (isCompilerGenerated)
        return null;

      if (!IsSuite(type))
        return null;

      var subjectType = type.DescendantsAndSelf(x => x.DeclaringType)
          .Select(
              x =>
              {
                var subjectAttributeData = x.GetAttributeData("Machine.Specifications.SubjectAttribute");
                if (subjectAttributeData == null)
                  return null;

                return subjectAttributeData.ConstructorArguments.First().Type.NotNull().ToCommon();
              }).WhereNotNull().FirstOrDefault();
      
      var concern = type.ToCommon().Name.Replace(oldChar: '_', newChar: ' ');
      var text = subjectType == null
          ? concern
          : subjectType.Name + ", " + concern;

      var identity = _assemblyIdentity.CreateChildIdentity(type.FullyQualifiedName);
      var categories = type.GetAttributeData<CategoriesAttribute>().GetValueOrDefault(
          x => x.ConstructorArguments[0].ValuesArray.Select(y => (string) y.Value),
          () => new string[0]).NotNull();
      var fieldTests = type.GetFields()
          .TakeWhile(_notInterrupted)
          .Select(x => GetFieldTest(x, identity))
          .WhereNotNull();

      return new TypeTestMetadata(identity, _project, categories, text, fieldTests, type);
    }

    private static bool IsSuite (IMetadataTypeInfo type)
    {
      if (type.GetAttributeData("Machine.Specifications.BehaviorsAttribute") != null)
        return false;

      var fields = type.GetFields();
      foreach (var field in fields)
      {
        var metadataClassType = field.Type as IMetadataClassType;
        if (metadataClassType == null)
          continue;

        if (metadataClassType.Type.FullyQualifiedName == "Machine.Specifications.It" ||
            metadataClassType.Type.FullyQualifiedName == "Machine.Specifications.Behaves_like`1")
          return true;
      }

      return false;
    }

    [CanBeNull]
    private ITestMetadata GetFieldTest (IMetadataField field, IIdentity identity)
    {
      if (field.Type.NotNull().FullName != "Machine.Specifications.It")
        return null;

      if (field.GetAttributeData<CompilerGeneratedAttribute>() != null)
        return null;

      var text = field.Name.Replace(oldChar: '_', newChar: ' ');
      return new MemberTestMetadata(identity.CreateChildIdentity(text), _project, text.Replace("_", " "), field);
    }

    #endregion
  }
}