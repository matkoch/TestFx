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
using NUnit.Framework;

namespace TestFx.MSpec.IntegrationTests.BaseContext
{
  [TestFixture]
  public abstract class ThrowingBaseSetupTestBase : BaseContextTestBase
  {
    public override void SetUp ()
    {
      BaseSetup = ThrowingAction;

      base.SetUp ();
    }

    [Test]
    public override void Test ()
    {
      A.CallTo (() => Setup ()).MustNotHaveHappened ();
      A.CallTo (() => Action ()).MustNotHaveHappened ();
      A.CallTo (() => Assertion ()).MustNotHaveHappened ();
      A.CallTo (() => Cleanup ()).MustNotHaveHappened ();
      A.CallTo (() => BaseCleanup ()).MustNotHaveHappened ();
    }
  }
}