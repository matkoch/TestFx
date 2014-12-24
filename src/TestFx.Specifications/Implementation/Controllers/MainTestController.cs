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
using System.Diagnostics;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;
using TestFx.Specifications.Implementation.Contexts;
using TestFx.Specifications.Implementation.Utilities;
using TestFx.Specifications.InferredApi;

namespace TestFx.Specifications.Implementation.Controllers
{
  public class MainTestController<TSubject, TResult> : TestController<TSubject, TResult, object>
  {
    private readonly MainTestContext<TSubject, TResult> _context;

    public MainTestController (
        TestProvider provider,
        MainTestContext<TSubject, TResult> context,
        ActionContainer<TSubject, TResult> actionContainer,
        IOperationSorter operationSorter,
        IControllerFactory controllerFactory)
        : base(provider, context, operationSorter, controllerFactory)
    {
      _context = context;

      var wrappedAction = actionContainer.VoidAction != null
          ? GuardAction(actionContainer.VoidAction)
          : GuardAction(x => _context.Result = actionContainer.ResultAction(x));
      AddAction<Act>(actionContainer.Text, x => wrappedAction());
    }

    public override ITestController<TSubject, TResult, TNewVars> SetVariables<TNewVars> (Func<Dummy, TNewVars> variablesProvider)
    {
      _context.Vars = variablesProvider(null);
      return CreateDelegate<TSubject, TResult, TNewVars>();
    }

    private Action GuardAction (Action<TSubject> action)
    {
      return () =>
      {
        try
        {
          var stopwatch = Stopwatch.StartNew();
          action(_context.Subject);
          _context.Duration = stopwatch.Elapsed;
        }
        catch (Exception exception)
        {
          if (!_context.ExpectsException)
            throw;
          _context.Exception = exception;
        }
        finally
        {
          _context.ActionExecuted = true;
        }
      };
    }
  }
}