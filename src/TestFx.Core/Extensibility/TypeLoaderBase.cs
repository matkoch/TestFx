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
using System.Runtime.Serialization;
using TestFx.Evaluation;
using TestFx.Evaluation.Loading;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Extensibility
{
  public abstract class TypeLoaderBase : ITypeLoader
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;

    protected TypeLoaderBase (IIntrospectionPresenter introspectionPresenter)
    {
      _introspectionPresenter = introspectionPresenter;
    }

    public ISuiteProvider Load (object suite, IDictionary<Type, Lazy<IAssemblySetup>> assemblySetups, IIdentity assemblyIdentity)
    {
      Trace.Assert(suite.GetType() != typeof (Type));
      var suiteType = suite.GetType();

      var subjectAttribute = suiteType.GetAttributeData<SuiteAttributeBase>().NotNull();
      var displayFormatAttribute = subjectAttribute.Constructor.GetAttributeData<DisplayFormatAttribute>().NotNull();

      var text = _introspectionPresenter.Present(displayFormatAttribute.ToCommon(), suiteType.ToCommon(), subjectAttribute.ToCommon());
      var identity = assemblyIdentity.CreateChildIdentity(suiteType.FullName);
      var resources = suiteType.GetAttribute<ResourcesAttribute>().GetValueOrDefault(x => x.Resources, () => new string[0]);
      var provider = SuiteProvider.Create(identity, text, ignored: false, resources: resources);

      InitializeAssemblySetupFields(suite, assemblySetups);
      InitializeTypeSpecificFields(suite, provider);

      InvokeConstructor(suite);

      return provider;
    }

    protected abstract void InitializeTypeSpecificFields (object suite, SuiteProvider provider);

    private void InitializeAssemblySetupFields (object suite, IDictionary<Type, Lazy<IAssemblySetup>> assemblySetups)
    {
      var suiteType = suite.GetType();
      var fields = suiteType.GetFieldsWithAttribute<AssemblySetupAttribute>(MemberBindings.Static).Select(x => x.Item1);

      foreach (var field in fields)
      {
        Lazy<IAssemblySetup> assemblySetup;
        if (!assemblySetups.TryGetValue(field.FieldType, out assemblySetup))
          throw new EvaluationException(string.Format("Type {0} is not associated with an instance of IAssemblySetup.", field.FieldType));
        field.SetValue(suite, assemblySetup.Value);
      }
    }

    private void InvokeConstructor (object suite)
    {
      var suiteType = suite.GetType();
      var constructor = suiteType.GetConstructor(MemberBindings.Instance, null, new Type[0], new ParameterModifier[0]);
      if (constructor == null)
        throw new EvaluationException(string.Format("Suite '{0}' doesn't provide a default constructor.", suiteType.Name));

      try
      {
        constructor.Invoke(suite, new object[0]);
      }
      catch (TargetInvocationException exception)
      {
        throw new EvaluationException(
            string.Format("Executing constructor for '{0}' has thrown an exception.", suiteType.Name),
            exception.InnerException);
      }
    }
  }
}