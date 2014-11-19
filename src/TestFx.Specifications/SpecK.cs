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
using System.Linq.Expressions;
using TestFx.Extensibility;
using TestFx.Specifications.Implementation.Containers;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications
{
  public abstract class SpecK<TSubject> : ISpecK<TSubject>
  {
    private const string c_specifyDisplayFormat = "{0}";

    // ReSharper disable UnassignedField.Compiler
    private IClassSuiteController<TSubject> _classSuiteController;
    private Func<ISpecK<TSubject>, TSubject> _subjectFactory;
    // ReSharper restore UnassignedField.Compiler

    public void SetupOnce (Action setup, Action cleanup = null)
    {
      _classSuiteController.AddSetupCleanup<SetupCommon, CleanupCommon>(setup, cleanup);
    }

    public void Setup (Action<ITestContext<TSubject>> setup, Action<ITestContext<TSubject>> cleanup = null)
    {
      _classSuiteController.AddTestSetupCleanup(setup, cleanup);
    }

    [DisplayFormat(c_specifyDisplayFormat)]
    public IIgnoreOrElaborate<TSubject, Dummy> Specify (Expression<Action<TSubject>> action)
    {
      var expressionSuiteController = _classSuiteController.CreateExpressionSuiteController<Dummy>(c_specifyDisplayFormat, action);
      return new ExpressionSuiteContainer<TSubject, Dummy>(expressionSuiteController);
    }

    [DisplayFormat(c_specifyDisplayFormat)]
    public IIgnoreOrElaborate<TSubject, TResult> Specify<TResult> (Expression<Func<TSubject, TResult>> action)
    {
      var expressionSuiteController = _classSuiteController.CreateExpressionSuiteController(c_specifyDisplayFormat, action);
      return new ExpressionSuiteContainer<TSubject, TResult>(expressionSuiteController);
    }

    public virtual TSubject CreateSubject ()
    {
      return _subjectFactory(this);
    }
  }

  public abstract class SpecK : SpecK<Dummy>
  {
    public sealed override Dummy CreateSubject ()
    {
      return null;
    }
  }
}