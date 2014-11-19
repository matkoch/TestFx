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
using System.Collections;
using System.Runtime.Serialization;

namespace TestFx.Utilities
{
  /// <summary>
  /// Exception that is thrown when ordering a set of items where the order is not totally defined.
  /// </summary>
  public class UndefinedOrderException : Exception
  {
    private const string c_message = "Undefined order for items:\r\n";

    public UndefinedOrderException (IEnumerable items)
    {
      Items = items;
    }

    protected UndefinedOrderException (SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public IEnumerable Items { get; private set; }

    public override string Message
    {
      get { return c_message + Items; }
    }
  }
}