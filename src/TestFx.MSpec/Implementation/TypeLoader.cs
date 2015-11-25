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
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Collections;
using TestFx.Utilities.Reflection;

namespace TestFx.MSpec.Implementation
{
  public class TypeLoader : TypeLoaderBase
  {
    public TypeLoader (IIntrospectionPresenter introspectionPresenter)
        : base(introspectionPresenter)
    {
    }

    protected override void InitializeTypeSpecificFields (object suite, SuiteProvider provider)
    {
      var suiteType = suite.GetType();

      var actionFields = GetFieldsStartingFromBase<Because>(suiteType).ToList();
      Trace.Assert(actionFields.Count == 1, "No 'Because' action provided.");
      var setupFields = GetFieldsStartingFromBase<Establish>(suiteType).Concat(actionFields.Single());
      var cleanupFields = GetFieldsStartingFromBase<Cleanup>(suiteType).Reverse();
      var assertionFields = GetFieldsStartingFromBase<It>(suiteType);

      var cleanupOperationProvider = CreateContextOperationProvider(suite, "Cleanup", cleanupFields);
      var setupOperationProvider = CreateContextOperationProvider(suite, "Establish", setupFields, cleanupOperationProvider);
      var testProviders = assertionFields.Select(x => CreateTestProvider(provider.Identity, suite, x));

      provider.ContextProviders = new[] { setupOperationProvider }.WhereNotNull();
      provider.TestProviders = testProviders;
    }

    private OperationProvider CreateContextOperationProvider (
        object suite,
        string text,
        IEnumerable<FieldInfo> actionFields,
        OperationProvider cleanupProvider = null)
    {
      var actionFieldList = actionFields.ToList();
      if (!actionFieldList.Any())
        return null;

      return OperationProvider.Create<Operation>(
          OperationType.Action,
          text,
          action: () => actionFieldList.Select(x => CreateAction(x, suite)).ForEach(x => x()),
          cleanupProvider: cleanupProvider);
    }

    private TestProvider CreateTestProvider (IIdentity parentIdentity, object suite, FieldInfo actionField)
    {
      var text = actionField.Name.Replace("_", " ");
      var testProvider = TestProvider.Create(parentIdentity.CreateChildIdentity(actionField.Name), text, ignored: false);
      var assertion = OperationProvider.Create<Operation>(OperationType.Assertion, text, CreateAction(actionField, suite));
      testProvider.OperationProviders = new[] { assertion };
      return testProvider;
    }

    private IEnumerable<FieldInfo> GetFieldsStartingFromBase<T> (Type suiteType)
    {
      return suiteType
          .DescendantsAndSelf(x => x.BaseType)
          .Reverse()
          .SelectMany(x => x.GetFields(MemberBindings.All | BindingFlags.DeclaredOnly))
          .Where(x => typeof (T).IsAssignableFrom(x.FieldType))
          .Where(x => !x.IsCompilerGenerated());
    }

    public Action CreateAction (FieldInfo field, object suite)
    {
      return () => ((Delegate) field.GetValue(suite)).DynamicInvoke();
    }
  }
}