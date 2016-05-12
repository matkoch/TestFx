// Copyright 2016, 2015, 2014 Matthias Koch
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
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.Extensibility;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Explorers.Tree
{
  public interface ITreePresenter
  {
    [CanBeNull]
    string Present (IClassDeclaration classDeclaration, string suiteAttributeType);

    [CanBeNull]
    string Present (IInvocationExpression invocationExpression);
  }

  [PsiComponent]
  internal class TreePresenter : ITreePresenter
  {
    private readonly IIntrospectionPresenter _introspectionPresenter;

    public TreePresenter ()
    {
      _introspectionPresenter = new IntrospectionPresenter();
    }

    [CanBeNull]
    public string Present (IClassDeclaration classDeclaration, string suiteAttributeType)
    {
      var clazz = (IClass) classDeclaration.DeclaredElement;
      var subjectAttributeData = clazz?.GetAttributeData(suiteAttributeType);
      if (subjectAttributeData == null)
        return null;

      var subjectAttribute = subjectAttributeData.ToCommon();

      var subjectAttributeConstructor = subjectAttributeData.Constructor.NotNull();
      var displayFormatAttribute = subjectAttributeConstructor.GetAttributeData<DisplayFormatAttribute>().NotNull().ToCommon();
      
      return _introspectionPresenter.Present(displayFormatAttribute, clazz.ToCommon(), subjectAttribute);
    }

    [CanBeNull]
    public string Present (IInvocationExpression invocationExpression)
    {
      var method = invocationExpression.Reference.NotNull().GetResolved<IMethod>();
      var displayFormatAttributeData = method?.GetAttributeData<DisplayFormatAttribute>();
      if (displayFormatAttributeData == null)
        return null;

      var displayFormatAttribute = displayFormatAttributeData.ToCommon();
      var commonExpressions = invocationExpression.Arguments
          .Select((x, i) => new { Index = i, Object = GetConstantValue(x) })
          .ToDictionary(x => x.Index.ToString(), x => x.Object);
      return _introspectionPresenter.Present(displayFormatAttribute, commonExpressions);
    }

    private object GetConstantValue (ICSharpArgument argument)
    {
      if (argument.Kind == ParameterKind.UNKNOWN)
        return IntrospectionPresenter.UnknownValue;

      var literalExpression = argument.Value as ILiteralExpression;
      if (literalExpression == null)
        return IntrospectionPresenter.UnknownValue;

      var constantValue = literalExpression.ConstantValue.Value.NotNull();
      return constantValue.ToString().Trim('"');
    }
  }
}