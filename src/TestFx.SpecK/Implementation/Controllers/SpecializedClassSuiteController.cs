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
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.SpecK.InferredApi;

namespace TestFx.SpecK.Implementation.Controllers
{
  public interface ISpecializedSuiteController<TSubject, out TResult> : ISuiteController
  {
    void IgnoreNext ();

    ITestController<TSubject, TResult, Dummy, Dummy> CreateTestController (string description);
  }

  public class SpecializedSuiteController<TSubject, TResult> : SuiteController, ISpecializedSuiteController<TSubject, TResult>
  {
    private readonly SuiteProvider _provider;
    private readonly ActionContainer<TSubject, TResult> _actionContainer;
    private readonly Action<ITestController> _testControllerConfigurator;
    private readonly IControllerFactory _controllerFactory;

    private bool _ignoreNext;

    public SpecializedSuiteController (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        Action<ITestController> testControllerConfigurator,
        IControllerFactory controllerFactory,
        IOperationSorter operationSorter)
        : base(provider, operationSorter)
    {
      _provider = provider;
      _actionContainer = actionContainer;
      _testControllerConfigurator = testControllerConfigurator;
      _controllerFactory = controllerFactory;
    }

    public void IgnoreNext ()
    {
      _ignoreNext = true;
    }

    public ITestController<TSubject, TResult, Dummy, Dummy> CreateTestController (string text)
    {
      var testProvider = CreateTestProvider(text, text, _ignoreNext);
      var controller = _controllerFactory.CreateMainTestController<TSubject, TResult, Dummy, Dummy>(
          _provider,
          testProvider,
          _testControllerConfigurator,
          _actionContainer,
          new Dummy());

      _ignoreNext = false;
      return controller;
    }
  }
}