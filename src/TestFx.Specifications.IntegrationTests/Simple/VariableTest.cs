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
using NUnit.Framework;

namespace TestFx.Specifications.IntegrationTests.Simple
{
  public class VariableTest : TestBase<VariableTest.DomainSpecK>
  {
    [Subject (typeof (VariableTest), "Test")]
    public class DomainSpecK : SpecK
    {
      int MyInteger;

      DomainSpecK ()
      {
        Specify (x => 1)
            .DefaultCase (_ => _
                .Given ("set MyInteger", x => MyInteger = 123)
                .GivenVars (x => new { MyString = "Foo", MyInteger })
                .It ("holds variables",
                    x =>
                    {
                      x.Vars.MyInteger.Should ().Be (123);
                      x.Vars.MyString.Should ().Be ("Foo");
                    }));
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTestPassed ("<Default>",
          "<Reset_Instance_Fields>",
          "set MyInteger",
          "<Set_Variables>",
          "<Action>",
          "holds variables");
    }
  }
}