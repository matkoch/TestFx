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
using TestFx.Utilities.Collections;
using TestFx.Utilities.Introspection;

namespace TestFx.Extensibility
{
  public interface IIntrospectionPresenter
  {
    string Present (CommonAttribute displayAttribute, CommonType declaringType, CommonAttribute subjectAttribute);
    string Present (CommonAttribute displayAttribute, IDictionary<string, object> arguments);
    string Present(string displayFormat, IDictionary<string, object> arguments);
  }

  public class IntrospectionPresenter : IIntrospectionPresenter
  {
    public const string UnknownValue = "???";

    public string Present(CommonAttribute displayAttribute, CommonType declaringType, CommonAttribute subjectAttribute)
    {
      var dictionary = Tuple.Create("type", (object) declaringType)
          .Concat(subjectAttribute.PositionalArguments.Select(x => Tuple.Create(x.Position.ToString(), x.Value)))
          .Concat(subjectAttribute.NamedArguments.Select(x => Tuple.Create(x.Name, x.Value)))
          .ToDictionary(x => x.Item1, x => x.Item2);
      return Present(GetDisplayFormat(displayAttribute), dictionary);
    }

    public string Present(CommonAttribute displayAttribute, IDictionary<string, object> arguments)
    {
      return Present(GetDisplayFormat(displayAttribute), arguments);
    }

    public string Present(string displayFormat, IDictionary<string, object> arguments)
    {
      return arguments.Aggregate(displayFormat, (current, pair) => current.Replace("{" + pair.Key + "}", pair.Value.ToString()));
    }

    private string GetDisplayFormat (CommonAttribute displayAttribute)
    {
      return (string) displayAttribute.PositionalArguments.Single().Value;
    }
  }
}