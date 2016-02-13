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
using JetBrains.Annotations;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface IOperationResult : IResult
  {
    OperationType Type { get; }

    [CanBeNull]
    IExceptionDescriptor Exception { get; }
  }

  [Serializable]
  internal class OperationResult : Result, IOperationResult
  {
    private readonly OperationType _type;
    private readonly IExceptionDescriptor _exception;

    public OperationResult (IIdentity identity, string text, OperationType type, State state, [CanBeNull] IExceptionDescriptor exception)
        : base(identity, text, state)
    {
      _type = type;
      _exception = exception;
    }

    public OperationType Type
    {
      get { return _type; }
    }

    [CanBeNull]
    public IExceptionDescriptor Exception
    {
      get { return _exception; }
    }
  }
}