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
using System.Linq;

namespace Example._103_Calculator
{
  public class Calculator
  {
    enum Mode
    {
      Plus,
      Minus
    }

    Mode _mode;

    readonly List<int> _values = new List<int> ();

    public int PressEquals ()
    {
      return _values.Aggregate (0, (a, c) => a + c);
    }

    public void PressPlus ()
    {
      _mode = Mode.Plus;
    }

    public void PressMinus ()
    {
      _mode = Mode.Minus;
    }

    public void Enter (int value)
    {
      if (_mode == Mode.Plus)
        _values.Add (value);
      else
        _values.Add (value * -1);
    }
  }
}