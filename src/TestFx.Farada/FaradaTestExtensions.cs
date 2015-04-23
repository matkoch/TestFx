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
    public const string Key = "Farada";

    public int Priority
    {
      get { return 0; }
    }

    public void Extend (ITestController testController, ISuite suite)
    {
      _testDataGenerator = TestDataGeneratorFactory.Create();

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

      testController.AddAction<SetupExtension>("<Create_Fakes>", x => propertiesWithAttribute.ForEach(t => CreateAndAssignFake(suite, t.Item1)));
    }

    private void CreateAndAssignFake (ISuite suite, PropertyInfo property)
    {
      var fake = this.InvokeGenericMethod("FillAuto", new object[]{property}, new[] { property.PropertyType });
      property.SetValue(suite, fake);
    }

    [UsedImplicitly]
    private object FillAuto<T> (PropertyInfo property)
    {
      return _testDataGenerator.Create<T>(propertyInfo: FastReflectionUtility.GetPropertyInfo(property));
    }
  }
}