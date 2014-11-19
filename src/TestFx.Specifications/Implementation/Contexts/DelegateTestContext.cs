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
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Specifications.Implementation.Contexts
{
  public class DelegateTestContext<TDelegateSubject, TDelegateResult, TDelegateVars, TSubject, TResult, TVars>
      : TestContext<TDelegateSubject, TDelegateResult, TDelegateVars>
  {
    private readonly TestContext<TSubject, TResult, TVars> _context;

    public DelegateTestContext (TestContext<TSubject, TResult, TVars> context)
    {
      _context = context;
    }

    public override TDelegateSubject Subject
    {
      get { return _context.Subject.To<TDelegateSubject>(); }
      set { _context.Subject = value.To<TSubject>(); }
    }

    public override TDelegateResult Result
    {
      get { return _context.Result.To<TDelegateResult>(); }
      set { throw new NotSupportedException(); }
    }

    public override TDelegateVars Vars
    {
      get { return _context.Vars.To<TDelegateVars>(); }
      set { throw new NotSupportedException(); }
    }

    public override Exception Exception
    {
      get { return _context.Exception; }
      set { throw new NotSupportedException(); }
    }

    public override TimeSpan Duration
    {
      get { return _context.Duration; }
      set { throw new NotSupportedException(); }
    }

    public override bool ExpectsException
    {
      get { return _context.ExpectsException; }
      set { _context.ExpectsException = value; }
    }
  }
}