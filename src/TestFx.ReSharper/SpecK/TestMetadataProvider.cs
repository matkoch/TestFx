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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Aggregation.Metadata;
using TestFx.ReSharper.Model.Metadata;
using TestFx.ReSharper.Utilities.Metadata;
using TestFx.Utilities;

namespace TestFx.ReSharper.SpecK
{
  public class TestMetadataProvider : ITestMetadataProvider
  {
    private readonly IMetadataPresenter _metadataPresenter;
    private readonly IProject _project;
    private readonly IIdentity _assemblyIdentity;
    private readonly Func<bool> _notInterrupted;

    public TestMetadataProvider (IMetadataPresenter metadataPresenter, IProject project, IIdentity assemblyIdentity, Func<bool> notInterrupted)
    {
      _metadataPresenter = metadataPresenter;
      _project = project;
      _assemblyIdentity = assemblyIdentity;
      _notInterrupted = notInterrupted;
    }

    #region ITestMetadataProvider

    [CanBeNull]
    public ITestMetadata GetTestMetadata (IMetadataTypeInfo type)
    {
      var text = _metadataPresenter.Present(type);
      if (text == null)
        return null;

      var identity = _assemblyIdentity.CreateChildIdentity(type.FullyQualifiedName);
      var categories = type.GetAttributeData<CategoriesAttribute>().GetValueOrDefault(
          x => x.ConstructorArguments[0].ValuesArray.Select(y => (string) y.Value),
          () => new string[0]);
      return new TypeTestMetadata(identity, _project, categories, text, type);
    }

    #endregion
  }
}