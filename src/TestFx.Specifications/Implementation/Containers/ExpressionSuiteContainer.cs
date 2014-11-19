﻿// Copyright 2014, 2013 Matthias Koch
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
using TestFx.Extensibility;
using TestFx.Extensibility.Containers;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Containers
{
  public class ExpressionSuiteContainer<TSubject, TResult> : Container, IIgnoreOrElaborate<TSubject, TResult>
  {
    private readonly IExpressionSuiteController<TSubject, TResult> _controller;

    public ExpressionSuiteContainer (IExpressionSuiteController<TSubject, TResult> controller)
        : base(controller)
    {
      _controller = controller;
    }

    public IElaborate<TSubject, TResult> Skip (string reason)
    {
      _controller.IgnoreNext();
      return this;
    }

    [DisplayFormat ("{0}")]
    public IIgnoreOrElaborate<TSubject, TResult> Elaborate (
        string description,
        Func<IDefineOrArrangeOrAssert<TSubject, TResult, object>, IAssert> succession)
    {
      var testController = _controller.CreateTestController(description);
      var testContainer = new TestContainer<TSubject, TResult, object>(testController);
      succession(testContainer);
      return this;
    }
  }
}