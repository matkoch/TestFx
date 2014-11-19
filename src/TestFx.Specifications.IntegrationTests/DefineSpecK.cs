// Copyright 2014, 2013 Matthias Koch
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
using TestFx.Evaluation.Results;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (DefineSpecK), "Method")]
  public class DefineSpecK : SpecK<DefineSpecK.DomainType>
  {
    DefineSpecK ()
    {
      Specify (x => 1)
          .Elaborate ("Case", _ => _
              .Define (x => new { StringBase = "Moep", IntegerBase = 2 })
              .Given ("set string property", x => x.Subject.String = x.Vars.StringBase)
              .Given ("set integer property", x => x.Subject.Integer = x.Vars.IntegerBase)
              .It ("appends '1' to set string", x => x.Subject.String.Should ().Be ("Moep1"))
              .It ("adds 1 to set integer", x => x.Subject.Integer.Should ().Be (3)));
    }

    public class DomainType
    {
      string _string;
      int _integer;

      public string String { get { return _string; } set { _string = value + "1"; } }
      public int Integer { get { return _integer; } set { _integer = value + 1; } }
    }
  }

#if !EXAMPLE
  public class DefineTest : TestBase<DefineSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Passed);
    }
  }
#endif
}