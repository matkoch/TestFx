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
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Old.OldEval;
using TestFx.Old.OldEval.Reporting;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner
{
  public class RemoteTaskServerListener : DefaultListener
  {
    private readonly IRemoteTaskServer _server;
    private readonly Dictionary<string, Task> _taskDictionary;

    public RemoteTaskServerListener (IRemoteTaskServer server, Dictionary<string, Task> taskDictionary)
    {
      _server = server;
      _taskDictionary = taskDictionary;
    }

    public override void OnBeginGroup (GroupIntent groupIntent)
    {
      var task = TryGetTask(groupIntent);
      if (task != null)
        _server.TaskStarting(task);
    }

    public override void OnEndGroup (GroupResult groupResult)
    {
      var task = TryGetTask(groupResult);
      if (task == null)
        return;

      ReportResult(task, groupResult.State, groupResult.Exception, groupResult.Name);
      
      foreach (var group in groupResult.Groups)
      {
        var groupTask = TryGetTask(group);
        if (groupTask == null)
        {
          groupTask = new GroupTask(group.GetAbsoluteId(), group.Id, task.AbsoluteId, group.Name);
          _server.CreateDynamicElement(groupTask);
          _taskDictionary.Add(groupTask.AbsoluteId, groupTask);
          OnEndGroup(group);
        }
      }

      foreach (var test in groupResult.Tests)
      {
        var testTask = TryGetTask(test);
        if (testTask == null)
        {
          testTask = new TestTask(test.GetAbsoluteId(), test.Id, task.AbsoluteId, test.Name);
          _server.CreateDynamicElement(testTask);
          _taskDictionary.Add(testTask.AbsoluteId, testTask);
          OnEndTest(test);
        }
      }
    }

    public override void OnBeginTest (TestIntent testIntent)
    {
      var task = TryGetTask(testIntent);
      if (task != null)
        _server.TaskStarting(task);
    }

    public override void OnEndTest (TestResult testResult)
    {
      _server.TaskOutput(null, "bla", TaskOutputType.STDOUT);

      var task = TryGetTask(testResult);
      if (task == null)
        return;

      ReportResult(task, testResult.State, testResult.Exception, testResult.Name);

      foreach (var operation in testResult.Operations)
      {
        var operationTask = new OperationTask(operation.GetAbsoluteId(), operation.Id, task.AbsoluteId, operation.Name, operation.Type);
        _server.CreateDynamicElement(operationTask);

        ReportResult(operationTask, operation.State, operation.Exception, operation.Name);
      }
    }

    [CanBeNull]
    private Task TryGetTask (IIdentifiable identifiable)
    {
      Task task;
      return _taskDictionary.TryGetValue(identifiable.GetAbsoluteId(), out task) ? task : null;
    }

    private void ReportResult (RemoteTask task, State state, ExceptionDescriptor exception, string desc)
    {
      if (exception == null)
      {
        _server.TaskFinished(task, string.Empty, state.ToTaskResult());
      }
      else
      {
        _server.TaskOutput(task, "MUUHU", TaskOutputType.STDERR);
        _server.TaskOutput(task, "MUUHU", TaskOutputType.DEBUGTRACE);
        _server.TaskOutput(task, "\u2713", TaskOutputType.STDOUT);
        _server.TaskOutput(task, "\u2717", TaskOutputType.STDOUT);
        _server.TaskException(task, new[] { exception.ToTaskException(desc) });

        _server.TaskOutput(task, "xxxxxxxxxxxxxxxxxxxxxxxxxx", TaskOutputType.STDOUT);
        _server.TaskException(task, new[] { exception.ToTaskException(desc) });

        _server.TaskFinished(task, exception.Type.Name, state.ToTaskResult());
      }
    }
  }
}