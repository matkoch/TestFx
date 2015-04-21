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
using JetBrains.Annotations;
using TestFx.Evaluation.Results;
using TestFx.Extensibility.Providers;
using TestFx.Utilities;

namespace TestFx.Evaluation.Reporting
{
  public class FillingOperationResult : IOperationResult
  {
    public IIdentity Identity
    {
      get { throw new NotSupportedException(); }
    }

    public string Text
    {
      get { throw new NotSupportedException(); }
    }

    public State State
    {
      get { throw new NotSupportedException(); }
    }

    public OperationType Type
    {
      get { throw new NotSupportedException(); }
    }

    [CanBeNull]
    public IExceptionDescriptor Exception
    {
      get { return null; }
    }
  }
}