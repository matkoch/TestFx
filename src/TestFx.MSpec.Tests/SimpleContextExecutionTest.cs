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
using System.Linq;
using FakeItEasy;
using FakeItEasy.Core;
using Machine.Specifications;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace TestFx.MSpec.Tests
{
  internal class SimpleContext
  {
    public static Action OuterContextBaseEstablish = A.Fake<Action>();
    public static Action OuterContextEstablish = A.Fake<Action>();
    public static Action BaseContextBaseEstablish = A.Fake<Action>();
    public static Action BaseContextEstablish = A.Fake<Action>();
    public static Action Establish = A.Fake<Action>();
    public static Action Action = A.Fake<Action>();
    public static Action Assertion = A.Fake<Action>();
    public static Action Cleanup = A.Fake<Action>();
    public static Action BaseContextCleanup = A.Fake<Action>();
    public static Action BaseContextBaseCleanup = A.Fake<Action>();
    public static Action OuterContextCleanup = A.Fake<Action>();
    public static Action OuterContextBaseCleanup = A.Fake<Action>();

    [Subject (typeof(int))]
    internal class outer_context : outer_context_base
    {
      internal class when_actioning : base_context
      {
        Establish ctx = () => Establish();

        Because of = () => Action();

        It asserts = () => Assertion();

        Cleanup stuff = () => Cleanup();
      }

      Establish ctx = () => OuterContextEstablish();

      Cleanup stuff = () => OuterContextCleanup();
    }

    internal class outer_context_base
    {
      Establish ctx = () => OuterContextBaseEstablish();

      Cleanup stuff = () => OuterContextBaseCleanup();
    }

    internal class base_context : base_context_base
    {
      Establish ctx = () => BaseContextEstablish();

      Cleanup stuff = () => BaseContextCleanup();
    }

    internal class base_context_base
    {
      Establish ctx = () => BaseContextBaseEstablish();

      Cleanup stuff = () => BaseContextBaseCleanup();
    }
  }

  internal class SimpleContextExecutionTest : TestBase<SimpleContext.outer_context.when_actioning>
  {
    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      using (scope.OrderedAssertions())
      {
        A.CallTo(() => SimpleContext.OuterContextBaseEstablish()).MustHaveHappened();
        A.CallTo(() => SimpleContext.OuterContextEstablish()).MustHaveHappened();
        A.CallTo(() => SimpleContext.BaseContextBaseEstablish()).MustHaveHappened();
        A.CallTo(() => SimpleContext.BaseContextEstablish()).MustHaveHappened();
        A.CallTo(() => SimpleContext.Establish()).MustHaveHappened();
        A.CallTo(() => SimpleContext.Action()).MustHaveHappened();
        A.CallTo(() => SimpleContext.Assertion()).MustHaveHappened();
        A.CallTo(() => SimpleContext.Cleanup()).MustHaveHappened();
        A.CallTo(() => SimpleContext.BaseContextCleanup()).MustHaveHappened();
        A.CallTo(() => SimpleContext.BaseContextBaseCleanup()).MustHaveHappened();
        A.CallTo(() => SimpleContext.OuterContextCleanup()).MustHaveHappened();
        A.CallTo(() => SimpleContext.OuterContextBaseCleanup()).MustHaveHappened();
      }
    }
  }
}