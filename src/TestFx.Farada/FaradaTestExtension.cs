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
using Farada.TestDataGeneration.Fluent;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities.Reflection;

namespace TestFx.Farada
{
  public class FaradaTestExtension : ITestExtension
  {
    public int Priority
    {
      get { return 0; }
    }

    public void Extend (ITestController testController, ISuite suite)
    {
      CreateAutos(testController, suite);
    }

    private void CreateAutos (ITestController testController, ISuite suite)
    {
      var fieldsWithAttribute = suite.GetType().GetFieldsWithAttribute<AutoAttribute>().ToList();
      if (fieldsWithAttribute.Count == 0)
        return;

      var random = new Random();
      var configuration = GetTestDataConfiguration(suite.GetType());
      var generator = TestDataGeneratorFactory.Create(x => configuration(x).UseRandom(random));

      // TODO: add seed to data
      testController.AddAction<SetupExtension>(
          "<Create_Autos>",
          x => fieldsWithAttribute.ForEach(t => CreateAndAssignAuto(suite, generator, t.Item2, t.Item1)));
    }

    private Func<ITestDataConfigurator, ITestDataConfigurator> GetTestDataConfiguration (Type suiteType)
    {
      var attribute = suiteType.GetAttribute<TestDataConfigurationAttribute>();
      if (attribute == null)
        return x => x;

      return attribute.ConfigurationType.CreateInstance<ITestDataConfigurationProvider>().Configuration;
    }

    private void CreateAndAssignAuto (ISuite suite, ITestDataGenerator generator, AutoAttribute attribute, FieldInfo field)
    {
      var auto = this.InvokeGenericMethod("CreateAuto", new object[] { generator }, new[] { field.FieldType });
      attribute.Mutate(auto, suite);
      field.SetValue(suite, auto);
    }

    [UsedImplicitly]
    private object CreateAuto<T> (ITestDataGenerator generator)
    {
      return generator.Create<T>();
    }
  }
}