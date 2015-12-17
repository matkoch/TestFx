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
using FakeItEasy.Core;
using FluentAssertions;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.IntegrationTests.Simple
{
  public class IsolationTest : TestBase<IsolationTest.DomainSpec>
  {
    [Subject (typeof (IsolationTest), "Test")]
    public class DomainSpec : Spec
    {
      static object StaticObject;
      object InstanceObject;

      DomainSpec ()
      {
        Specify (x => 1)
            .Case ("Setting", _ => _
                .Given (x => StaticObject = new object ())
                .Given (x => InstanceObject = new object ()))
            .Case ("Reusing", _ => _
                .It ("resets instance object", x => InstanceObject.Should ().BeNull ())
                .It ("saves static object", x => StaticObject.Should ().NotBeNull ()));
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.HasPassed ();
    }
  }
}
