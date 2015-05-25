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
using System.Linq.Expressions;
using TestFx.Specifications.InferredApi;
using TestFx.Utilities.Reflection;

namespace TestFx.Specifications
{
  public static class CombinatoricExtensions
  {
    public static IArrangeOrAssert<TSubject, TResult, Dummy, TNewCombi> WithSequences<TSubject, TResult, TNewCombi> (
        this ICombine<TSubject, TResult> combine,
        string text1,
        TNewCombi sequence1,
        string text2,
        TNewCombi sequence2)
    {
      return combine.WithCombinations(
          new Dictionary<string, TNewCombi>
          {
              { text1, sequence1 },
              { text2, sequence2 }
          });
    }

    public static IArrangeOrAssert<TSubject, TResult, Dummy, TNewCombi> WithSequences<TSubject, TResult, TNewCombi> (
        this ICombine<TSubject, TResult> combine,
        TNewCombi sequence1,
        TNewCombi sequence2)
    {
      return combine.WithCombinations(
          new Dictionary<string, TNewCombi>
          {
              { GetText(sequence1), sequence1 },
              { GetText(sequence2), sequence2 }
          });
    }

    public static IArrangeOrAssert<TSubject, TResult, Dummy, TNewCombi> WithPermutations<TSubject, TResult, TNewCombi, T1, T2> (
        this ICombine<TSubject, TResult> combine,
        TNewCombi template,
        Expression<Func<TNewCombi, T1>> propertySelector1,
        IEnumerable<T1> propertyValues1,
        Expression<Func<TNewCombi, T2>> propertySelector2,
        IEnumerable<T2> propertyValues2)
    {
      var factory = CreateFactory<TNewCombi>(propertySelector1, propertySelector2);
      var values =
          from propertyValue1 in propertyValues1.ToList()
          from propertyValue2 in propertyValues2.ToList()
          select factory(new object[] { propertyValue1, propertyValue2 });

      return combine.WithCombinations(values.ToDictionary(GetText, x => x));
    }

    public static IArrangeOrAssert<TSubject, TResult, Dummy, TNewCombi> WithPermutations<TSubject, TResult, TNewCombi, T1, T2, T3> (
        this ICombine<TSubject, TResult> combine,
        TNewCombi template,
        Expression<Func<TNewCombi, T1>> propertySelector1,
        IEnumerable<T1> propertyValues1,
        Expression<Func<TNewCombi, T2>> propertySelector2,
        IEnumerable<T2> propertyValues2,
        Expression<Func<TNewCombi, T3>> propertySelector3,
        IEnumerable<T3> propertyValues3)
    {
      var factory = CreateFactory<TNewCombi>(propertySelector1, propertySelector2, propertySelector3);
      var values =
          from propertyValue1 in propertyValues1.ToList()
          from propertyValue2 in propertyValues2.ToList()
          from propertyValue3 in propertyValues3.ToList()
          select factory(new object[] { propertyValue1, propertyValue2, propertyValue3 });

      return combine.WithCombinations(values.ToDictionary(GetText, x => x));
    }

    public static IArrangeOrAssert<TSubject, TResult, Dummy, TNewCombi> WithPermutations<TSubject, TResult, TNewCombi, T1, T2, T3, T4>
        (
        this ICombine<TSubject, TResult> combine,
        TNewCombi template,
        Expression<Func<TNewCombi, T1>> propertySelector1,
        IEnumerable<T1> propertyValues1,
        Expression<Func<TNewCombi, T2>> propertySelector2,
        IEnumerable<T2> propertyValues2,
        Expression<Func<TNewCombi, T3>> propertySelector3,
        IEnumerable<T3> propertyValues3,
        Expression<Func<TNewCombi, T4>> propertySelector4,
        IEnumerable<T4> propertyValues4)
    {
      var factory = CreateFactory<TNewCombi>(propertySelector1, propertySelector2, propertySelector3, propertySelector4);
      var values =
          from propertyValue1 in propertyValues1.ToList()
          from propertyValue2 in propertyValues2.ToList()
          from propertyValue3 in propertyValues3.ToList()
          from propertyValue4 in propertyValues4.ToList()
          select factory(new object[] { propertyValue1, propertyValue2, propertyValue3, propertyValue4 });

      return combine.WithCombinations(values.ToDictionary(GetText, x => x));
    }

    private static Func<IList<object>, TCombi> CreateFactory<TCombi> (params LambdaExpression[] memberExpressions)
    {
      var constructor = typeof (TCombi).GetConstructors(MemberBindings.Instance).Single();
      var parameters = constructor.GetParameters();
      var memberDictionary = memberExpressions
          .Select(x => ((MemberExpression) x.Body).Member.Name)
          .Select((x, i) => new { Name = x, Index = i })
          .ToDictionary(x => x.Name, x => x.Index);

      var argumentMapping = parameters.Select(x => memberDictionary[x.Name]).ToArray();
      return values => typeof (TCombi).CreateInstance<TCombi>(argumentMapping.Select(x => values[x]));
    }

    private static string GetText<TCombi> (TCombi combi)
    {
      var properties = typeof (TCombi).GetProperties();
      return string.Join(
          ", ",
          properties.Select(
              x => x.Name + " = " + (x.PropertyType == typeof (Type) || x.PropertyType.IsClass
                  ? x.PropertyType.Name
                  : x.GetValue(combi))));
    }
  }
}