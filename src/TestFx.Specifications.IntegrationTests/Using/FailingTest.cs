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

namespace TestFx.Specifications.IntegrationTests.Using
{
  public class FailingTest : TestBase<FailingTest.DomainSpecK>
  {
    [Subject (typeof (FailingTest), "Test")]
    public class DomainSpecK : SpecK
    {
      public DomainSpecK ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .GivenUsing (typeof (PassingDisposable))
                .GivenUsing (typeof (ThrowingDisposable))
                .GivenUsing (typeof (SkippedDisposable))
                .It ("Assertion", x => { }));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertDefaultTest (State.Failed)
          .WithOperations (
              "Create PassingDisposable",
              "Create ThrowingDisposable",
              "Dispose PassingDisposable");
    }

    class PassingDisposable : IDisposable
    {
      public void Dispose ()
      {
      }
    }

    class ThrowingDisposable : IDisposable
    {
      public ThrowingDisposable ()
      {
        throw new Exception ();
      }

      public void Dispose ()
      {
      }
    }

    class SkippedDisposable : IDisposable
    {
      public void Dispose ()
      {
      }
    }
  }
}