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
using TestFx.Extensibility.Contexts;
using TestFx.Extensibility.Controllers;
using TestFx.Specifications.Implementation;
using TestFx.Utilities.Reflection;

namespace TestFx.Farada
{
  public class FaradaTestExtensions : ITestExtension
  {
    public const string Key = "Farada";
    public const string ConfigurationKey = "TestDataDomainConfiguration";

    public int Priority
    {
      get { return 0; }
    }

    public void Extend (ITestController testController, ISuite suite)
    {
      FillAutos(testController, suite);
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

      testController.AddAssertion<Assert>("<Setup_Autos>", x => CreateAndAssignAutos(x, propertiesWithAttribute, suite));
    }

    private void CreateAndAssignAutos (ITestContext context, IEnumerable<Tuple<PropertyInfo, AutoAttribute>> propertiesWithAttribute, ISuite suite)
    {
      var testDataGeneratorContext = (TestDataDomainConfiguration) context[ConfigurationKey];
      var testDataGenerator = TestDataGeneratorFactory.Create(testDataGeneratorContext);

      foreach (var property in propertiesWithAttribute.Select(x => x.Item1))
      {
        var autoValue = this.InvokeGenericMethod("GetAutoValue", new object[] { testDataGenerator, property }, new[] { property.PropertyType });
        property.SetValue(suite, autoValue);
      }
    }

    [UsedImplicitly]
    private object GetAutoValue<T> (ITestDataGenerator testDataGenerator, PropertyInfo property)
    {
      return testDataGenerator.Create<T>(propertyInfo: FastReflectionUtility.GetPropertyInfo(property));
    }
  }
}