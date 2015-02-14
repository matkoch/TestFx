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
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner
{
  public class ReSharperRunListener : RunListener
  {
    private readonly IRemoteTaskServer _server;
    private readonly IDictionary<string, Task> _taskDictionary;

    public ReSharperRunListener (IRemoteTaskServer server, IDictionary<string, Task> taskDictionary)
    {
      _server = server;
      _taskDictionary = taskDictionary;
    }

    public override void OnSuiteStarted (ISuiteIntent intent)
    {
      IfTaskExists(Started, intent);
    }

    public override void OnTestStarted (ITestIntent intent)
    {
      IfTaskExists(Started, intent);
    }

    public override void OnTestFinished (ITestResult result)
    {
      IfTaskExists(Finished, result);
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      IfTaskExists(Finished, result);
    }

    private void Started (IIntent intent, Task task)
    {
      _server.TaskStarting(task);
    }

    private void Finished (ISuiteResult result, Task task)
    {
      var operationResults = result.SetupResults.Concat(new FillingOperationResult()).Concat(result.CleanupResults);
      Finished(result, operationResults, result.OutputEntries, task);

      result.SuiteResults.Where(TaskDoesNotExist).ForEach(CreateDynamicTask);
      result.TestResults.Where(TaskDoesNotExist).ForEach(CreateDynamicTask);
    }

    private void Finished (ITestResult result, Task task)
    {
      Finished(result, result.OperationResults, result.OutputEntries, task);
    }

    private void Finished (IResult result, IEnumerable<IOperationResult> operationResults, IEnumerable<OutputEntry> entries, Task task)
    {
      var operationResultsList = operationResults.ToList();
      var exceptions = operationResultsList.Select(x => x.Exception).WhereNotNull().Select(x => x.ToTaskException()).ToArray();
      var message = exceptions.Length == 0
          ? operationResultsList.Count(x => !(x is FillingOperationResult)) + " Operations"
          : exceptions.Length == 1
              ? exceptions[0].Type
              : exceptions.Length + " Exceptions";

      _server.TaskOutput(task, GetOutput(operationResultsList, entries.ToList()), TaskOutputType.STDOUT);
      _server.TaskException(task, exceptions);
      _server.TaskFinished(task, message, result.GetTaskResult());
    }

    private string GetOutput (IEnumerable<IOperationResult> results, IEnumerable<OutputEntry> entries)
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

      return builder.ToString();
    }

    private void CreateDynamicTask (ISuiteResult result)
    {
      var dynamicTask = new DynamicTask(typeof (SuiteTask), result.Identity, result.Text);
      _server.CreateDynamicElement(dynamicTask);
      Finished(result, dynamicTask);
    }

    private void CreateDynamicTask (ITestResult result)
    {
      var dynamicTask = new DynamicTask(typeof (TestTask), result.Identity, result.Text);
      _server.CreateDynamicElement(dynamicTask);
      Finished(result, dynamicTask);
    }

    private void IfTaskExists<T> (Action<T, Task> action, T node) where T : IIdentifiable
    {
      var task = GetTask(node.Identity);
      if (task != null)
        action(node, task);
    }

    private bool TaskDoesNotExist (IResult result)
    {
      var task = GetTask(result.Identity);
      return task == null;
    }

    private Task GetTask (IIdentity identity)
    {
      Task task;
      return _taskDictionary.TryGetValue(identity.Absolute, out task) ? task : null;
    }
  }
}