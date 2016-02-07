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
using System.Linq;
using TestFx.Evaluation.Results;
using TestFx.Utilities.Collections;

namespace TestFx.TestInfrastructure
{
  public static class RunResultExtensions
  {
    public static ISuiteResult GetAssemblySuiteResult (this IRunResult runResult)
    {
      return runResult.SuiteResults.Single();
    }

    public static ISuiteResult GetClassSuiteResult (this IRunResult runResult)
    {
      return runResult.GetAssemblySuiteResult().SuiteResults.Single();
    }

    public static IReadOnlyList<ITestResult> GetTestResults (this IRunResult runResult)
    {
      var suiteResults = runResult.SuiteResults.SelectMany(x => x.DescendantsAndSelf(y => y.SuiteResults)).ToList();
      return suiteResults.SelectMany(x => x.TestResults).ToList();
    }

    public static ITestResult GetTestResult (this IRunResult runResult)
    {
      return runResult.GetTestResults().Single();
    }
  }
}