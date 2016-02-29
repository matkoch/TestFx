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
using System.Reflection;
using FakeItEasy.Core;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Generics
{
  [Ignore]
  internal class GenericTypeTest : TestBase<GenericTypeTest.DomainSpec>
  {
    [Subject(typeof(GenericTypeTest), "Test")]
    internal class DomainSpec : Spec
    {
      dynamic Argument;

      public DomainSpec ()
      {
        Specify (x => GenericHelper.Create<T1, T2, T3> (Argument))
            .Case ("Integer", _ => _
                .WithPermutations (
                    new { T1 = default(Type), T2 = default(Type), T3 = default(Type) },
                    x => x.T1, Assembly.GetExecutingAssembly ().GetTypes (),
                    x => x.T2, Assembly.GetExecutingAssembly ().GetTypes (),
                    x => x.T3, Assembly.GetExecutingAssembly ().GetTypes ())
                //.GivenGenericsFromSequence ()
                //.GivenGenerics(x => x.Generics.T1 = x.Sequence.T1)

                .GivenVars(x => new { T1 = typeof(int) })
                //.GivenGenericsFromVars()
                //.GivenGenerics(T1: typeof(int))
                .ItThrows (typeof (InvalidOperationException)))
            .Case ("Integer", _ => _
                .Given ("DefaultDomain")
                .It ("", x => x.Result.Should ().NotBe (0)));


      }
    }

    protected override void AssertResults(IRunResult runResult, IFakeScope scope)
    {
      var testResults = runResult.GetTestResults();
      testResults[0].HasPassed();
      testResults[1].HasFailed();
      testResults[2].HasFailed();
    }

    private static class GenericHelper
    {
      public static T1 Create<T1, T2, T3>(T3 arg)
        where T1 : new()
      {
        return new T1();
      }
    }

    class T1 { }
    class T2 { }
    class T3 { }
  }
}