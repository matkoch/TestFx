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
  public interface ITypeLoader<T> : ITypeLoader
  {
  }

  public abstract class TypeLoader<TSuiteType> : ITypeLoader<TSuiteType>
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;

    protected TypeLoader (IIntrospectionPresenter introspectionPresenter)
    {
      _introspectionPresenter = introspectionPresenter;
    }

    public ISuiteProvider Load (Type suiteType, ICollection<TypedLazy<IAssemblySetup>> assemblySetups, IIdentity assemblyIdentity)
    {
      var uninitializedSuite = (TSuiteType) FormatterServices.GetUninitializedObject(suiteType);

      var subjectAttribute = suiteType.GetAttributeData<SubjectAttributeBase>();
      var displayFormatAttribute = subjectAttribute.Constructor.GetAttributeData<DisplayFormatAttribute>();

      var text = _introspectionPresenter.Present(displayFormatAttribute.ToCommon(), subjectAttribute.ToCommon());
      var identity = assemblyIdentity.CreateChildIdentity(suiteType.FullName);
      var provider = SuiteProvider.Create(identity, text, ignored: false);

      InitializeAssemblySetupFields(uninitializedSuite, assemblySetups.ToList());
      InitializeTypeSpecificFields(uninitializedSuite, provider);

      InvokeConstructor(uninitializedSuite);

      return provider;
    }

    protected abstract void InitializeTypeSpecificFields (TSuiteType suite, SuiteProvider provider);

    private void InitializeAssemblySetupFields (TSuiteType suite, ICollection<TypedLazy<IAssemblySetup>> assemblySetups)
    {
      var suiteType = suite.GetType();
      var fields = suiteType.GetFieldsWithAttribute<AssemblySetupAttribute>(MemberBindings.Static).Select(x => x.Item1);

      foreach (var field in fields)
      {
        var assemblySetup = assemblySetups.FirstOrDefault(x => field.FieldType == x.Type);
        if (assemblySetup != null)
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