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

namespace TestFx.MSpec.IntegrationTests.BaseContext
{
  public abstract class BaseContextTestBase : TestBase<BaseContextTestBase.when_calling>
  {
    protected static Action BaseSetup = A.Fake<Action> ();
    protected static Action Setup = A.Fake<Action> ();
    protected static Action Action = A.Fake<Action> ();
    protected static Action Assertion = A.Fake<Action> ();
    protected static Action Cleanup = A.Fake<Action> ();
    protected static Action BaseCleanup = A.Fake<Action> ();

    protected static Action ThrowingAction = () => { throw new Exception (); };

    [Subject (typeof (int))]
    public class when_calling : given_context
    {
      Establish ctx = () => Setup ();

      Because of = () => Action ();

      It asserts = () => Assertion ();

      Cleanup stuff = () => Cleanup ();
    }

    public class given_context
    {
      Establish ctx = () => BaseSetup ();

      Cleanup stuff = () => BaseCleanup ();
    }
  }
}