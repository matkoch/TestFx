﻿// Copyright 2015, 2014 Matthias Koch
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

namespace TestFx.Extensibility.Contexts
{
  public class DelegateTestContext : ITestContext
  {
    private readonly TestContext _context;

    public DelegateTestContext (TestContext context)
    {
      _context = context;
    }

    public bool IsFailing
    {
      get { return _context.IsFailing; }
      set { _context.IsFailing = value; }
    }

    public object this [string key]
    {
      get { return _context[key]; }
      set { _context[key] = value; }
    }

    public bool HasKey (string key)
    {
      return _context.HasKey(key);
    }
  }
}