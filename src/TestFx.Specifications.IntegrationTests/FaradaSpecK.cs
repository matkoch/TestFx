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
using Farada.TestDataGeneration;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Farada;

namespace TestFx.Specifications.IntegrationTests
{

  public class FaradaSpecK
  {
    [Subject (typeof (DefaultSpecK), "Method")]
    public class DefaultSpecK : SpecK
    {
      [Auto]
      public string SomeString { get; set; }

      [System.ComponentModel.DataAnnotations.Range (1, 1)]
      [Auto]
      public int RangedInt { get; set; }

      [Auto]
      public CompoundClass Compound { get; set; }

      public DefaultSpecK ()
      {
        Specify (_ => _)
            .DefaultCase (_ => _
                .It ("fills a standard string", x => SomeString.Should ().NotBeNullOrEmpty ())
                .It ("fills a ranged integer", x => RangedInt.Should ().Be (1))
                .It ("fills a compound class", x =>
                    Compound.SubClass.SomeValue.Should ().Be (4)));
      }
    }

    [Subject (typeof (ConfigurationSpecK), "Method")]
    public class ConfigurationSpecK : SpecK
    {
      [Auto]
      public CompoundClass Compound { get; set; }

      [TestDomainConfiguration]
      static TestDataDomainConfiguration GetConfiguration ()
      {
        return cfg => cfg.UseDefaults(false).For ((CompoundClass c) => c.SomeValue).AddProvider (x => 10);
      }

      public ConfigurationSpecK ()
      {
        Specify (_ => _)
            .DefaultCase (_ => _
                .It ("fills a compound class according to configuration", x =>
                    Compound.SomeValue.Should ().Be (10)));
      }
    }

    public class CompoundClass
    {
      public CompoundClass SubClass { get; set; }

      [System.ComponentModel.DataAnnotations.Range (4, 4)]
      public int SomeValue { get; set; }
    }
  }

#if !EXAMPLE

  namespace TestFx.Specifications.IntegrationTests
  {
    public class FaradaTest : TestBase<FaradaSpecK.DefaultSpecK>
    {
      [Test]
      public void Test ()
      {
        RunResult.State.Should ().Be (State.Passed);
        AssertResult (OperationResults[1], "<Setup_Autos>", State.Passed);
      }
    }
  }
}

#endif