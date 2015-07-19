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
using NUnit.Framework;
using TestFx.Evaluation.Results;

namespace TestFx.SpecK.IntegrationTests.Using
{
  public class NestingTest : TestBase<NestingTest.DomainSpec>
  {
    [Subject (typeof (NestingTest), "Test")]
    public class DomainSpec : Spec
    {
      public DomainSpec ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .GivenUsing (typeof (FirstDisposable))
                .GivenUsing ("SecondDisposable (named)", x => new SecondDisposable ())
                .Given ("Arrangement", x => { })
                .GivenUsing (x => new ThirdDisposable ())
                .It ("Failing Assertion", x => { throw new Exception (); }));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTest (Default, State.Failed)
          .WithOperations (
              "Create FirstDisposable",
              "Create SecondDisposable (named)",
              "Arrangement",
              "Create ThirdDisposable",
              Action,
              "Failing Assertion",
              "Dispose ThirdDisposable",
              "Dispose SecondDisposable (named)",
              "Dispose FirstDisposable");
    }

    class FirstDisposable : IDisposable
    {
      public void Dispose ()
      {
      }
    }

    class SecondDisposable : IDisposable
    {
      public void Dispose ()
      {
      }
    }

    class ThirdDisposable : IDisposable
    {
      public void Dispose ()
      {
      }
    }
  }
}