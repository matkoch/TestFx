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
using TestFx.Extensibility.Contexts;

namespace TestFx.SpecK.Implementation.Contexts
{
  public abstract class TestContext<TSubject, TResult, TVars, TSequence> : TestContext, ITestContext<TSubject, TResult, TVars, TSequence>
  {
    public TestContext<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence>
        CreateDelegate<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence> ()
    {
      return new DelegateTestContext<TDelegateSubject, TDelegateResult, TDelegateVars, TDelegateSequence, TSubject, TResult, TVars, TSequence>(this);
    }

    public abstract TSubject Subject { get; set; }
    public abstract TResult Result { get; set; }
    public abstract object VarsObject { get; set; }
    public abstract TVars Vars { get; set; }
    public abstract object SeqObject { get; set; }
    public abstract TSequence Sequence { get; set; }
    public abstract Exception Exception { get; set; }
    public abstract TimeSpan Duration { get; set; }
    public abstract bool ExpectsException { get; set; }
  }
}