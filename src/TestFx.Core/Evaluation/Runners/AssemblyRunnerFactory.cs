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
using TestFx.Evaluation.Loading;
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility;
using TestFx.Extensibility.Utilities;

namespace TestFx.Evaluation.Runners
{
  public interface IAssemblyRunnerFactory
  {
    IAssemblyRunner Create (IRunListener listener, IResourceManager resourceManager, ICancellationTokenSource cancellationTokenSource);
  }

  public class AssemblyRunnerFactory : MarshalByRefObject, IAssemblyRunnerFactory
  {
    public IAssemblyRunner Create (IRunListener listener, IResourceManager resourceManager, ICancellationTokenSource cancellationTokenSource)
    {
      var builder = new ContainerBuilder();
      builder.RegisterModule(new EvaluationModule(listener, useSeparateAppDomains: false /* don't care */));
      // Re-registering
      builder.RegisterInstance(resourceManager).As<IResourceManager>();
      builder.RegisterInstance(cancellationTokenSource).As<ICancellationTokenSource>();
      // Only SetupCommon's should be added for assembly suite
      builder.RegisterInstance(new OperationSorter(new[] { typeof (SetupCommon) })).As<IOperationSorter>();
      var container = builder.Build();
      return new AssemblyRunner(container.Resolve<IAssemblyLoader>(), container.Resolve<ISuiteRunner>());
    }
  }
}