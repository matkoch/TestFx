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
using TestFx.Evaluation;
using TestFx.Evaluation.Loading;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Extensibility
{
  public abstract class TestLoaderBase : ITestLoader
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;

    protected TestLoaderBase (IIntrospectionPresenter introspectionPresenter)
    {
      _introspectionPresenter = introspectionPresenter;
    }

    [CanBeNull]
    public ISuiteProvider Load (object suite, IDictionary<Type, Lazy<IAssemblySetup>> assemblySetups, IIdentity assemblyIdentity)
    {
      var suiteType = suite.GetType();
      Trace.Assert(suiteType != typeof (Type));

      var provider = CreateSuiteProvider(suiteType, assemblyIdentity);
      if (provider == null)
        return null;
      
      InitializeAssemblySetupFields(suite, assemblySetups);
      Initialize(suiteType, suite, provider);

      return provider;
    }

    protected abstract void Initialize (Type suiteType, object suite, SuiteProvider provider);

    [CanBeNull]
    private SuiteProvider CreateSuiteProvider (Type suiteType, IIdentity assemblyIdentity)
    {
      var text = GetText(suiteType);
      if (text == null)
        return null;

      var identity = assemblyIdentity.CreateChildIdentity(suiteType.FullName);
      var ignoreReason = suiteType.GetAttribute<IgnoreAttribute>().GetValueOrDefault(x => x.Reason);
      var resources = suiteType.GetAttribute<ResourcesAttribute>().GetValueOrDefault(x => x.Resources);

      return SuiteProvider.Create(identity, text, ignoreReason, resources);
    }

    [CanBeNull]
    protected virtual string GetText (Type suiteType)
    {
      var subjectAttribute = GetDisplayAttribute(suiteType);
      var displayFormatAttribute = subjectAttribute?.Constructor.GetAttributeData<DisplayFormatAttribute>().NotNull();
      if (displayFormatAttribute == null)
        return null;

      return _introspectionPresenter.Present(displayFormatAttribute.ToCommon(), suiteType.ToCommon(), subjectAttribute.ToCommon());
    }

    [CanBeNull]
    protected virtual CustomAttributeData GetDisplayAttribute (Type suiteType)
    {
      return suiteType.GetAttributeData<SuiteAttributeBase>();
    }

    private void InitializeAssemblySetupFields (object suite, IDictionary<Type, Lazy<IAssemblySetup>> assemblySetups)
    {
      var suiteType = suite.GetType();
      var fields = suiteType.GetFieldsWithAttribute<AssemblySetupAttribute>(MemberBindings.Static).Select(x => x.Item1);

      foreach (var field in fields)
      {
        Lazy<IAssemblySetup> assemblySetup;
        if (!assemblySetups.TryGetValue(field.FieldType, out assemblySetup))
          throw new EvaluationException($"Type {field.FieldType} is not associated with an instance of IAssemblySetup.");
        field.SetValue(suite, assemblySetup.Value);
      }
    }

    protected void InvokeConstructor (object suite)
    {
      var suiteType = suite.GetType();
      var constructor = suiteType.GetConstructor(MemberBindings.Instance, binder: null, types: new Type[0], modifiers: new ParameterModifier[0]);
      if (constructor == null)
        throw new EvaluationException($"Suite '{suiteType.Name}' doesn't provide a default constructor.");

      try
      {
        constructor.Invoke(suite, new object[0]);
      }
      catch (TargetInvocationException exception)
      {
        throw new EvaluationException(
            $"Executing constructor for '{suiteType.Name}' has thrown an exception.",
            exception.InnerException);
      }
    }
  }
}