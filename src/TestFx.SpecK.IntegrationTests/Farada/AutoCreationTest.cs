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
using System.ComponentModel.DataAnnotations;
using FakeItEasy.Core;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Farada;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.IntegrationTests.Farada
{
  public class AutoCreationTest : TestBase<AutoCreationTest.DomainSpec>
  {
    [Subject (typeof (AutoCreationTest), "Test")]
    [AutoDataSeed (1337)]
    public class DomainSpec : Spec
    {
      [AutoData] DomainModel Model;

      public DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("Fills properties", x =>
                {
                  Model.Age.Should ().BeInRange (30, 33);
                  Model.FirstName.Length.Should ().BeInRange (3, 10);
                }));
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

    public class DomainModel
    {
      [Required]
      [MinLength (3)]
      [MaxLength (10)]
      public string FirstName { get; set; }

      [System.ComponentModel.DataAnnotations.Range (30, 33)]
      public int Age { get; set; }
    }
  }
}