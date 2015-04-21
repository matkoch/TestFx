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
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace TestFx.Utilities
{
  public class NamespaceModule : Module
  {
    private readonly Assembly _assembly;
    private readonly string _namespace;

    public NamespaceModule (Type typeInNamespace)
        : this(typeInNamespace.Assembly, typeInNamespace.Namespace)
    {
    }

    public NamespaceModule (Assembly assembly, string @namespace)
    {
      _assembly = assembly;
      _namespace = @namespace;
    }

    protected override void Load (ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(_assembly).InNamespace(_namespace)
          .AsImplementedInterfaces()
          .SingleInstance();
    }
  }
}