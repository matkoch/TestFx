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
using TestFx.Evaluation.Results;
using TestFx.Evaluation.Utilities;
using TestFx.Extensibility.Providers;

namespace TestFx.Evaluation.Runners
{
  public interface IOperationRunner
  {
    IOperationResult Run (IOperationProvider provider);
  }

  public class OperationRunner : IOperationRunner
  {
    private readonly IResultFactory _resultFactory;

    public OperationRunner (IResultFactory resultFactory)
    {
      _resultFactory = resultFactory;
    }

    public IOperationResult Run (IOperationProvider provider)
    {
      if (provider.Action == OperationProvider.NotImplemented)
        return _resultFactory.CreateNotImplementedOperationResult(provider);

      try
      {
        provider.Action();
        return _resultFactory.CreatePassedOperationResult(provider);
      }
      catch (Exception exception)
      {
        return _resultFactory.CreateFailedOperationResult(provider, exception);
      }
    }
  }
}