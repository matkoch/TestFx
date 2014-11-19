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
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Controllers
{
  public interface IClassSuiteController : ISuiteController
  {
    void ConfigureTestController (ITestController testController);
  }

  public class ClassSuiteController : SuiteController, IClassSuiteController
  {
    private readonly ISuite _suite;
    private readonly IEnumerable<ITestExtension> _testExtensions;

    public ClassSuiteController (SuiteProvider provider, ISuite suite, IEnumerable<ITestExtension> testExtensions, IOperationSorter operationSorter)
        : base(provider, operationSorter)
    {
      _suite = suite;
      _testExtensions = testExtensions;
    }

    public virtual void ConfigureTestController (ITestController testController)
    {
      _testExtensions.ForEach(x => x.Extend(testController, _suite));
    }
  }
}