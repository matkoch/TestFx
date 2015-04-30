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
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests.FakeItEasy
{
  public class OrderedAssertionsTest : TestBase<OrderedAssertionsTest.DomainSpecK>
  {
    [Subject (typeof (OrderedAssertionsTest), "Test")]
    [OrderedAssertions]
    public class DomainSpecK : SpecK<DomainType>
    {
      [Faked] IDisposable FirstDisposable;
      [Faked] IDisposable SecondDisposable;

      public DomainSpecK ()
      {
        Specify (x => x.DoSomething ())
            .DefaultCase (_ => _
                .ItCallsInOrder ("first and second disposable", x =>
                {
                  A.CallTo (() => FirstDisposable.Dispose ()).MustHaveHappened ();
                  A.CallTo (() => SecondDisposable.Dispose ()).MustHaveHappened ();
                }));
      }

      public override DomainType CreateSubject ()
      {
        return new DomainType (FirstDisposable, SecondDisposable);
      }
    }

    [Test]
    public override void Test ()
    {
      AssertTestFailed ("<Default>",
          operationTexts: null,
          failedOperationTexts: new[] { "calls in order first and second disposable" });
      // TODO: test for FakeItEasy exception
    }

    public class DomainType
    {
      readonly IDisposable _firstDisposable;
      readonly IDisposable _secondDisposable;

      public DomainType (IDisposable FirstDisposable, IDisposable SecondDisposable)
      {
        _firstDisposable = FirstDisposable;
        _secondDisposable = SecondDisposable;
      }

      public void DoSomething ()
      {
        _secondDisposable.Dispose ();
        _firstDisposable.Dispose ();
      }
    }
  }
}