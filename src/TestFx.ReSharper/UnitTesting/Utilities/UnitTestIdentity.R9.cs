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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Utilities
{
  public partial interface IUnitTestIdentity
  {
    UnitTestElementId ElementId { get; }
  }

  public partial class UnitTestIdentity
  {
    private readonly UnitTestElementId _elementId;

    public UnitTestIdentity (IUnitTestProviderEx provider, IProject project, IIdentity wrappedIdentity)
        : this(provider, wrappedIdentity)
    {
      _elementId = new UnitTestElementId(_provider, new PersistentProjectId(project), Absolute);
    }

    public UnitTestElementId ElementId
    {
      get { return _elementId; }
    }

    public IProject GetProject ()
    {
      return ElementId.GetProject();
    }
  }
}