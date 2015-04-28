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
using FakeItEasy;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (UsingSpecK), "Method")]
  public class UsingSpecK : SpecK
  {
    public static readonly Action SetupAction1 = A.Fake<Action> ();
    public static readonly Action SetupAction2 = A.Fake<Action> ();
    public static readonly Action CleanupAction1 = A.Fake<Action> ();
    public static readonly Action CleanupAction2 = A.Fake<Action> ();

    public UsingSpecK ()
    {
      Specify (x => 1)
          .DefaultCase (_ => _
              .GivenUsing ("context 1", x => new TestScope (SetupAction1, CleanupAction1))
              .Given ("Arrangement", x => { })
              .GivenUsing (x => new TestScope (SetupAction2, CleanupAction2))
              .It ("Assertion", x => { throw new Exception (); }));
    }
  }

  public class TestScope : IDisposable
  {
    readonly Action _cleanup;

    public TestScope (Action setup, Action cleanup)
    {
      setup ();
      _cleanup = cleanup;
    }


    public void Dispose ()
    {
      _cleanup ();
    }
  }
}

#if !EXAMPLE

namespace TestFx.Specifications.IntegrationTests
{
  using Evaluation.Results;
  using Extensibility.Providers;
  using FluentAssertions;
  using NUnit.Framework;

  public class UsingTest : TestBase<UsingSpecK>
  {
    [Test]
    public void Test ()
    {
      OperationResults.Should ().HaveCount (7);
      AssertResult (OperationResults[0], "Create context 1", State.Passed, OperationType.Action);
      AssertResult (OperationResults[1], "Arrangement", State.Passed, OperationType.Action);
      AssertResult (OperationResults[2], "Create TestScope", State.Passed, OperationType.Action);
      AssertResult (OperationResults[5], "Dispose TestScope", State.Passed, OperationType.Action);
      AssertResult (OperationResults[6], "Dispose context 1", State.Passed, OperationType.Action);
    }
  }
}

#endif