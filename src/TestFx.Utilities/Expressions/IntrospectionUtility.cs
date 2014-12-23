using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using TestFx.Utilities.Introspection;
using TestFx.Utilities.Reflection;

namespace TestFx.Utilities.Expressions
{
  public interface IIntrospectionUtility
  {
    CommonExpressionProvider GetCommonExpressionProvider (Expression expression, IEnumerable<Type> strippedTypes);
  }

  public class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonExpressionProvider GetCommonExpressionProvider (Expression expression, IEnumerable<Type> strippedTypes)
    {
      return new CommonExpressionProvider(() => Visit(expression), strippedTypes.Select(x => x.ToCommon()));
    }

    private CommonExpression Visit (Expression expression)
    {
      if (expression == null)
        return null;

      if (expression is LambdaExpression)
        return VisitLambda(expression.To<LambdaExpression>());
      if (expression is ConstantExpression)
        return VisitConstant(expression.To<ConstantExpression>());
      if (expression is MethodCallExpression)
        return VisitMethodCall(expression.To<MethodCallExpression>());
      if (expression is MemberExpression)
        return VisitMember(expression.To<MemberExpression>());
      if (expression is ParameterExpression)
        return VisitParameter(expression.To<ParameterExpression>());
      if (expression is NewArrayExpression)
        return VisitNewArray(expression.To<NewArrayExpression>());

      throw new Exception(string.Format("Expressions of type {0} are not supported.", expression.GetType()));
    }

    private CommonExpression VisitParameter (ParameterExpression expression)
    {
      return new CommonParameterExpression(expression.Type.ToCommon(), expression.Name);
    }

    private CommonExpression VisitMember (MemberExpression expression)
    {
      return new CommonMemberAccessExpression(Visit(expression.Expression), expression.Member.ToCommon(), new CommonExpression[0]);
    }

    private CommonExpression VisitMethodCall (MethodCallExpression expression)
    {
      var instance = Visit(expression.Object);
      var arguments = expression.Arguments.Select(Visit);

      var propertyInfo = expression.Method.GetPropertyInfo();
      if (propertyInfo != null)
        return new CommonMemberAccessExpression(instance, propertyInfo.ToCommon(), arguments);

      var eventInfo = expression.Method.GetEventInfo();
      Trace.Assert(eventInfo == null, "Expressions accessing events are not supported.");

      return new CommonInvocationExpression(instance, expression.Method.ToCommon(), arguments);
    }

    private CommonExpression VisitConstant (ConstantExpression expression)
    {
      var value = expression.Value is Type ? expression.Value.To<Type>().ToCommon() : expression.Value;

      return new CommonConstantExpression(expression.Type.ToCommon(), value);
    }

    private CommonExpression VisitLambda (LambdaExpression expression)
    {
      return Visit(expression.Body);
    }

    private CommonExpression VisitNewArray (NewArrayExpression expression)
    {
      var items = expression.Expressions.Select(Visit);
      return new CommonArrayItemsExpression(items, expression.Type.ToCommon());
    }
  }
}