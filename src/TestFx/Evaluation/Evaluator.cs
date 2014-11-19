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
using System.Reflection;
using Autofac;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Runners;

namespace TestFx.Evaluation
{
  public static class Evaluator
  {
    public static IRunResult Run (IRunIntent runIntent, params IRunListener[] listeners)
    {
      var listener = CrossAppDomainRunListener.Create(listeners);

      var builder = new ContainerBuilder();
      var evaluationModule = new EvaluationModule(listener, runIntent.CreateSeparateAppDomains);
      builder.RegisterModule(evaluationModule);
      var container = builder.Build();

      var rootRunner = container.Resolve<IRootRunner>();
      return rootRunner.Run(runIntent);
    }

    public static IRunResult Run (params Assembly[] assemblies)
    {
      var runIntent = RunIntent.Create();
      runIntent.AddAssemblies(assemblies);
      return Run(runIntent);
    }

    public static IRunResult Run (params Type[] types)
    {
      var runIntent = RunIntent.Create();
      runIntent.AddTypes(types);
      return Run(runIntent);
    }
  }
}