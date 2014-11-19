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
using TestFx.Extensibility.Containers;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Containers
{
  public class TestContainer<TSubject, TResult, TVars> : Container, IDefineOrArrangeOrAssert<TSubject, TResult, TVars>
  {
    private readonly ITestController<TSubject, TResult, TVars> _controller;

    public TestContainer (ITestController<TSubject, TResult, TVars> controller)
        : base(controller)
    {
      _controller = controller;
    }

    public IArrangeOrAssert<TSubject, TResult, TNewVars> Define<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      var controller = _controller.SetVariables(variablesProvider);
      return new TestContainer<TSubject, TResult, TNewVars>(controller);
    }

    public IArrangeOrAssert<TSubject, TResult, TVars> GivenSubject (string description, Func<Dummy, TSubject> subjectFactory)
    {
      _controller.SetSubjectFactory<ArrangeSubject>("subject " + description, subjectFactory);
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars> Given (string description, Arrangement<TSubject, TResult, TVars> arrangement)
    {
      _controller.AddArrangement(description, arrangement);
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars> Given (Context context)
    {
      context(new TestContainer<Dummy, Dummy, Dummy>(_controller.CreateDelegate<Dummy, Dummy, Dummy>()));
      return this;
    }

    public IArrangeOrAssert<TSubject, TResult, TVars> Given (Context<TSubject> context)
    {
      context(new TestContainer<TSubject, Dummy, Dummy>(_controller.CreateDelegate<TSubject, Dummy, Dummy>()));
      return this;
    }

    public IAssert<TSubject, TResult, TVars> It (string description, Assertion<TSubject, TResult, TVars> assertion)
    {
      _controller.AddAssertion(description, assertion);
      return this;
    }

    public IAssert<TSubject, TResult, TVars> It (Behavior behavior)
    {
      behavior(new TestContainer<Dummy, Dummy, Dummy>(_controller.CreateDelegate<Dummy, Dummy, Dummy>()));
      return this;
    }

    public IAssert<TSubject, TResult, TVars> It (Behavior<TResult> behavior)
    {
      behavior(new TestContainer<Dummy, TResult, Dummy>(_controller.CreateDelegate<Dummy, TResult, Dummy>()));
      return this;
    }

    public IAssert<TSubject, TResult, TVars> It (Behavior<TSubject, TResult> behavior)
    {
      behavior(new TestContainer<TSubject, TResult, Dummy>(_controller.CreateDelegate<TSubject, TResult, Dummy>()));
      return this;
    }

    public IAssert ItThrows<T> (Func<TVars, string> messageProvider = null, Func<TVars, Exception> innerExceptionProvider = null) where T : Exception
    {
      return this.ItThrows<TSubject, TResult, TVars, T>(messageProvider, innerExceptionProvider);
    }

    public IAssert ItReturns<T> ()
    {
      return this.ItReturns<TSubject, TResult, TVars, T>();
    }
  }
}