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

namespace TestFx.Utilities.Introspection
{
  public class CommonNamedArgument
  {
    public CommonNamedArgument (string name, CommonType type, object value)
    {
      Name = name;
      Type = type;
      Value = value;
    }

    public string Name { get; }

    public CommonType Type { get; }

    public object Value { get; }
  }
}