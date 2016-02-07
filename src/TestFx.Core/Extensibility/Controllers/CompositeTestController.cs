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
using System.Linq;
using JetBrains.Annotations;
using TestFx.Extensibility.Contexts;
using TestFx.Utilities.Collections;

namespace TestFx.Extensibility.Controllers
{
  public class CompositeTestController : ITestController
  {
    private readonly ICollection<ITestController> _controllers;

    protected CompositeTestController (IEnumerable<ITestController> controllers)
    {
      _controllers = controllers.ToList();
    }

    public void AddSetupCleanup<TSetup, TCleanup> (
        string setupText,
        Action<ITestContext> setup,
        [CanBeNull] string cleanupText,
        [CanBeNull] Action<ITestContext> cleanup)
        where TSetup : IActionDescriptor
        where TCleanup : ICleanupDescriptor
    {
      _controllers.ForEach(x => x.AddSetupCleanup<TSetup, TCleanup>(setupText, setup, cleanupText, cleanup));
    }

    public void AddAction<T> (string text, Action<ITestContext> action)
        where T : IActionDescriptor
    {
      _controllers.ForEach(x => x.AddAction<T>(text, action));
    }

    public void AddAssertion<T> (string text, Action<ITestContext> action)
        where T : IAssertionDescriptor
    {
      _controllers.ForEach(x => x.AddAssertion<T>(text, action));
    }

    public void AddNotImplementedAction<T> (string text) where T : IOperationDescriptor
    {
      _controllers.ForEach(x => x.AddNotImplementedAction<T>(text));
    }

    public void AddNotImplementedAssertion<T> (string text) where T : IOperationDescriptor
    {
      _controllers.ForEach(x => x.AddNotImplementedAssertion<T>(text));
    }

    public void Replace<T> (Action<ITestContext, Action> replacingAction)
        where T : IOperationDescriptor
    {
      _controllers.ForEach(x => x.Replace<T>(replacingAction));
    }

    public void RemoveAll<T> ()
        where T : IOperationDescriptor
    {
      _controllers.ForEach(x => x.RemoveAll<T>());
    }
  }
}