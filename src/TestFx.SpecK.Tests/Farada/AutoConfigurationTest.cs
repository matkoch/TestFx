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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FakeItEasy.Core;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.Fluent;
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.Evaluation.Results;
using TestFx.Farada;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Farada
{
  internal class AutoConfigurationTest : TestBase<AutoConfigurationTest.DomainSpec>
  {
    [Subject (typeof (AutoCreationTest))]
    [AutoDataSeed (1337)]
    [AutoDataConfiguration (typeof (StringConfiguration))]
    internal class DomainSpec : Spec
    {
      [AutoData] string String;

      public DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("Fills properties", x => String.Should().BeOneOf("1", "2", "3")));
      }
    }

    public class StringConfiguration : ITestDataConfigurationProvider
    {
      public Func<ITestDataConfigurator, ITestDataConfigurator> Configuration
      {
        get
        {
          return x =>
          {
            var values = new ChooseSingleItemValueProvider<int, string> (new[] { 1, 2, 3 }, i => i.ToString());
            x.For<string> ().AddProvider (values);
            return x;
          };
        }
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasPassed ()
          .HasOperations (
              Constants.Reset_Instance_Fields,
              Constants.Create_AutoData + "<1337>",
              Constants.Action,
              "Fills properties");
    }
  }
}