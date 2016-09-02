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
using System.Reflection;
using Autofac.Core;
using Autofac.Core.Resolving;

namespace TestFx.Evaluation.Utilities
{
  public static class AutofacExtensions
  {
    private static readonly FieldInfo s_contextFieldInfo;
    private static readonly FieldInfo s_activationStackFieldInfo;

    static AutofacExtensions ()
    {
      var autofacAssembly = typeof (IInstanceLookup).Assembly;
      var instanceLookupType = autofacAssembly.GetType("Autofac.Core.Resolving.InstanceLookup");
      s_contextFieldInfo = instanceLookupType.GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic);
      var resolveOperationType = autofacAssembly.GetType("Autofac.Core.Resolving.ResolveOperation");
      s_activationStackFieldInfo = resolveOperationType.GetField("_activationStack", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    /// <summary>
    /// Pass parameters from the top level resolve operation (typically a delegate factory call)
    /// to a nested component activation.
    /// </summary>
    public static void ForwardFactoryParameters (PreparingEventArgs e)
    {
      var instanceLookup = (IInstanceLookup) e.Context;
      var resolveOperation = (IResolveOperation) s_contextFieldInfo.GetValue(instanceLookup);
      var activationStack = (IEnumerable<IInstanceLookup>) s_activationStackFieldInfo.GetValue(resolveOperation);

      e.Parameters = e.Parameters.Concat(activationStack.Last().Parameters);
    }
  }
}