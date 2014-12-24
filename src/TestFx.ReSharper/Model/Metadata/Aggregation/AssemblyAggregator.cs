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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Utilities;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Metadata.Aggregation
{
  public interface IAssemblyAggregator
  {
    ISuiteMetadata GetAssemblySuite (IMetadataAssembly assembly);
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

    public ISuiteMetadata GetAssemblySuite (IMetadataAssembly assembly)
    {
      var identity = new Identity(_project.GetOutputFilePath().FullPath);
      var suites = MetadataEntityCollection.Create(assembly.GetTypes(), x => VisitType(x, identity), _notInterrupted);

      return new AssemblySuiteMetadata(identity, _project, _project.Name, suites, assembly);
    }

    private ISuiteMetadata VisitType (IMetadataTypeInfo type, IIdentity parentIdentity)
    {
      var text = _metadataPresenter.Present(type);
      if (text == null)
        return null;

      var identity = parentIdentity.CreateChildIdentity(type.FullyQualifiedName);
      return new TypeSuiteMetadata(identity, _project, text, type);
    }
  }
}