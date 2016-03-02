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
using System.Linq;
using System.Reflection;
using Farada.TestDataGeneration;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;
using TestFx.Extensibility;
using TestFx.Extensibility.Controllers;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Farada
{
  public class FaradaTestExtension : ITestExtension
  {
    private readonly FastReflectionUtility _reflectionUtility;
    private readonly Random _seedGenerator;

    public FaradaTestExtension ()
    {
      _reflectionUtility = new FastReflectionUtility(new DefaultMemberExtensionService());
      _seedGenerator = new Random();
    }

    public int Priority => 0;

    public void Extend (ITestController testController, object suite)
    {
      var suiteType = suite.GetType();
      var fieldsWithAttribute = suiteType.GetFieldsWithAttribute<AutoDataAttribute>()
          .OrderBy(x => x.Item1.Name).ToList();

      if (fieldsWithAttribute.Count == 0)
        return;

      var seed = GetSeed(suiteType);
      var random = new DefaultRandom(seed);
      var configuration = GetAutoDataConfiguration(suiteType);

      var generator = TestDataGeneratorFactory.Create(x => configuration(x).UseRandom(random));

      // TODO: add seed to data
      testController.AddAction<SetupExtension>(
          $"<Create_AutoData><{seed}>",
          x => fieldsWithAttribute.ForEach(t => CreateAndAssignAuto(suite, generator, t.Item2, t.Item1)));
    }

    private int GetSeed (Type suiteType)
    {
      var attribute = suiteType.GetAttribute<AutoDataSeedAttribute>();
      return attribute?.Seed ?? _seedGenerator.Next();
    }

    private Func<ITestDataConfigurator, ITestDataConfigurator> GetAutoDataConfiguration (Type suiteType)
    {
      var attribute = suiteType.GetAttribute<AutoDataConfigurationAttribute>();
      return attribute != null
          ? attribute.ConfigurationType.CreateInstance<ITestDataConfigurationProvider>().Configuration
          : (x => x);
    }

    private void CreateAndAssignAuto (object suite, ITestDataGenerator generator, AutoDataAttribute attribute, FieldInfo field)
    {
      var autoData = this.InvokeGenericMethod(
          "CreateAutoData",
          new object[] { generator, field, attribute.MaxRecursionDepth },
          new[] { field.FieldType }).NotNull();

      attribute.CurrentSuite = suite;
      attribute.Mutate(autoData);

      field.SetValue(suite, autoData);
    }

    [UsedImplicitly]
    private object CreateAutoData<T> (ITestDataGenerator generator, FieldInfo field, int maxRecursionDepth)
    {
      return generator.Create<T>(maxRecursionDepth, _reflectionUtility.GetFieldInfo(field));
    }
  }
}