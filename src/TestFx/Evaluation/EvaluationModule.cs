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
using Autofac;
using JetBrains.Annotations;
using TestFx.Evaluation.Loading;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Runners;
using TestFx.Evaluation.Utilities;

namespace TestFx.Evaluation
{
  public class EvaluationModule : Module
  {
    private readonly bool _useSeparateAppDomains;
    private readonly IRunListener _listener;

    public EvaluationModule (IRunListener listener, bool useSeparateAppDomains)
    {
      _listener = listener;
      _useSeparateAppDomains = useSeparateAppDomains;
    }

    protected override void Load (ContainerBuilder builder)
    {
      // Loading
      builder.RegisterType<AssemblyExplorer>().As<IAssemblyExplorer>();
      builder.RegisterType<AssemblyLoader>().As<IAssemblyLoader>();
      builder.RegisterType<SuiteControllerFactory>().As<ISuiteControllerFactory>();

      // Results
      builder.RegisterType<ResultFactory>().As<IResultFactory>();

      // Reporting
      builder.RegisterInstance(_listener).As<IRunListener>();

      // Runners
      builder.RegisterType<RootRunner>().As<IRootRunner>();
      builder.RegisterType<AssemblyRunnerFactory>().As<IAssemblyRunnerFactory>();
      builder.RegisterType<ContextRunner>().As<IContextRunner>();
      builder.RegisterType<SuiteRunner>().As<ISuiteRunner>();
      builder.RegisterType<TestRunner>().As<ITestRunner>();
      builder.RegisterType<OperationRunner>().As<IOperationRunner>();

      // Utilities
      if (_useSeparateAppDomains)
        builder.RegisterType<AppDomainFactory>().As<IAppDomainFactory>();
      else
        builder.RegisterType<FakeAppDomainFactory>().As<IAppDomainFactory>();
      builder.RegisterType<ResourceManager>().As<IResourceManager>();
      builder.RegisterType<IntentProviderPairer>().As<IIntentProviderPairer>();
    }
  }
}