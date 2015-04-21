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
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

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
      var operations = MergeSetupsAndCleanups(result);
      Finished(result, operations, result.OutputEntries, task);

      result.SuiteResults.Where(TaskDoesNotExist).ForEach(CreateDynamicTask);
      result.TestResults.Where(TaskDoesNotExist).ForEach(CreateDynamicTask);
    }

    private void Finished (ITestResult result, Task task)
    {
      Finished(result, result.OperationResults, result.OutputEntries, task);
    }

    private void Finished (IResult result, IEnumerable<IOperationResult> operationResults, IEnumerable<OutputEntry> entries, Task task)
    {
      if (!task.IsMeaningfulTask)
          // Affects results representing the assembly.
        return;

      var operations = operationResults.ToList();
      var exceptions = GetExceptions(operations).ToList();

      var message = GetGeneralMessage(exceptions, operations);
      var details = GetDetails(operations, entries);

      _server.TaskOutput(task, details, TaskOutputType.STDOUT);
      _server.TaskException(task, exceptions.Select(x => x.ToTaskException()).ToArray());
      _server.TaskFinished(task, message, result.GetTaskResult());
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