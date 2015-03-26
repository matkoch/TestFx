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
using System.Globalization;
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Utilities;

namespace TestFx.Specifications.Implementation.Controllers
{
  public interface ISpecializedSuiteController<TSubject, out TResult> : ISuiteController
  {
    void IgnoreNext ();

    ITestController<TSubject, TResult, object> CreateTestController (string description);
  }

  public class SpecializedSuiteController<TSubject, TResult> : SuiteController, ISpecializedSuiteController<TSubject, TResult>
  {
    private readonly ActionContainer<TSubject, TResult> _actionContainer;
    private readonly IClassSuiteController<TSubject> _classSuiteController;
    private readonly IControllerFactory _controllerFactory;

    private int _sequenceNumber;
    private bool _ignoreNext;

    public SpecializedSuiteController (
        SuiteProvider provider,
        ActionContainer<TSubject, TResult> actionContainer,
        IClassSuiteController<TSubject> classSuiteController,
        IControllerFactory controllerFactory,
        IOperationSorter operationSorter)
        : base(provider, operationSorter)
    {
      _actionContainer = actionContainer;
      _classSuiteController = classSuiteController;
      _controllerFactory = controllerFactory;
    }

    public void IgnoreNext ()
    {
      _ignoreNext = true;
    }

    public ITestController<TSubject, TResult, object> CreateTestController (string text)
    {
      var provider = CreateTestProvider(_sequenceNumber++.ToString(CultureInfo.InvariantCulture), text, _ignoreNext);
      var controller = _controllerFactory.CreateMainTestController(provider, _actionContainer);
      _classSuiteController.ConfigureTestController(controller);

      _ignoreNext = false;
      return controller;
    }
  }
}