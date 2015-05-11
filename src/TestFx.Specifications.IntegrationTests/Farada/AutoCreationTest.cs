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
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.Farada;

namespace TestFx.Specifications.IntegrationTests.Farada
{
  public class AutoModelAttribute : AutoAttribute
  {
    [SuiteMemberDependency]
    public string IdMember { get; set; }

    public override void Mutate (object auto)
    {
      var id = GetNonNullValueFromSuiteMember<Guid> (IdMember);
      var model = (AutoCreationTest.DomainModel) auto;

      model.FirstName = id.ToString ();
    }
  }

  public class AutoCreationTest : TestBase<AutoCreationTest.DomainSpecK>
  {
    [Subject (typeof (AutoCreationTest), "Test")]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Auto] Guid Id;
      [AutoModel (IdMember = "Id")] DomainModel Model;

      public DomainSpecK ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .It ("injects non-empty Guid", x => x.Subject.Id.Should ().NotBe (Guid.Empty)));
      }

      public override DomainType CreateSubject ()
      {
        return new DomainType (Id, Model);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertDefaultTest (State.Passed)
          .WithOperations (
              "<Reset_Instance_Fields>",
              "<Create_Autos>",
              "<CreateSubject>",
              "<Action>",
              "returns FormatProvider");
    }

    public class DomainType
    {
      public DomainType (Guid id, DomainModel model)
      {
        Id = id;
        Model = model;
      }

      public Guid Id { get; private set; }
      public DomainModel Model { get; private set; }
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