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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using TestFx.Evaluation.Loading;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;
using TestFx.Utilities.Introspection;
using TestFx.Utilities.Reflection;

namespace TestFx.Extensibility
{
  public interface ITypeLoader<T> : ITypeLoader
  {
  }

  public abstract class TypeLoader<TSuiteType, TSubjectAttribute> : ITypeLoader<TSuiteType>
      where TSubjectAttribute : SubjectAttributeBase
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;
    private readonly CommonAttribute _displayFormatAttribute;

    protected TypeLoader (IIntrospectionPresenter introspectionPresenter)
    {
      _introspectionPresenter = introspectionPresenter;
      _displayFormatAttribute = typeof (TSubjectAttribute).GetConstructors().Single().GetAttributeData<DisplayFormatAttribute>().ToCommon();
    }

    public ISuiteProvider Load (Type suiteType, IEnumerable<IAssemblySetup> assemblySetups, IIdentity assemblyIdentity)
    {
      var uninitializedSuite = (TSuiteType) FormatterServices.GetUninitializedObject(suiteType);

      var subjectAttribute = suiteType.GetAttributeData<TSubjectAttribute>().ToCommon();
      var text = _introspectionPresenter.Present(_displayFormatAttribute, subjectAttribute);
      var identity = assemblyIdentity.CreateChildIdentity(suiteType.FullName);
      var provider = SuiteProvider.Create(identity, text, ignored: false);

      InitializeAssemblyContextFields(uninitializedSuite, assemblySetups.ToList());
      InitializeTypeSpecificFields(uninitializedSuite, provider);

      InvokeConstructor(uninitializedSuite);

      return provider;
    }

    protected abstract void InitializeTypeSpecificFields (TSuiteType suite, SuiteProvider provider);

    private void InitializeAssemblyContextFields (TSuiteType suite, ICollection<IAssemblySetup> assemblySetups)
    {
      var suiteType = suite.GetType();
      var fields = suiteType.GetFields(MemberBindings.Instance);
      foreach (var field in fields)
      {
        var injectableAssemblyContext = assemblySetups.FirstOrDefault(x => field.FieldType.IsInstanceOfType(x));
        if (injectableAssemblyContext != null)
          field.SetValue(suite, injectableAssemblyContext);
      }
    }

    private void InvokeConstructor (TSuiteType suite)
    {
      var suiteType = suite.GetType();
      var constructor = suiteType.GetConstructor(MemberBindings.Instance, null, new Type[0], new ParameterModifier[0])
          .AssertNotNull("Suite types must have a default constructor.");
      try
      {
        constructor.Invoke(suite, new object[0]);
      }
      catch (TargetInvocationException exception)
      {
        throw exception.InnerException;
      }
    }
  }
}