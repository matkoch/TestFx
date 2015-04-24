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
using System.Collections.Generic;

namespace TestFx.Extensibility.Contexts
{
  public interface ITestContext
  {
    bool IsFailing { get; }
    object this [string key] { get; set; }
    bool HasKey(string key);
  }

  public class TestContext : ITestContext
  {
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    public bool IsFailing { get; set; }

    public object this [string key]
    {
      get { return _data[key]; }
      set { _data[key] = value; }
    }

    public bool HasKey(string key)
    {
      return _data.ContainsKey(key);
    }
  }
}