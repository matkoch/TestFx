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
using System.Globalization;
using System.IO;

namespace TestFx.Evaluation.Utilities
{
  public class DelegateStringWriter : StringWriter
  {
    private readonly Action<string> _write;

    public DelegateStringWriter (Action<string> write)
    {
      _write = write;
    }

    public override void Write (string value)
    {
      _write(value);
    }

    public override void Write (char[] buffer, int index, int count)
    {
      _write(new string(buffer, index, count));
    }

    public override void Write (char value)
    {
      _write(value.ToString(CultureInfo.InvariantCulture));
    }
  }
}