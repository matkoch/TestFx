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
using System.Linq;
using FakeItEasy.Core;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Generics
{
  internal class GenericArgumentTest : TestBase<GenericArgumentTest.DomainSpec>
  {
    [Subject (typeof (GenericArgumentTest))]
    internal class DomainSpec : Spec
    {
      dynamic Argument;

      public DomainSpec ()
      {
        Specify (x => GenericHelper.GetObject (Argument))
            .Case ("Passing", _ => _
                .Given (x => { Argument = 1; })
                .ItReturns (x => 1))
            .Case ("Failing Value", _ => _
                .Given (x => { Argument = "one"; })
                .ItReturns (x => "two"))
            .Case ("Failing Type", _ => _
                .Given (x => { Argument = 1; })
                .ItReturns (x => "one"));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      var testResults = runResult.GetTestResults ();
      testResults[0].HasPassed ();
      testResults[1].HasFailed ();
      testResults[2].HasFailed ();
    }

    private static class GenericHelper
    {
      public static T GetObject<T>(T obj)
      {
        return obj;
      }
    }
  }
}