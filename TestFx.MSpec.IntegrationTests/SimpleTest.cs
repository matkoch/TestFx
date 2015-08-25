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
using FluentAssertions;
using TestFx.Evaluation.Results;

namespace TestFx.MSpec.IntegrationTests
{
  public class SimpleTest : TestBase<SimpleTest.when_adding>
  {
    [Subject(typeof(int))]
    public class when_adding
    {
      static int A;
      static int B;
      static int Result;

      Establish ctx = () =>
      {
        A = 1;
        B = 2;
      };

      Because of = () => Result = A + B;
      It returns_three = () => Result.Should ().Be (3);
      It returns_three_again = () => Result.Should ().Be (3);
    }

    public override void Test ()
    {
      AssertTest ("Int32, when adding", State.Passed)
          .WithOperations (
              "<Establish>",
              "<Because of>",
              "It returns three",
              "It returns three again");
    }
  }
}