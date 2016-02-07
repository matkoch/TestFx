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
using System.Collections.Generic;
using TestFx.Extensibility.Containers;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK.Implementation.Containers
{
  public static class TestContainer
  {
    public static TestContainer<TSubject, TResult, TVars, TSequence> Create<TSubject, TResult, TVars, TSequence> (
        ITestController<TSubject, TResult, TVars, TSequence> testController)
    {
      return new TestContainer<TSubject, TResult, TVars, TSequence>(testController);
    }
  }

  public class TestContainer<TSubject, TResult, TVars, TSequence> : Container, ICombineOrArrangeOrAssert<TSubject, TResult, TVars, TSequence>
  {
    private readonly ITestController<TSubject, TResult, TVars, TSequence> _controller;

    public TestContainer (ITestController<TSubject, TResult, TVars, TSequence> controller)
        : base(controller)
    {
      _controller = controller;
    }

    public IArrangeOrAssert<TSubject, TResult, Dummy, TNewSequence> WithSequences<TNewSequence> (IDictionary<string, TNewSequence> sequences)
    {
      var controller = _controller.SetSequences(sequences);
      return TestContainer.Create(controller);
    }

    public IArrangeOrAssert<TSubject, TResult, TNewVars, TSequence> GivenVars<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      var controller = _controller.SetVariables(variablesProvider);
      return TestContainer.Create(controller);
    }

    public IArrangeOrAssert<TSubject, TResult, TVars, TSequence> GivenSubject (string description, Func<Dummy, TSubject> subjectFactory)
    {
      _controller.SetSubjectFactory<ArrangeSubject>("subject " + description, subjectFactory);
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (
        string description,
        Arrangement<TSubject, TResult, TVars, TSequence> arrangement)
    {
      _controller.AddArrangement(description, arrangement);
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (Context context)
    {
      var controller = _controller.CreateDelegate<Dummy, Dummy, Dummy, Dummy>();
      context(TestContainer.Create(controller));
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars, TSequence> Given (Context<TSubject> context)
    {
      var controller = _controller.CreateDelegate<TSubject, Dummy, Dummy, Dummy>();
      context(TestContainer.Create(controller));
      return this;
    }

    public IAssert<TSubject, TResult, TVars, TSequence> It (string description, Assertion<TSubject, TResult, TVars, TSequence> assertion)
    {
      _controller.AddAssertion(description, assertion);
      return this;
    }

    public IAssert<TSubject, TResult, TVars, TSequence> It (Behavior behavior)
    {
      var controller = _controller.CreateDelegate<Dummy, Dummy, Dummy, Dummy>();
      behavior(TestContainer.Create(controller));
      return this;
    }

    public IAssert<TSubject, TResult, TVars, TSequence> It (Behavior<TResult> behavior)
    {
      var controller = _controller.CreateDelegate<Dummy, TResult, Dummy, Dummy>();
      behavior(TestContainer.Create(controller));
      return this;
    }

    public IAssert<TSubject, TResult, TVars, TSequence> It (Behavior<TSubject, TResult> behavior)
    {
      var controller = _controller.CreateDelegate<TSubject, TResult, Dummy, Dummy>();
      behavior(TestContainer.Create(controller));
      return this;
    }
  }
}