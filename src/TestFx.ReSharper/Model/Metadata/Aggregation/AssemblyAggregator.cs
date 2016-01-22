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
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Utilities.Metadata;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Model.Metadata.Aggregation
{
  public interface IAssemblyAggregator
  {
    ITestAssembly GetTestAssembly (IMetadataAssembly assembly);
  }

  public class AssemblyAggregator : IAssemblyAggregator
  {
    private readonly IMetadataPresenter _metadataPresenter;
    private readonly IProject _project;
    private readonly Func<bool> _notInterrupted;

    public AssemblyAggregator (IMetadataPresenter metadataPresenter, IProject project, Func<bool> notInterrupted)
    {
      _metadataPresenter = metadataPresenter;
      _project = project;
      _notInterrupted = notInterrupted;
    }

    public ITestAssembly GetTestAssembly (IMetadataAssembly assembly)
    {
      var identity = new Identity(_project.GetOutputFilePath().FullPath);
      var testTypes = assembly.GetTypes()
          .TakeWhile(_notInterrupted)
          .Select(x => VisitType(x, identity))
          .WhereNotNull();

      return new TestAssembly(testTypes.ToList(), assembly);
    }

    [CanBeNull]
    private ITestMetadata VisitType (IMetadataTypeInfo type, IIdentity parentIdentity)
    {
      var text = _metadataPresenter.Present(type);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(type.FullyQualifiedName);
      var categories = type.GetAttributeData<CategoriesAttribute>().GetValueOrDefault(
          x => x.ConstructorArguments[0].ValuesArray.Select(y => (string) y.Value),
          () => new string[0]);
      return new TypeTestMetadata(identity, _project, categories, text, type);
    }
  }
}