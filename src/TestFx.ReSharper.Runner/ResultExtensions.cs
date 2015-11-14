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
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Evaluation.Results;

namespace TestFx.ReSharper.Runner
{
  public static class ResultExtensions
  {
    private static readonly Dictionary<State, TaskResult> s_taskResults;

    static ResultExtensions ()
    {
      s_taskResults = new Dictionary<State, TaskResult>
                      {
                          { State.Passed, TaskResult.Success },
                          { State.Ignored, TaskResult.Skipped },
                          { State.Inconclusive, TaskResult.Inconclusive },
                          { State.NotImplemented, TaskResult.Inconclusive },
                          { State.Failed, TaskResult.Exception }
                      };
    }

    public static TaskResult GetTaskResult (this IResult result)
    {
      return s_taskResults[result.State];
    }
  }
}