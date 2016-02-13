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
using TestFx.Extensibility.Controllers;
using TestFx.Extensibility.Providers;
using TestFx.Extensibility.Utilities;

namespace TestFx.Evaluation.Loading
{
  public interface ISuiteControllerFactory
  {
    ISuiteController Create (SuiteProvider suiteProvider);
  }

  internal class SuiteControllerFactory : ISuiteControllerFactory
  {
    private readonly IOperationSorter _operationSorter;

    public SuiteControllerFactory (IOperationSorter operationSorter)
    {
      _operationSorter = operationSorter;
    }

    public ISuiteController Create (SuiteProvider suiteProvider)
    {
      return new SuiteController(suiteProvider, _operationSorter);
    }
  }
}