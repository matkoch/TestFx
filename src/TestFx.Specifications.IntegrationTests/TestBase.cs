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
using TestFx.Specifications.Implementation;
using TestFx.Utilities;
using Assert = NUnit.Framework.Assert;

namespace TestFx.Specifications.IntegrationTests
{
  public abstract class TestBase<T>
      where T : ISpecK
  {
    protected IFakeScope Scope;

    protected IRunResult RunResult;
    protected IList<ISuiteResult> AssemblyResults;
    protected IList<ISuiteResult> TypeResults;
    protected IList<ITestResult> TestResults;
    protected IList<IOperationResult> OperationResults;

    [SetUp]
    public virtual void SetUp ()
    {
      var runIntent = RunIntent.Create (useSeparateAppDomains: false);
      runIntent.AddType (typeof (T));

      using (Scope = Fake.CreateScope ())
      {
        RunResult = Evaluator.Run (runIntent);
      }

      AssemblyResults = RunResult.SuiteResults.ToList ();
      TypeResults = AssemblyResults.SelectMany (x => x.SuiteResults).ToList ();
      TestResults = TypeResults.SelectMany (x => x.TestResults).ToList ();
      OperationResults = TestResults.SelectMany (x => x.OperationResults).ToList ();
    }

    [Test]
    public abstract void Test ();

    protected ITestResult AssertTestPassed (string text, params string[] operationTexts)
    {
      return AssertTest (text, State.Passed, operationTexts);
    }

    protected ITestResult AssertTestFailed (string text, string[] operationTexts = null, string[] failedOperationTexts = null)
    {
      var testResult = AssertTest (text, State.Failed, operationTexts);

      if (failedOperationTexts != null)
        AssertFailedOperations (testResult, failedOperationTexts);

      return testResult;
    }

    protected IExceptionDescriptor GetFailedException(ITestResult testResult, string operationText)
    {
      var operationResult = testResult.OperationResults.SingleOrDefault (x => x.Text == operationText);
      if (operationResult == null)
        Assert.Fail ("Operation '{0}' is not present.", operationText);

      return operationResult.Exception.AssertNotNull ();
    }

    protected IOperationResult[] AssertFailedOperations (ITestResult testResult, params string[] operationTexts)
    {
      var failedOperations = testResult.OperationResults.Where (x => x.State == State.Failed).ToArray ();
      Assert.That (failedOperations.Select (x => x.Text).ToArray (), Is.EqualTo (operationTexts));
      return failedOperations;
    }

    protected ITestResult AssertTest (string text, State state, params string[] operationTexts)
    {
      var testResult = TestResults.SingleOrDefault (x => x.Text == text);
      if (testResult == null)
        Assert.Fail ("Test '{0}' is not present.", text);

      Assert.That (testResult.State, Is.EqualTo (state));
      if (operationTexts != null)
        Assert.That (testResult.OperationResults.Select (x => x.Text).ToArray (), Is.EqualTo (operationTexts));

      return testResult;
    }


    //protected IOperationResult AssertOperation(string text, State? state = null, OperationType? type = null)
    //{
    //  var operationResults = OperationResults.Where (x => x.Text == text).ToList ();
    //  if (operationResults.Count != 1)
    //    Assert.Fail ("Operation '{0}' ist not uniquely existent.", text);

    //  var operationResult = operationResults.Single ();
    //  if (state != null)
    //    Assert.That (operationResult.State, Is.EqualTo (state));
    //  if (type != null)
    //    Assert.That (operationResult.Type, Is.EqualTo (type));

    //  return operationResult;
    //}

    protected void AssertResult (IResult result, string relativeIdAndText, State state)
    {
      AssertResult (result, relativeIdAndText, relativeIdAndText, state);
    }

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
  }
}