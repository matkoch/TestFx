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
using System.Collections.Generic;
using System.Linq;
using TestFx.Utilities;
using TestFx.Utilities.Introspection;

namespace TestFx.Extensibility
{
  public interface IIntrospectionPresenter
  {
    string Present (CommonAttribute displayAttribute, CommonAttribute subjectAttribute);
    string Present (string displayFormat, CommonAttribute subjectAttribute);

    string Present (CommonAttribute displayAttribute, IEnumerable<object> arguments);
    string Present (string displayFormat, IEnumerable<object> arguments);
  }

  public partial class IntrospectionPresenter : IIntrospectionPresenter
  {
    public const string UnknownValue = "???";

    public string Present (CommonAttribute displayAttribute, CommonAttribute subjectAttribute)
    {
      return Present(GetDisplayFormat(displayAttribute), subjectAttribute);
    }

    public string Present (string displayFormat, CommonAttribute subjectAttribute)
    {
      return Present(displayFormat, subjectAttribute.PositionalArguments.Select(x => x.Value));
    }

    public string Present (CommonAttribute displayAttribute, IEnumerable<object> arguments)
    {
      return Present(GetDisplayFormat(displayAttribute), arguments);
    }

    public string Present (string displayFormat, IEnumerable<object> arguments)
    {
      return String.Format(displayFormat, arguments.Concat(Enumerable.Repeat(UnknownValue, 10)).ToArray());
    }

    private string GetDisplayFormat (CommonAttribute displayAttribute)
    {
      return (string) displayAttribute.PositionalArguments.Single().Value;
    }
  }
}