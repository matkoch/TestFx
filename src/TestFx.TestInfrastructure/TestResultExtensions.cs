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
using System.Linq;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.TestInfrastructure
{
  public static class TestResultExtensions
  {
    public static ITestResult HasOperations (this ITestResult testResult, params string[] operations)
    {
      var operationResults = testResult.OperationResults;
      Assert.That(operationResults.Select(x => x.Text).ToArray(), Is.EqualTo(operations), "Operations");
      return testResult;
    }

    public static ITestResult HasFailingOperations (this ITestResult testResult, params string[] failingOperations)
    {
      var operationResults = testResult.OperationResults.Where(x => x.State == State.Failed);
      Assert.That(operationResults.Select(x => x.Text).ToArray(), Is.EqualTo(failingOperations), "Operations");
      return testResult;
    }

    public static ITestResult HasFailingOperation (this ITestResult testResult, string failingOperation, string message = null)
    {
      var failure = GetFailingOperation(testResult, failingOperation);

      var exception = failure.Exception.NotNull();
      if (message != null)
        Assert.That(exception.Message, Is.EqualTo(message));

      return testResult;
    }

    public static ITestResult HasFailingOperation (
        this ITestResult testResult,
        string failingOperation,
        Action<IExceptionDescriptor> exceptionAssertion)
    {
      var failure = GetFailingOperation(testResult, failingOperation);
      exceptionAssertion(failure.Exception);

      return testResult;
    }

    private static IOperationResult GetFailingOperation (ITestResult testResult, string failingOperation)
    {
      var operationResults = testResult.OperationResults.Where(x => x.State == State.Failed && x.Text == failingOperation).ToList();
      if (operationResults.Count != 1)
        Assert.Fail("There is no single failing operation with text '{0}'.", failingOperation);

      return operationResults.Single();
    }
  }
}