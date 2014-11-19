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
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using TestFx.Utilities.Reflection;

namespace TestFx.Utilities.Expressions
{
  public class ExpressionParser
  {
    private readonly Type[] _strippedTypes;
    private readonly StringBuilder _builder;

    public ExpressionParser (Type[] strippedTypes)
    {
      _strippedTypes = strippedTypes;
      _builder = new StringBuilder();
    }

    public string Parse (LambdaExpression expression)
    {
      Visit(expression.Body);

      return _builder.ToString();
    }

    private void Visit (Expression expression)
    {
      if (expression is MethodCallExpression)
        VisitMethodCall(expression.To<MethodCallExpression>());
      else if (expression is ConstantExpression)
        VisitConstant(expression.To<ConstantExpression>());
      else if (expression is MemberExpression)
        VisitMember(expression.To<MemberExpression>());
      else if (expression is UnaryExpression)
        VisitUnary(expression.To<UnaryExpression>());
      else if (expression is ParameterExpression)
        VisitParameter(expression.To<ParameterExpression>());
      else if (expression is NewArrayExpression)
        VisitNewArray(expression.To<NewArrayExpression>());
      else
        throw new Exception(expression.GetType().Name);
    }

    private void VisitMethodCall (MethodCallExpression expression)
    {
      var instance = expression.Object;
      var arguments = expression.Arguments.ToList();
      var method = expression.Method;
      if (method.IsExtensionMethod())
      {
        instance = arguments[0];
        arguments = arguments.Skip(1).ToList();
      }

      if (instance != null)
      {
        if (!_strippedTypes.Any(x => x.IsAssignableFrom(instance.Type)))
        {
          Visit(instance);
          _builder.Append(".");
        }
      }
      else if (method.IsStatic)
      {
        _builder.Append(method.DeclaringType.Name);
        _builder.Append(".");
      }

      _builder.Append(method.Name);

      _builder.Append("(");
      for (var i = 0; i < arguments.Count; i++)
      {
        Visit(arguments[i]);
        if (i < arguments.Count - 1)
          _builder.Append(", ");
      }
      _builder.Append(")");
    }

    private void VisitConstant (ConstantExpression expression)
    {
      if (expression.Type != typeof (string))
        _builder.Append(expression.Value);
      else
        _builder.Append("\"").Append(expression.Value).Append("\"");
    }

    private void VisitMember (MemberExpression expression)
    {
      if (expression.Expression != null && !_strippedTypes.Any(x => x.IsAssignableFrom(expression.Expression.Type)))
      {
        _builder.Append(expression.Expression.Type.Name);
        _builder.Append(".");
      }
      _builder.Append(expression.Member.Name);
    }

    private void VisitUnary (UnaryExpression expression)
    {
      Visit(expression.Operand);
    }

    private void VisitParameter (ParameterExpression expression)
    {
      _builder.Append(expression.Type.Name);
    }

    private void VisitNewArray (NewArrayExpression expression)
    {
      _builder.Append("[");

      var items = expression.Expressions;
      for (var i = 0; i < items.Count; i++)
      {
        _builder.Append(" ");
        Visit(items[i]);
        _builder.Append(" ");

        if (i < items.Count - 1)
          _builder.Append(",");
      }

      _builder.Append("]");
    }
  }
}