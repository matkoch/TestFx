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
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Utilities
{
  public partial interface ITestIdentity : IIdentity
  {
    ITestProvider Provider { get; }
    IProject GetProject ();
  }

  public partial class TestIdentity : ITestIdentity
  {
    private readonly ITestProvider _provider;
    private readonly IIdentity _wrappedIdentity;

    private TestIdentity (ITestProvider provider, IIdentity wrappedIdentity)
    {
      _provider = provider;
      _wrappedIdentity = wrappedIdentity;
    }

    public ITestProvider Provider
    {
      get { return _provider; }
    }

    public IIdentity Parent
    {
      get { return _wrappedIdentity.Parent; }
    }

    public string Relative
    {
      get { return _wrappedIdentity.Relative; }
    }

    public string Absolute
    {
      get { return _wrappedIdentity.Absolute; }
    }
  }
}