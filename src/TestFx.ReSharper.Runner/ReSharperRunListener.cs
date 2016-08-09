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
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner
{
  internal class ReSharperRunListener : RunListener
  {
    private readonly IRemoteTaskServer _server;
    private readonly IDictionary<IIdentity, Task> _taskDictionary;

    public ReSharperRunListener (IRemoteTaskServer server, IDictionary<IIdentity, Task> taskDictionary)
    {
      _server = server;
      _taskDictionary = taskDictionary;
    }

    public override void OnSuiteStarted (IIntent intent, string text)
    {
      Started(intent, text);
    }

    public override void OnTestStarted (IIntent intent, string text)
    {
      Started(intent, text);
    }

    public override void OnTestFinished (ITestResult result)
    {
      Finished(result);
    }

    public override void OnSuiteFinished (ISuiteResult result)
    {
      Finished(result);
    }

    private void Started (IIntent intent, string text)
    {
      Task task;
      if (!_taskDictionary.TryGetValue(intent.Identity, out task))
        task = CreateDynamicTask(intent, text);

      _server.TaskStarting(task);
    }

    private void Finished (IOutputResult result)
    {
      var task = _taskDictionary[result.Identity];

      // Don't process if task represents an assembly suite.
      if (!task.IsMeaningfulTask)
        return;

      _server.TaskOutput(task, result.GetDetailedSummary(includeExceptions: false), TaskOutputType.STDOUT);
      _server.TaskException(task, result.GetExceptions().ToList().Select(x => x.ToTaskException()).ToArray());
      _server.TaskFinished(task, result.GetBriefSummary(), result.GetTaskResult());
    }

    private Task CreateDynamicTask (IIntent intent, string text)
    {
      var parentTask = _taskDictionary[intent.Identity.Parent.NotNull()];
      var dynamicTask = new DynamicTask(parentTask.Id, intent.Identity, text);
      _server.CreateDynamicElement(dynamicTask);
      _taskDictionary.Add(intent.Identity, dynamicTask);
      return dynamicTask;
    }
  }
}