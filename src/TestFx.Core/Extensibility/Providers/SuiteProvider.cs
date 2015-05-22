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
using System.Collections.Generic;
using System.Linq;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface ISuiteProvider : IProvider
  {
    IEnumerable<IOperationProvider> ContextProviders { get; }
    IEnumerable<ISuiteProvider> SuiteProviders { get; }
    IEnumerable<ITestProvider> TestProviders { get; }
  }

  public class SuiteProvider : Provider, ISuiteProvider
  {
    public static SuiteProvider Create (IIdentity identity, string text, bool ignored)
    {
      return new SuiteProvider(identity, text, ignored);
    }

    private ICollection<IOperationProvider> _contextProviders;
    private ICollection<ISuiteProvider> _suiteProviders;
    private ICollection<ITestProvider> _testProviders;

    private SuiteProvider (IIdentity identity, string text, bool ignored)
        : base(identity, text, ignored)
    {
      _contextProviders = new IOperationProvider[0];
      _suiteProviders = new ISuiteProvider[0];
      _testProviders = new ITestProvider[0];
    }

    public IEnumerable<IOperationProvider> ContextProviders
    {
      get { return _contextProviders; }
      set { _contextProviders = value.ToList(); }
    }

    public IEnumerable<ISuiteProvider> SuiteProviders
    {
      get { return _suiteProviders; }
      set { _suiteProviders = value.ToList(); }
    }

    public IEnumerable<ITestProvider> TestProviders
    {
      get { return _testProviders; }
      set { _testProviders = value.ToList(); }
    }
  }
}