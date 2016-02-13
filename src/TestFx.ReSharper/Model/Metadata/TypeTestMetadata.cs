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
using System.Diagnostics;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Metadata.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Metadata
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  internal class TypeTestMetadata : MetadataTypeInfoBase, ITestMetadata
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly string _text;
    private readonly IEnumerable<string> _categories;

    public TypeTestMetadata (IIdentity identity, IProject project, IEnumerable<string> categories, string text, IMetadataTypeInfo metadataTypeInfo)
        : base(metadataTypeInfo)
    {
      _identity = identity;
      _project = project;
      _text = text;
      _categories = categories;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public IProject Project
    {
      get { return _project; }
    }

    public string Text
    {
      get { return _text; }
    }

    public IEnumerable<string> Categories
    {
      get { return _categories; }
    }

    public IEnumerable<ITestMetadata> TestMetadatas
    {
      get { yield break; }
    }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { yield break; }
    }
  }
}