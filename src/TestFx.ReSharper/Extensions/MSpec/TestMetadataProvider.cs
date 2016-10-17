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
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Metadata;
using TestFx.ReSharper.UnitTesting.Explorers.Metadata;
using TestFx.ReSharper.Utilities.Metadata;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Extensions.MSpec
{
  public class TestMetadataProvider : ITestMetadataProvider
  {
    private readonly IProject _project;
    private readonly IIdentity _assemblyIdentity;
    private readonly Func<bool> _notInterrupted;

    public TestMetadataProvider (IProject project, IIdentity assemblyIdentity, Func<bool> notInterrupted)
    {
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


      var identity = _assemblyIdentity.CreateChildIdentity(type.FullyQualifiedName);
      var categories = type.GetAttributeData<CategoriesAttribute>().GetValueOrDefault(
          x => x.ConstructorArguments[0].ValuesArray.Select(y => (string) y.Value),
          () => new string[0]).NotNull();
      var text = GetText(type);
      var fieldTests = type.GetFields()
          .TakeWhile(_notInterrupted)
          .Select(x => GetFieldTest(x, identity))
          .WhereNotNull();

      return new TypeTestMetadata(identity, _project, categories, text, fieldTests, type);
    }

    private bool IsSuite (IMetadataTypeInfo type)
    {
      if (type.GetAttributeData(MSpecUtility.BehaviorsAttributeFullName) != null)
        return false;

      return type.GetFields().Select(x => x.Type).OfType<IMetadataClassType>().Select(x => x.Type.FullyQualifiedName)
          .Any(x => x == MSpecUtility.ItDelegateFullName || x == MSpecUtility.BehavesLikeDelegateFullName);
    }

    private string GetText (IMetadataTypeInfo type)
    {
      var subjectAttribute = type.DescendantsAndSelf(x => x.DeclaringType)
          .Select(x => x.GetAttributeData(MSpecUtility.SubjectAttributeFullName)).WhereNotNull().First();

      var subjectTypes = subjectAttribute.ConstructorArguments.Select(x => x.Value as IMetadataType).WhereNotNull();
      var subjectText = subjectAttribute.ConstructorArguments.Select(x => x.Value as string).WhereNotNull().FirstOrDefault();

      return MSpecUtility.CreateText(type.ToCommon(), subjectTypes.SingleOrDefault()?.ToCommon(), subjectText);
    }

    [CanBeNull]
    private ITestMetadata GetFieldTest (IMetadataField field, IIdentity parentIdentity)
    {
      if (field.Type.NotNull().FullName != MSpecUtility.ItDelegateFullName)
        return null;

      if (field.GetAttributeData<CompilerGeneratedAttribute>() != null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(field.Name);
      var text = field.Name.Replace(oldChar: '_', newChar: ' ');
      return new MemberTestMetadata(identity, _project, text, field);
    }

    #endregion
  }
}