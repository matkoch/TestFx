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
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.Specifications.Implementation.Utilities
{
  /// <summary>
  /// Generator for a factory delegate that creates an instance of the subject based on the fields marked with <see cref="InjectedAttribute"/>
  /// available in the <see cref="ISpecK{TSubject}"/> instance.
  /// </summary>
  public interface ISubjectFactoryGenerator
  {
    Delegate GetFactory (Type suiteType);
  }

  /// <summary>
  /// </summary>
  public class SubjectFactoryGenerator : ISubjectFactoryGenerator
  {
    public Delegate GetFactory (Type suiteType)
    {
      var closedSpeckType = suiteType.GetClosedTypeOf(typeof (ISpecK<>)).AssertNotNull();
      var subjectType = closedSpeckType.GetGenericArguments().Single();
      var delegateType = typeof (Func<,>).MakeGenericType(closedSpeckType, subjectType);

      var suiteParameter = Expression.Parameter(closedSpeckType, "suite");
      var constructingExpression = CreateConstructingExpression(suiteType, subjectType, suiteParameter);
      return Expression.Lambda(delegateType, constructingExpression, suiteParameter).Compile();
    }

    private Expression CreateConstructingExpression (Type suiteType, Type subjectType, Expression suiteExpression)
    {
      var constructors = subjectType.GetConstructors(MemberBindings.Instance);
      if (constructors.Length != 1)
      {
        var exception = CreateThrowExpression<Exception>("Missing default constructor for subject type '{0}'.", subjectType.Name);
        return Expression.Throw(exception, subjectType);
      }

      var constructor = constructors.Single();
      var constructorParameters = constructor.GetParameters();

      var suiteFields = suiteType.GetFieldsWithAttribute<InjectedAttribute>().Select(x => x.Item1).ToList();
      var castedSuiteExpression = Expression.Convert(suiteExpression, suiteType);
      var argumentAccessExpressions =
          constructorParameters.Select(x => CreateArgumentAccessExpression(x, castedSuiteExpression, suiteFields)).ToList();
      if (argumentAccessExpressions.Any(x => x == null))
      {
        var missingParameters = constructorParameters.Select((x, i) => Tuple.Create(x, argumentAccessExpressions[i]))
            .Where(x => x.Item2 == null)
            .Select(x => x.Item1.AssertNotNull().Name);

        var exception = CreateThrowExpression<Exception>(
            "Missing constructor arguments for subject type '{0}': {1}",
            subjectType.Name,
            string.Join(", ", missingParameters.ToArray()));
        return Expression.Throw(exception, subjectType);
      }

      return Expression.New(constructor, argumentAccessExpressions);
    }

    private Expression CreateArgumentAccessExpression (ParameterInfo parameter, Expression suiteExpression, IList<FieldInfo> candidates)
    {
      var argumentField = candidates.SingleOrDefault(x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
      if (argumentField != null)
        return Expression.Field(argumentField.IsStatic ? null : suiteExpression, argumentField);

      return null;
    }

    private Expression CreateThrowExpression<T> (string format, params object[] args) where T : Exception
    {
      var constructor = typeof (T).GetConstructor(new[] { typeof (string) }).AssertNotNull();
      Debug.Assert(constructor.GetParameters().Single().Name == "message");
      var message = string.Format(format, args);
      return Expression.New(constructor, Expression.Constant(message));
    }
  }
}