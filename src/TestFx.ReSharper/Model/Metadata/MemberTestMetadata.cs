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
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Metadata.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Metadata
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  internal class MemberTestMetadata : MetadataTypeMemberBase, ITestMetadata
  {
    public MemberTestMetadata (IIdentity identity, IProject project, string text, IMetadataTypeMember metadataTypeMember)
        : base(metadataTypeMember)
    {
      Identity = identity;
      Project = project;
      Text = text;
    }

    public IIdentity Identity { get; }
    public IProject Project { get; }

    public IEnumerable<string> Categories
    {
      get { yield break; }
    }

    public string Text { get; }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { yield break; }
    }

    public IEnumerable<ITestMetadata> TestMetadatas
    {
      get { yield break; }
    }
  }
}