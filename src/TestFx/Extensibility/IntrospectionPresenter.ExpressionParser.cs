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
using System.Text;
using JetBrains.Annotations;
using TestFx.Utilities;
using TestFx.Utilities.Introspection;

namespace TestFx.Extensibility
{
  public partial class IntrospectionPresenter
  {
    private class ExpressionParser
    {
      private readonly CommonExpressionProvider _expressionProvider;
      private readonly IEnumerable<string> _strippedTypeFullNames;
      private readonly StringBuilder _builder;

      public ExpressionParser (CommonExpressionProvider expressionProvider, IEnumerable<string> strippedTypeFullNames)
      {
        _expressionProvider = expressionProvider;
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
        else
          throw new Exception(expression.GetType().Name);
      }

      private void VisitThis (CommonThisExpression expression)
      {
        _builder.Append(expression.Type.Name);
      }

      private void VisitMethodCall (CommonInvocationExpression expression)
      {
        if (expression.Instance != null)
        {
          Visit(expression.Instance);
          _builder.Append(".");
        }
        else if (expression.Method.IsStatic)
        {
          _builder.Append(expression.Method.DeclaringType.Name);
          _builder.Append(".");
        }

        _builder.Append(expression.Method.Name);

        _builder.Append("(");
        var arguments = expression.Arguments.ToList();
        for (var i = 0; i < arguments.Count; i++)
        {
          Visit(arguments[i]);
          if (i < arguments.Count - 1)
            _builder.Append(", ");
        }
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
        if (!_strippedTypeFullNames.Any(x => expression.Instance.Type.IsAssignableTo(x)))
        {
          _builder.Append(expression.Instance.Type.Name);
          _builder.Append(".");
        }
        _builder.Append(expression.Member.Name);
      }

      //private void VisitUnary (UnaryExpression expression)
      //{
      //  Visit(expression.Operand);
      //}

      private void VisitParameter (CommonParameterExpression expression)
      {
        _builder.Append(expression.Type.Name);
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
          Visit(_expressionProvider.Expression);

        return _builder.ToString();
      }
    }
  }
}