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
using JetBrains.Annotations;

namespace TestFx.Utilities.Introspection
{
  public class CommonInvocationExpression : CommonExpression
  {
    private readonly CommonExpression _instance;
    private readonly CommonMethodInfo _method;
    private readonly IEnumerable<CommonExpression> _arguments;

    public CommonInvocationExpression ([CanBeNull] CommonExpression instance, CommonMethodInfo method, IEnumerable<CommonExpression> arguments)
        : base(method.Type)
    {
      _instance = instance;
      _method = method;
      _arguments = arguments.ToList();
    }

    [CanBeNull]
    public CommonExpression Instance
    {
      get { return _instance; }
    }

    public CommonMethodInfo Method
    {
      get { return _method; }
    }

    public IEnumerable<CommonExpression> Arguments
    {
      get { return _arguments; }
    }
  }
}