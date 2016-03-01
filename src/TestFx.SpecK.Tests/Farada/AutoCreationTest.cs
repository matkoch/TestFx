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
using FakeItEasy.Core;
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.Evaluation.Results;
using TestFx.Farada;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Farada
{
  internal class AutoCreationTest : TestBase<AutoCreationTest.DomainSpec>
  {
    [Subject (typeof (AutoCreationTest))]
    [AutoDataSeed (1337)]
    internal class DomainSpec : Spec
    {
      [AutoData] DomainModel Model;
      [AutoData] [Range (5, 7)] int Integer;

      public DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("Fills properties", x =>
                {
                  Model.Age.Should ().BeInRange (30, 33);
                  Model.FirstName.Length.Should ().BeInRange (3, 10);
                })
                .It ("Fills fields", x => Integer.Should ().BeInRange (5, 7)));
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
              "Fills properties",
              "Fills fields");
    }

    [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
    internal class DomainModel
    {
      [Required]
      [MinLength (3)]
      [MaxLength (10)]
      public string FirstName { get; set; }

      [Range (30, 33)]
      public int Age { get; set; }
    }
  }
}