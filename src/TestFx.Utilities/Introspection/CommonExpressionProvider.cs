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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestFx.Utilities.Introspection
{
  public class CommonExpressionProvider
  {
    private readonly Lazy<CommonExpression> _lazyExpression;
    private readonly Lazy<string> _toString;

    public CommonExpressionProvider (Func<CommonExpression> factory, IEnumerable<CommonType> strippedTypes)
    {
      _lazyExpression = new Lazy<CommonExpression>(factory);
      _toString = new Lazy<string>(() => new Parser(Expression, strippedTypes.Select(x => x.Fullname)).ToString());
    }

    public CommonExpression Expression
    {
      get { return _lazyExpression.Value; }
    }

    public override string ToString ()
    {
      return _toString.Value;
    }

    private class Parser
    {
      private readonly CommonExpression _expression;
      private readonly IEnumerable<string> _strippedTypeFullNames;
      private readonly StringBuilder _builder;

      public Parser (CommonExpression expression, IEnumerable<string> strippedTypeFullNames)
      {
        _expression = expression;
        _strippedTypeFullNames = strippedTypeFullNames;
        _builder = new StringBuilder();
      }

      private void Visit (CommonExpression expression)
      {
        if (expression is CommonInvocationExpression)
          VisitMethodCall(expression.To<CommonInvocationExpression>());
        else if (expression is CommonConstantExpression)
          VisitConstant(expression.To<CommonConstantExpression>());
        else if (expression is CommonMemberAccessExpression)
          VisitMemberAccess(expression.To<CommonMemberAccessExpression>());
        else if (expression is CommonParameterExpression)
          VisitParameter(expression.To<CommonParameterExpression>());
        else if (expression is CommonThisExpression)
          VisitThis(expression.To<CommonThisExpression>());
        else if (expression is CommonArrayItemsExpression)
          VisitArrayItems(expression.To<CommonArrayItemsExpression>());
        else
          throw new Exception(string.Format("Expressions of type {0} are not supported.", expression.GetType()));
      }

      private void VisitThis (CommonThisExpression expression)
      {
        _builder.Append(expression.Type.Name);
      }

      private void VisitMethodCall (CommonInvocationExpression expression)
      {
        var instance = expression.Instance;
        var arguments = expression.Arguments.ToArray();
        var method = expression.Method;
        if (method.IsExtension)
        {
          instance = arguments.First();
          arguments = arguments.Skip(1).ToArray();
        }

        if (instance != null)
        {
          if (!_strippedTypeFullNames.Any(x => instance.Type.IsAssignableTo(x)))
          {
            Visit(instance);
            _builder.Append(".");
          }
        }
        else
        {
          Trace.Assert(method.IsStatic);
          _builder.Append(method.DeclaringType.Name);
          _builder.Append(".");
        }

        _builder.Append(method.Name);

        _builder.Append("(");
        VisitEnumerable(arguments.ToArray());
        _builder.Append(")");
      }

      private void VisitConstant (CommonConstantExpression expression)
      {
        if (expression.Type.Fullname != typeof (string).FullName)
          _builder.Append(expression.Value);
        else
          _builder.Append("\"").Append(expression.Value).Append("\"");
      }

      private void VisitMemberAccess (CommonMemberAccessExpression expression)
      {
        // TODO: better API for CommonType
        if (expression.Instance != null && !_strippedTypeFullNames.Any(x => expression.Instance.Type.IsAssignableTo(x)))
        {
          _builder.Append(expression.Instance.Type.Name);
          _builder.Append(".");
        }
        _builder.Append(expression.Member.Name);
      }

      private void VisitArrayItems (CommonArrayItemsExpression expression)
      {
        _builder.Append("[ ");
        VisitEnumerable(expression.Items.ToArray());
        _builder.Append(" ]");
      }

      //private void VisitUnary (UnaryExpression expression)
      //{
      //  Visit(expression.Operand);
      //}

      private void VisitParameter (CommonParameterExpression expression)
      {
        _builder.Append(expression.Type.Name);
      }

      private void VisitEnumerable (CommonExpression[] expressions)
      {
        for (var i = 0; i < expressions.Length; i++)
        {
          Visit(expressions[i]);
          if (i < expressions.Length - 1)
            _builder.Append(", ");
        }
      }

      //private void VisitNewArray (NewArrayExpression expression)
      //{
      //  _builder.Append("[");

      //  var items = expression.Expressions;
      //  for (var i = 0; i < items.Count; i++)
      //  {
      //    _builder.Append(" ");
      //    Visit(items[i]);
      //    _builder.Append(" ");

      //    if (i < items.Count - 1)
      //      _builder.Append(",");
      //  }

      //  _builder.Append("]");
      //}

      public override string ToString ()
      {
        if (_builder.Length == 0)
          Visit(_expression);

        return _builder.ToString();
      }
    }
  }
}