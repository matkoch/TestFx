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

namespace TestFx.Specifications.IntegrationTests
{
  public abstract class TestBase<T>
      where T : ISpecK
  {
    protected IFakeScope Scope;

    protected IRunResult RunResult;
    protected IList<ISuiteResult> AssemblyResults;
    protected IList<ISuiteResult> TypeResults;
    protected IList<ISuiteResult> ExpressionResults;
    protected IList<ITestResult> TestResults;
    protected IList<IOperationResult> OperationResults;

    [SetUp]
    public void Setup ()
    {
      var runIntent = RunIntent.Create (useSeparateAppDomains: false);
      runIntent.AddTypes (typeof (T));

      using (Scope = Fake.CreateScope ())
      {
        RunResult = Evaluator.Run (runIntent);
      }

      AssemblyResults = RunResult.SuiteResults.ToList ();
      TypeResults = AssemblyResults.SelectMany (x => x.SuiteResults).ToList ();
      ExpressionResults = TypeResults.SelectMany (x => x.SuiteResults).ToList ();
      TestResults = ExpressionResults.SelectMany (x => x.TestResults).ToList ();
      TestResults = ExpressionResults.SelectMany (x => x.TestResults).ToList ();
      OperationResults = TestResults.SelectMany (x => x.OperationResults).ToList ();
    }

    protected void AssertResult (IResult result, string relativeId, string text, State state)
    {
      result.Identity.Relative.Should ().Be (relativeId);
      result.Text.Should ().Be (text);
      result.State.Should ().Be (state);
    }

    protected void AssertResult (IOperationResult result, string relativeId, string text, State state, OperationType type)
    {
      AssertResult (result, relativeId, text, state);
      result.Type.Should ().Be (type);
    }
  }
}