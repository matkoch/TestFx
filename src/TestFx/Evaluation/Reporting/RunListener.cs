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
using System.Text;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;
using TestFx.Utilities;

namespace TestFx.Evaluation.Reporting
{
  public interface IRunListener
  {
    void OnRunStarted (IRunIntent intent);
    void OnRunFinished (IRunResult result);

    void OnSuiteStarted (ISuiteIntent intent);
    void OnSuiteFinished (ISuiteResult result);

    void OnTestStarted (ITestIntent intent);
    void OnTestFinished (ITestResult result);

    void OnError (IExceptionDescriptor exception);
  }

  public class RunListener : IRunListener
  {
    public virtual void OnRunStarted (IRunIntent intent)
    {
    }

    public virtual void OnRunFinished (IRunResult result)
    {
    }

    public virtual void OnSuiteStarted (ISuiteIntent intent)
    {
    }

    public virtual void OnSuiteFinished (ISuiteResult result)
    {
    }

    public virtual void OnTestStarted (ITestIntent intent)
    {
    }

    public virtual void OnTestFinished (ITestResult result)
    {
    }

    public virtual void OnError (IExceptionDescriptor exception)
    {
    }

    protected IEnumerable<IOperationResult> MergeSetupsAndCleanups (ISuiteResult result)
    {
      return result.SetupResults.Concat(new FillingOperationResult()).Concat(result.CleanupResults);
    }

    protected IEnumerable<IExceptionDescriptor> GetExceptions (IEnumerable<IOperationResult> operations)
    {
      return operations.Select(x => x.Exception).WhereNotNull();
    }

    protected string GetGeneralMessage (IList<IExceptionDescriptor> exceptions, IEnumerable<IOperationResult> operations)
    {
      return exceptions.Count == 0
          ? operations.Count(x => !(x is FillingOperationResult)) + " Operations"
          : exceptions.Count == 1
              ? exceptions[0].Name
              : exceptions.Count + " Exceptions";
    }

    protected string GetDetails (
        IEnumerable<IOperationResult> results,
        IEnumerable<OutputEntry> entries,
        IEnumerable<IExceptionDescriptor> exceptions = null)
    {
      var builder = new StringBuilder();

      builder.AppendLine("Operations:");
      foreach (var result in results)
      {
        if (result is FillingOperationResult)
        {
          builder.AppendLine(".. InnerOperations ..");
          continue;
        }

        builder.AppendFormat("{0} {1}", result.GetSymbol(), result.Text);

        if (result.Exception != null)
          builder.AppendFormat(" ({0})", result.Exception.Name);

        builder.Append("\r\n");
      }

      var entriesList = entries.ToList();
      if (entriesList.Count != 0)
      {
        builder.AppendLine().AppendLine("Output:");
        entriesList.ForEach(x => builder.AppendFormat("[{0}] {1}\r\n", x.Type.ToString(), x.Message));
      }

      if (exceptions != null)
      {
        foreach (var exception in exceptions)
        {
          builder.AppendLine().AppendLine();

          builder.AppendFormat("{0}:", exception.FullName).AppendLine();
          builder.Append(exception.Message).AppendLine();
          builder.Append(exception.StackTrace);
        }
      }

      return builder.ToString();
    }
  }
}