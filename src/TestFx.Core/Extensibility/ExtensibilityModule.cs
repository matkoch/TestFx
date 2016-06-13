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
using System.Linq;
using Autofac;
using JetBrains.Annotations;
using TestFx.Evaluation.Loading;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Utilities;

namespace TestFx.Extensibility
{
  internal class ExtensibilityModule : Module
  {
    private readonly Type _testLoaderType;
    private readonly Type[] _operationOrdering;

    public ExtensibilityModule (Type testLoaderType, Type[] operationOrdering)
    {
      _testLoaderType = testLoaderType;
      _operationOrdering = operationOrdering;
    }

    protected override void Load ([NotNull] ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(_testLoaderType.Assembly)
          .AsImplementedInterfaces()
          .OnPreparing(AutofacExtensions.ForwardFactoryParameters);
      builder.RegisterType(_testLoaderType).As<ITestLoader>();
      builder.Register(ctx => new OperationSorter(_operationOrdering)).As<IOperationSorter>();
      builder.RegisterType<IntrospectionPresenter>().As<IIntrospectionPresenter>();
    }
  }
}