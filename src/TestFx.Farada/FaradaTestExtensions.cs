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
using Farada.TestDataGeneration;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx.Farada
{
  public class FaradaTestExtensions : ITestExtension
  {
    private ITestDataGenerator _testDataGenerator;

    public int Priority
    {
      get { return 0; }
    }

    public void Extend (ITestController testController, ISuite suite)
    {
      InitTestDataGenerator(suite);
      FillAutos(testController, suite);
    }

    private void InitTestDataGenerator (ISuite suite)
    {
      var setupMethod = suite.GetType()
          .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
          .FirstOrDefault(m => m.GetCustomAttribute<TestDomainConfigurationAttribute>() != null);

      if (setupMethod == null)
      {
        _testDataGenerator = TestDataGeneratorFactory.Create();
      }
      else
      {
        var configuration = setupMethod.Invoke(null, new object[0]) as TestDataDomainConfiguration;
        if (configuration == null)
        {
          throw new ArgumentException(
              string.Format(
                  "The method marked with the {0} must return a valid non-null {1}",
                  typeof (TestDomainConfigurationAttribute).Name,
                  typeof (TestDataDomainConfiguration).Name));
        }

        _testDataGenerator = TestDataGeneratorFactory.Create(configuration);
      }
    }

    private void FillAutos (ITestController testController, ISuite suite)
    {
      var propertiesWithAttribute =
          AttributeUtility.Instance.GetMembersWithAttribute<PropertyInfo, AutoAttribute>(
              suite.GetType(),
              BindingFlags.Default | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
              | BindingFlags.NonPublic).ToList();
      if (propertiesWithAttribute.Count == 0)
        return;

      testController.AddAction<SetupExtension>("<Setup_Autos>", x => CreateAndAssignAutos(propertiesWithAttribute, suite));
    }

    private void CreateAndAssignAutos (IEnumerable<Tuple<PropertyInfo, AutoAttribute>> propertiesWithAttribute, ISuite suite)
    {
      foreach (var property in propertiesWithAttribute.Select(x => x.Item1))
      {
        var autoValue = this.InvokeGenericMethod("GetAutoValue", new object[] { property }, new[] { property.PropertyType });
        property.SetValue(suite, autoValue);
      }
    }

    [UsedImplicitly]
    private object GetAutoValue<T> (PropertyInfo property)
    {
      //We use a max recursion depth of 3 here, because the property itself counts already as a recursion depth of 1,
      //so not even the first nested property of the same type would be filled.
      return _testDataGenerator.Create<T>(maxRecursionDepth:3, propertyInfo: FastReflectionUtility.GetPropertyInfo(property));
    }
  }
}