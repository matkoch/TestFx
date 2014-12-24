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
using System.Linq;
using System.Reflection;
using TestFx.Evaluation.Intents;
using TestFx.Utilities;

namespace TestFx.Evaluation
{
  public static class RunIntentExtensions
  {
    public static void AddAssemblies (this IRunIntent runIntent, params Assembly[] assemblies)
    {
      assemblies.Select(GetIdentity).Select(SuiteIntent.Create).ForEach(runIntent.AddSuiteIntent);
    }

    public static void AddTypes (this IRunIntent runIntent, params Type[] types)
    {
      foreach (var assemblyWithTypes in types.GroupBy(x => x.Assembly))
      {
        var suiteIntent = SuiteIntent.Create(GetIdentity(assemblyWithTypes.Key));
        runIntent.AddSuiteIntent(suiteIntent);
        assemblyWithTypes.ForEach(x => suiteIntent.AddSuiteIntent(SuiteIntent.Create(GetIdentity(x))));
      }
    }

    private static IIdentity GetIdentity (Assembly assembly)
    {
      return Identity.Parse(assembly.Location);
    }

    private static IIdentity GetIdentity (Type type)
    {
      return new Identity(type.FullName, GetIdentity(type.Assembly));
    }
  }
}