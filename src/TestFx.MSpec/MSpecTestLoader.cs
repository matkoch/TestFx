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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Machine.Specifications;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.MSpec.Implementation;
using TestFx.Utilities;
using TestFx.Utilities.Collections;
using TestFx.Utilities.Reflection;

namespace TestFx.MSpec
{
  [OperationOrdering (typeof (Operation))]
  public class MSpecTestLoader : TestLoaderBase
  {
    public MSpecTestLoader (IIntrospectionPresenter introspectionPresenter)
        : base(introspectionPresenter)
    {
    }

    protected override CustomAttributeData GetDisplayAttribute (Type suiteType)
    {
      return suiteType.DescendantsAndSelf(x => x.DeclaringType).Select(x => x.GetAttributeData<SuiteAttributeBase>())
          .WhereNotNull().FirstOrDefault();
    }

    protected override void Initialize (Type suiteType, object suite, SuiteProvider provider)
    {
      var hierarchyTypes = HierarchyLoader.GetExecutionHierarchy(suiteType).ToList();
      var behaviorTypes = suiteType.Descendants(x => GetBehaviorTypes(x)).ToList();

      var setupOperationProviders = GetSetupOperationProviders(hierarchyTypes, behaviorTypes, suite, suiteType);

      var assertionFields = GetFields<It>(suiteType).Concat(behaviorTypes.SelectMany(GetFields<It>));
      var testProviders = assertionFields.Select(x => CreateTestProvider(provider.Identity, GetInstance(x.DeclaringType.NotNull(), suite), x));

      provider.ContextProviders = setupOperationProviders;
      provider.TestProviders = testProviders;
    }

    private IEnumerable<Type> GetBehaviorTypes (Type typeWithBehavior)
    {
      return typeWithBehavior.GetFields(MemberBindings.All)
          .Where(x => x.FieldType.IsGenericType && x.FieldType.GetGenericTypeDefinition() == typeof (Behaves_like<>))
          .Where(x => !x.IsCompilerGenerated())
          .Select(x => x.FieldType.GetGenericArguments().Single());
    }

    private IEnumerable<IOperationProvider> GetSetupOperationProviders (
        IEnumerable<Type> hierarchyTypes,
        IEnumerable<Type> behaviorTypes,
        object suite,
        Type suiteType)
    {
      var establishAndCleanupOperationProviders = hierarchyTypes.Select(x => GetEstablishOperationProviderOrNull(x, GetInstance(x, suite)));

      var becauseFields = GetFields<Because>(suiteType).ToList();
      Trace.Assert(becauseFields.Count == 1, "No single 'Because' field provided.");
      var becauseOperationProvider = OperationProvider.Create<Operation>(OperationType.Action, "Because", CreateUnwrappingAction(becauseFields.Single(), suite));

      IOperationProvider fieldsCopyingOperationProvider = null;
      var behaviorTypeList = behaviorTypes.ToList();
      if (behaviorTypeList.Count > 0)
      {
        fieldsCopyingOperationProvider = OperationProvider.Create<Operation>(
            OperationType.Action,
            "<CopyBehaviorFields>",
            GetFieldsCopyingAction(suiteType, behaviorTypeList));
      }

      return establishAndCleanupOperationProviders
          .Concat(becauseOperationProvider)
          .Concat(fieldsCopyingOperationProvider)
          .WhereNotNull();
    }

    private object GetInstance (Type declaringType, object suite)
    {
      if (declaringType.IsAbstract)
      {
        var message = $"Type '{declaringType}' is contained in execution hierarchy of suite type '{suite.GetType()}' " +
                      $"but cannot be instantiated because it is marked as abstract.";
        throw new Exception(message);
      }

      return declaringType.IsInstanceOfType(suite) ? suite : declaringType.CreateInstance<object>();
    }

    [CanBeNull]
    private IOperationProvider GetEstablishOperationProviderOrNull (Type type, object instance)
    {
      var setupField = GetFields<Establish>(type).SingleOrDefault();
      var cleanupField = GetFields<Cleanup>(type).SingleOrDefault();

      if (setupField == null && cleanupField == null)
        return null;

      IOperationProvider cleanupProvider = null;
      if (cleanupField != null)
      {
        var cleanupAction = CreateUnwrappingAction(cleanupField, instance);
        cleanupProvider = OperationProvider.Create<Operation>(OperationType.Action, "Cleanup " + type.Name, cleanupAction);
      }

      var setupAction = setupField != null ? CreateUnwrappingAction(setupField, instance) : () => { };
      return OperationProvider.Create<Operation>(OperationType.Action, "Establish " + type.Name, setupAction, cleanupProvider);
    }

    private Action GetFieldsCopyingAction (Type suiteType, IEnumerable<Type> behaviorTypes)
    {
      return () =>
      {
        var suiteFields = suiteType.GetFields(MemberBindings.Static);
        var behaviorFields = behaviorTypes.ToList().SelectMany(GetFields<object>).ToLookup(x => x.Name);

        foreach (var suiteField in suiteFields)
        {
          foreach (var behaviorField in behaviorFields[suiteField.Name])
            behaviorField.SetValue(obj: null, value: suiteField.GetValue(obj: null));
        }
      };
    }

    private IEnumerable<FieldInfo> GetFields<T> (Type type)
    {
      return type
          .GetFields(MemberBindings.All | BindingFlags.DeclaredOnly)
          .Where(x => typeof (T).IsAssignableFrom(x.FieldType))
          .Where(x => !x.IsCompilerGenerated());
    }

    private TestProvider CreateTestProvider (IIdentity parentIdentity, object instance, FieldInfo actionField)
    {
      var text = actionField.Name.Replace("_", " ");
      var testProvider = TestProvider.Create(parentIdentity.CreateChildIdentity(actionField.Name), text, ignoreReason: null);
      var action = CreateUnwrappingAction(actionField, instance);
      var assertion = OperationProvider.Create<Operation>(OperationType.Assertion, text, action);
      testProvider.OperationProviders = new[] { assertion };
      return testProvider;
    }

    private Action CreateUnwrappingAction (FieldInfo fieldInfo, object instance)
    {
      return () =>
      {
        try
        {
          ((Delegate) fieldInfo.GetValue(instance)).DynamicInvoke();
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
      };
    }
  }
}