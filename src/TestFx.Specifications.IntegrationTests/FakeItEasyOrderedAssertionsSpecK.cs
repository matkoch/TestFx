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
using TestFx.FakeItEasy;

namespace TestFx.Specifications.IntegrationTests
{
  [Subject (typeof (FakeItEasyOrderedAssertionsSpecK), "Method")]
  public class FakeItEasyOrderedAssertionsSpecK : FakeItEasySpecK
  {
    public FakeItEasyOrderedAssertionsSpecK ()
    {
      Specify (x => x.DoSomething2 ())
          .Elaborate ("Case 1", _ => _
              .ItMakesOrderedCallsTo (x => new object[] { Disposable, ServiceProvider }))
          .Elaborate ("Case 2", _ => _
              .ItMakesOrderedCallsWithAnyArguments (
                  x => Disposable.Dispose (),
                  x => ServiceProvider.GetService (null))
              .It ("Returns service from provider", x => x.Result.Should ().BeSameAs (Service)))
          .Elaborate ("Case 3", _ => _
              .ItMakesOrderedCallsWithAnyArguments (
                  x => ServiceProvider.GetService (null),
                  x => Disposable.Dispose ())
              .It ("Returns service from provider", x => x.Result.Should ().BeSameAs (Service)));
    }
  }

#if !EXAMPLE
  public class FakeItEasyOrderedAssertionsTest : TestBase<FakeItEasyOrderedAssertionsSpecK>
  {
    [Test]
    public void Test ()
    {
      RunResult.State.Should ().Be (State.Failed);

      AssertResult (OperationResults[4], "<OPERATION>", "CallsInOrder [ Disposable , ServiceProvider ]", State.Passed);
      AssertResult (OperationResults[9], "<OPERATION>", "CallsInOrder [ Disposable.Dispose() , ServiceProvider.GetService() ]", State.Passed);
      AssertResult (OperationResults[10], "<OPERATION>", "Returns service from provider", State.Passed);
    }
  }
#endif
}