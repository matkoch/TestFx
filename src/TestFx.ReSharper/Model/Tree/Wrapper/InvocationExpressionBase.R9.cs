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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract partial class InvocationExpressionBase
  {
    public ICSharpExpression SetConditionalQualifier (ICSharpExpression expression)
    {
      return _invocationExpression.SetConditionalQualifier(expression);
    }

    public void SetConditionalAccessSign (bool value)
    {
      _invocationExpression.SetConditionalAccessSign(value);
    }

    public IExpressionType UnliftedExpressionType ()
    {
      return _invocationExpression.UnliftedExpressionType();
    }

    public bool HasConditionalAccessSign
    {
      get { return _invocationExpression.HasConditionalAccessSign; }
    }

    public ICSharpExpression ConditionalQualifier
    {
      get { return _invocationExpression.ConditionalQualifier; }
    }

    public IConditionalAccessSign ConditionalAccessSign
    {
      get { return _invocationExpression.ConditionalAccessSign; }
    }
  }
}