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
using FakeItEasy;
using Machine.Specifications;
using TestFx.TestInfrastructure;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local

namespace TestFx.MSpec.Tests
{
  [Subject (typeof(int))]
  internal class outer_context : outer_context_base
  {
    internal class when_actioning : base_context
    {
      Establish ctx = () => ContextTestBase.Establish();

      Because of = () => ContextTestBase.Action();

      It asserts = () => ContextTestBase.Assertion();

      Cleanup stuff = () => ContextTestBase.Cleanup();
    }

    Establish ctx = () => ContextTestBase.OuterContextEstablish();

    Cleanup stuff = () => ContextTestBase.OuterContextCleanup();
  }

  internal class outer_context_base
  {
    Establish ctx = () => ContextTestBase.OuterContextBaseEstablish();

    Cleanup stuff = () => ContextTestBase.OuterContextBaseCleanup();
  }

  internal class base_context : base_context_base
  {
    Establish ctx = () => ContextTestBase.BaseContextEstablish();

    Cleanup stuff = () => ContextTestBase.BaseContextCleanup();
  }

  internal class base_context_base
  {
    Establish ctx = () => ContextTestBase.BaseContextBaseEstablish();

    Cleanup stuff = () => ContextTestBase.BaseContextBaseCleanup();
  }

  internal abstract class ContextTestBase : TestBase<outer_context.when_actioning>
  {
    public static Action OuterContextBaseEstablish;
    public static Action OuterContextEstablish;
    public static Action BaseContextBaseEstablish;
    public static Action BaseContextEstablish;
    public static Action Establish;

    public static Action Action;
    public static Action Assertion;

    public static Action Cleanup;
    public static Action BaseContextCleanup;
    public static Action BaseContextBaseCleanup;
    public static Action OuterContextCleanup;
    public static Action OuterContextBaseCleanup;

    public static Action ThrowingAction = () => { throw new Exception(); };

    public override void SetUp ()
    {
      base.SetUp();

      OuterContextBaseEstablish = A.Fake<Action>();
      OuterContextEstablish = A.Fake<Action>();
      BaseContextBaseEstablish = A.Fake<Action>();
      BaseContextEstablish = A.Fake<Action>();
      Establish = A.Fake<Action>();

      Action = A.Fake<Action>();
      Assertion = A.Fake<Action>();

      Cleanup = A.Fake<Action>();
      BaseContextCleanup = A.Fake<Action>();
      BaseContextBaseCleanup = A.Fake<Action>();
      OuterContextCleanup = A.Fake<Action>();
      OuterContextBaseCleanup = A.Fake<Action>();
    }
  }
}