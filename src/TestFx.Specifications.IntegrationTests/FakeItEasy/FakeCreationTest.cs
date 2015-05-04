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
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TestFx.Evaluation.Results;
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests.FakeItEasy
{
  public class FakeCreationTest : TestBase<FakeCreationTest.DomainSpecK>
  {
    [Subject (typeof (FakeCreationTest), "Test")]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Faked] IFormatProvider FormatProvider;

      public DomainSpecK ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _
                .Given ("FormatProvider returns", x => A.CallTo (() => FormatProvider.GetFormat (A<Type>._)).Returns (1))
                .It ("returns FormatProvider", x => x.Subject.FormatProvider.GetFormat (null).Should ().Be (1)));
      }

      public override DomainType CreateSubject ()
      {
        return new DomainType (FormatProvider);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertDefaultTest (State.Passed)
          .WithOperations (
              "<Reset_Instance_Fields>",
              "<Create_Fakes>",
              "<CreateSubject>",
              "FormatProvider returns",
              "<Action>",
              "returns FormatProvider");
    }

    public class DomainType
    {
      public DomainType (IFormatProvider formatProvider)
      {
        FormatProvider = formatProvider;
      }

      public IFormatProvider FormatProvider { get; private set; }
    }
  }
}