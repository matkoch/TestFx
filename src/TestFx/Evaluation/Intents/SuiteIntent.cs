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
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Evaluation.Intents
{
  public interface ISuiteIntent : ISuiteIntentHolder
  {
    IEnumerable<ITestIntent> TestIntents { get; }

    void AddTestIntent (ITestIntent testIntent);
  }

  [Serializable]
  public class SuiteIntent : Intent, ISuiteIntent
  {
    public static ISuiteIntent Create(IIdentity identity)
    {
      return new SuiteIntent(identity);
    }

    private readonly List<ISuiteIntent> _suiteIntents;
    private readonly List<ITestIntent> _testIntents;

    private SuiteIntent (IIdentity identity)
        : base(identity)
    {
      _suiteIntents = new List<ISuiteIntent>();
      _testIntents = new List<ITestIntent>();
    }

    public IEnumerable<ISuiteIntent> SuiteIntents
    {
      get { return _suiteIntents; }
    }

    public IEnumerable<ITestIntent> TestIntents
    {
      get { return _testIntents; }
    }
    
    public void AddSuiteIntent (ISuiteIntent suiteIntent)
    {
      Trace.Assert(suiteIntent.Identity.Parent.Equals(Identity));
      _suiteIntents.Add(suiteIntent);
    }

    public void AddTestIntent (ITestIntent testIntent)
    {
      Trace.Assert(testIntent.Identity.Parent.Equals(Identity));
      _testIntents.Add(testIntent);
    }
  }
}