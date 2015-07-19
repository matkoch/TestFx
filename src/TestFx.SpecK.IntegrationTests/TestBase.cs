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
using FakeItEasy;
using FakeItEasy.Core;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;
using TestFx.SpecK.Implementation;
using TestFx.Utilities;
using TestFx.Utilities.Collections;
using Assert = NUnit.Framework.Assert;

namespace TestFx.SpecK.IntegrationTests
{
  public abstract class TestBase<T>
      where T : ISuite
  {
    protected const string Default = "<Default>";
    protected const string Action = "<Action>";
    protected const string Create_Subject = "<Create_Subject>";
    protected const string Reset_Instance_Fields = "<Reset_Instance_Fields>";
    protected const string Create_Fakes = "<Create_Fakes>";
    protected const string Setup_Fakes = "<Setup_Fakes>";
    protected const string Create_AutoData = "<Create_AutoData>";

    protected IFakeScope Scope;

    protected IRunResult RunResult;
    protected IList<ISuiteResult> SuiteResults; 
    protected IList<ITestResult> TestResults; 

    [SetUp]
    public virtual void SetUp ()
    {
      var runIntent = RunIntent.Create (useSeparateAppDomains: false);
      runIntent.AddType (typeof (T));

      using (Scope = Fake.CreateScope ())
      {
        RunResult = Evaluator.Run (runIntent);
      }

      SuiteResults = RunResult.SuiteResults.SelectMany (x => x.DescendantsAndSelf (y => y.SuiteResults)).ToList ();
      TestResults = SuiteResults.SelectMany (x => x.TestResults).ToList ();
    }

    [Test]
    public abstract void Test ();
    
    protected void AssertResult (IOperationResult result, string text, State state)
    {
      AssertResult (result, "<OPERATION>", text, state);
    }

    protected void AssertResult (IResult result, string relativeId, string text, State state)
    {
      result.Identity.Relative.Should ().Be (relativeId);
      result.Text.Should ().Be (text);
      result.State.Should ().Be (state);
    }

    protected void AssertResult (IOperationResult result, string text, State state, OperationType type)
    {
      AssertResult (result, text, state);
      result.Type.Should ().Be (type);
    }

    protected TestAssertion AssertTest (string testText, State state)
    {
      return GetTestResult (testText).HasState (state);
    }

    private TestAssertion GetTestResult (string text)
    {
      var testResult = TestResults.SingleOrDefault (x => x.Text == text);
      if (testResult == null)
        Assert.Fail ("Test '{0}' is not present.", text);
      return new TestAssertion (testResult);
    }

    protected class TestAssertion
    {
      readonly ITestResult _testResult;

      public TestAssertion (ITestResult testResult)
      {
        _testResult = testResult;
      }

      public TestAssertion HasState (State state)
      {
        Assert.That (_testResult.State, Is.EqualTo (state));
        return this;
      }

      public TestAssertion WithOperations (params string[] operationTexts)
      {
        var operations = _testResult.OperationResults;
        Assert.That (operations.Select (x => x.Text).ToArray (), Is.EqualTo (operationTexts), "Operations");
        return this;
      }

      public TestAssertion WithFailures (params string[] failureTexts)
      {
        var failures = _testResult.OperationResults.Where (x => x.State == State.Failed);
        Assert.That (failures.Select (x => x.Text).ToArray (), Is.EqualTo (failureTexts), "Failures");
        return this;
      }

      public TestAssertion WithFailureDetails (string failureText, string message)
      {
        var failure = GetFailure (failureText);

        var exception = failure.Exception.AssertNotNull ();
        if (message != null)
          Assert.That (exception.Message, Is.EqualTo (message));

        return this;
      }

      public TestAssertion WithFailureDetails (string failureText, Action<IExceptionDescriptor> exceptionAssertion)
      {
        var failure = GetFailure (failureText);

        exceptionAssertion (failure.Exception);

        return this;
      }

      private IOperationResult GetFailure (string failureText)
      {
        var failure = _testResult.OperationResults.SingleOrDefault (x => x.State == State.Failed && x.Text == failureText);
        if (failure == null)
          Assert.Fail ("Failure '{0}' is not present.", failureText);
        return failure;
      }
    }
  }
}