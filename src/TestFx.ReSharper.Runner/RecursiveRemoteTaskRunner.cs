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

extern alias mscorlib;
using System;
using System.Linq;
using JetBrains.ReSharper.TaskRunnerFramework;
using mscorlib::System.Threading;
using TestFx.Evaluation;
using TestFx.Evaluation.Intents;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner
{
  public class RecursiveRemoteTaskRunner : JetBrains.ReSharper.TaskRunnerFramework.RecursiveRemoteTaskRunner
  {
    public static readonly string ID = typeof (RecursiveRemoteTaskRunner).FullName;

    private CancellationTokenSource _cancellationTokenSource;

    public RecursiveRemoteTaskRunner (IRemoteTaskServer server)
        : base(server)
    {
    }

    public override void ExecuteRecursive (TaskExecutionNode node)
    {
      var runIntent = CreateRunIntent(node);
      var taskDictionary = node.DescendantsAndSelf(x => x.Children)
          .Select(x => x.RemoteTask)
          .Cast<Task>()
          .ToDictionary(x => x.Identity.Absolute, x => x);
      var listener = new ReSharperRunListener(Server, taskDictionary);

      try
      {
        var result = Evaluator.Run(runIntent, listener);
      }
      catch (Exception)
      {
        Server.SetTempFolderPath(runIntent.ShadowCopyPath);
        throw;
      }
    }

    public override void Abort ()
    {
      _cancellationTokenSource.Cancel();
    }

    private IRunIntent CreateRunIntent (TaskExecutionNode node)
    {
      var configuration = TaskExecutor.Configuration;

      var runTask = node.RemoteTask.To<RunTask>();
      var runIntent = RunIntent.Create(configuration.SeparateAppDomain, configuration.ShadowCopy, runTask.VisualStudioProcessId);
      _cancellationTokenSource = runIntent.CancellationTokenSource;

      node.Children.Where(x => x.RemoteTask is SuiteTask).Select(CreateSuiteIntent).ForEach(runIntent.AddSuiteIntent);

      return runIntent;
    }

    private ISuiteIntent CreateSuiteIntent (TaskExecutionNode node)
    {
      var suiteTask = node.RemoteTask.To<SuiteTask>();
      var suiteIntent = SuiteIntent.Create(suiteTask.Identity);

      node.Children.Where(x => x.RemoteTask is SuiteTask).Select(CreateSuiteIntent).ForEach(suiteIntent.AddSuiteIntent);
      node.Children.Where(x => x.RemoteTask is TestTask).Select(CreateTestIntent).ForEach(suiteIntent.AddTestIntent);

      return suiteIntent;
    }

    private ITestIntent CreateTestIntent (TaskExecutionNode node)
    {
      var testTask = node.RemoteTask.To<TestTask>();
      return TestIntent.Create(testTask.Identity);
    }
  }
}