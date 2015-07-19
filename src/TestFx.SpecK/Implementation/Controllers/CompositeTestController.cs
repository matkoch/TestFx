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
using System.Collections.Generic;
using System.Linq;
using TestFx.Extensibility.Controllers;
using TestFx.SpecK.InferredApi;
using TestFx.Utilities.Collections;

namespace TestFx.SpecK.Implementation.Controllers
{
  public class CompositeTestController<TSubject, TResult, TVars, TCombi> : CompositeTestController, ITestController<TSubject, TResult, TVars, TCombi>
  {
    private readonly IControllerFactory _controllerFactory;
    private readonly ICollection<ITestController<TSubject, TResult, TVars, TCombi>> _controllers;

    public CompositeTestController (ICollection<ITestController<TSubject, TResult, TVars, TCombi>> controllers, IControllerFactory controllerFactory)
        : base(controllers.Cast<ITestController>().ToList())
    {
      _controllerFactory = controllerFactory;
      _controllers = controllers;
    }

    public void SetSubjectFactory<T> (string text, Func<Dummy, TSubject> subjectFactory) where T : SubjectFactory
    {
      _controllers.ForEach(x => x.SetSubjectFactory<T>(text, subjectFactory));
    }

    public ITestController<TSubject, TResult, TNewVars, TCombi> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      var delegateControllers = _controllers.Select(x => x.SetVariables(variablesProvider));
      return _controllerFactory.CreateCompositeTestController(delegateControllers);
    }

    public ITestController<TSubject, TResult, Dummy, TNewCombi> SetCombinations<TNewCombi> (IDictionary<string, TNewCombi> combinations)
    {
      throw new NotSupportedException();
    }

    public void AddArrangement (string text, Arrangement<TSubject, TResult, TVars, TCombi> arrangement)
    {
      _controllers.ForEach(x => x.AddArrangement(text, arrangement));
    }

    public void AddAssertion (string text, Assertion<TSubject, TResult, TVars, TCombi> assertion, bool expectException = false)
    {
      _controllers.ForEach(x => x.AddAssertion(text, assertion, expectException));
    }

    public ITestController<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi> ()
    {
      var delegateControllers = _controllers.Select(x => x.CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateCombi>());
      return _controllerFactory.CreateCompositeTestController(delegateControllers);
    }
  }
}