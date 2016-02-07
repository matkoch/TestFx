// Copyright 2016, 2015, 2014 Matthias Koch
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
using JetBrains.Annotations;
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

    public override void OnSuiteStarted (IIntent intent)
    {
      IfTaskExists(intent, Started);
    }

    public override void OnTestStarted (IIntent intent)
    {
      IfTaskExists(intent, Started);
    }

    public override void OnTestFinished (ITestResult result)
    {
      IfTaskExists(result, x => Finished(result, x));
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      IfTaskExists(result, x => Finished(result, x));
    }

    private void Started (Task task)
    {
      _server.TaskStarting(task);
    }

    private void Finished (ISuiteResult result, Task task)
    {
      var operations = MergeSetupsAndCleanups(result);
      Finished(result, operations, result.OutputEntries, task);

      result.SuiteResults.Where(TaskDoesNotExist).ForEach(x => CreateDynamicTask(task.Id, x));
      result.TestResults.Where(TaskDoesNotExist).ForEach(x => CreateDynamicTask(task.Id, x));
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

    // TODO: repetition
    private void CreateDynamicTask (string parentGuid, ISuiteResult result)
    {
      var dynamicTask = new DynamicTask(parentGuid, result.Identity, result.Text);
      _server.CreateDynamicElement(dynamicTask);
      Finished(result, dynamicTask);
    }

    private void CreateDynamicTask (string parentGuid, ITestResult result)
    {
      var dynamicTask = new DynamicTask(parentGuid, result.Identity, result.Text);
      _server.CreateDynamicElement(dynamicTask);
      Finished(result, dynamicTask);
    }

    private void IfTaskExists<T> (T node, Action<Task> action) where T : IIdentifiable
    {
      var task = GetTask(node.Identity);
      if (task != null)
        action(task);
    }

    private bool TaskDoesNotExist (IResult result)
    {
      return GetTask(result.Identity) == null;
    }

    [CanBeNull]
    private Task GetTask (IIdentity identity)
    {
      Task task;
      return _taskDictionary.TryGetValue(identity.Absolute, out task) ? task : null;
    }
  }
}