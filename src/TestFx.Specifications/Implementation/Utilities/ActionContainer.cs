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
using JetBrains.Annotations;

namespace TestFx.Specifications.Implementation.Utilities
{
  public class ActionContainer<TSubject, TResult>
  {
    private readonly string _text;
    private readonly Action<TSubject> _voidAction;
    private readonly Func<TSubject, TResult> _resultAction;

    public ActionContainer (string text, [CanBeNull] Action<TSubject> voidAction, [CanBeNull] Func<TSubject, TResult> resultAction)
    {
      Debug.Assert(voidAction != null || resultAction != null);

      _text = text;
      _voidAction = voidAction;
      _resultAction = resultAction;
    }

    public string Text
    {
      get { return _text; }
    }

    [CanBeNull]
    public Action<TSubject> VoidAction
    {
      get { return _voidAction; }
    }

    [CanBeNull]
    public Func<TSubject, TResult> ResultAction
    {
      get { return _resultAction; }
    }
  }
}