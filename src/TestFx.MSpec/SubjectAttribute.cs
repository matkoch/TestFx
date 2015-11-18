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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Collections;
using TestFx.Utilities.Reflection;

namespace TestFx.MSpec
{
  [MeansImplicitUse]
  [TypeLoaderType(typeof(TypeLoader))]
  [OperationOrdering(typeof(Operation))]
  public class SubjectAttribute : SuiteAttributeBase
  {
    [UsedImplicitly]
    [DisplayFormat ("{0}, {type}")]
    public SubjectAttribute (Type type)
    {
    }

    [UsedImplicitly]
    [DisplayFormat ("{0}")]
    public SubjectAttribute (string text)
    {
    }
  }

  public interface Operation : IActionDescriptor
  {
  }

  public class TypeLoader : TypeLoaderBase
  {
    public TypeLoader (IIntrospectionPresenter introspectionPresenter)
        : base(introspectionPresenter)
    {
    }

    protected override void InitializeTypeSpecificFields (object suite, SuiteProvider provider)
    {
      var suiteType = suite.GetType();
      var actions = GetFieldsStartingFromBase<Because>(suite, suiteType).ToList();
      Trace.Assert(actions.Count == 1, "No 'Because' action provided.");
      var setups = GetFieldsStartingFromBase<Establish>(suite, suiteType).Concat(actions.Single());
      var cleanups = GetFieldsStartingFromBase<Cleanup>(suite, suiteType).Reverse();
      var assertions = GetFieldsStartingFromBase<It>(suite, suiteType);

      var cleanupOperationProvider = OperationProvider.Create<Operation>(
          OperationType.Action,
          "Cleanup",
          () => cleanups.Select(x => CreateAction(x, suite)).ForEach(x => x()));
      var setupOperationProvider = OperationProvider.Create<Operation>(
          OperationType.Action,
          "Establish",
          () => setups.Select(x => CreateAction(x, suite)).ForEach(x => x()),
          cleanupOperationProvider);
      var testProviders = assertions.Select(x => CreateTestProvider(suite, provider, x));

      provider.ContextProviders = new[] { setupOperationProvider };
      provider.TestProviders = testProviders;
    }

    private TestProvider CreateTestProvider (object suite, SuiteProvider provider, FieldInfo x)
    {
      var text = x.Name.Replace("_", " ");
      var testProvider = TestProvider.Create(provider.Identity.CreateChildIdentity(text), text, ignored: false);
      var assertion = OperationProvider.Create<Operation>(OperationType.Assertion, text, CreateAction(x, suite));
      testProvider.OperationProviders = new[] { assertion };
      return testProvider;
    }

    private IEnumerable<FieldInfo> GetFieldsStartingFromBase<T> (object suite, Type suiteType)
    {
      return suiteType
          .DescendantsAndSelf(x => x.BaseType)
          .Reverse()
          .SelectMany(x => x.GetFields(MemberBindings.All | BindingFlags.DeclaredOnly))
          .Where(x => typeof (T).IsAssignableFrom(x.FieldType))
          .Where(x => !x.IsCompilerGenerated());
    }

    public Action CreateAction(FieldInfo field, object suite)
    {
      return () => ((Delegate) field.GetValue(suite)).DynamicInvoke();
    }
  }
}