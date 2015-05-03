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

namespace TestFx.Specifications.Implementation.Contexts
{
  public class DelegateTestContext<TDelegateSubject, TDelegateResult, TDelegateVars, TSubject, TResult, TVars>
      : TestContext<TDelegateSubject, TDelegateResult, TDelegateVars>
  {
    private readonly TestContext<TSubject, TResult, TVars> _context;
    private object _varsObject;

    public DelegateTestContext (TestContext<TSubject, TResult, TVars> context)
    {
      _context = context;
    }

    public override TDelegateSubject Subject
    {
      get { return (TDelegateSubject) (object) _context.Subject; }
      set { _context.Subject = (TSubject) (object) value; }
    }

    public override TDelegateResult Result
    {
      get { return (TDelegateResult) (object) _context.Result; }
      set { throw new NotSupportedException(); }
    }

    public override object VarsObject
    {
      get { return _context.VarsObject; }
      set { _context.VarsObject = value; }
    }

    public override TDelegateVars Vars
    {
      get { return (TDelegateVars) _context.VarsObject; }
      set { _context.VarsObject = (TVars) (object) value; }
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