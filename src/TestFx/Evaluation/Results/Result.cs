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
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  public interface IResult : IIdentifiable
  {
    string Text { get; }
    State State { get; }
  }

  [Serializable]
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public abstract class Result : IResult
  {
    private readonly IIdentity _identity;
    private readonly string _text;
    private readonly State _state;

    protected Result (IIdentity identity, string text, State state)
    {
      _identity = identity;
      _text = text;
      _state = state;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public string Text
    {
      get { return _text; }
    }

    public State State
    {
      get { return _state; }
    }
  }
}