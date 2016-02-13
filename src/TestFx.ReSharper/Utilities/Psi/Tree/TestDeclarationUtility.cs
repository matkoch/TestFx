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
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using TestFx.ReSharper.Model.Tree;
using TestFx.Utilities;

namespace TestFx.ReSharper.Utilities.Psi.Tree
{
  public interface ITestDeclarationUtility
  {
    Ranges GetRanges (ITestDeclaration declaration);
  }

  internal class TestDeclarationUtility : ITestDeclarationUtility
  {
    public static ITestDeclarationUtility Instance = new TestDeclarationUtility();

    public Ranges GetRanges(ITestDeclaration declaration)
    {
      if (declaration is IClassDeclaration)
        return GetUnitTestElementLocation((IClassDeclaration) declaration);
      if (declaration is IExpressionStatement)
        return GetUnitTestElementLocation((IExpressionStatement) declaration);
      if (declaration is IInvocationExpression)
        return GetUnitTestElementLocation((IInvocationExpression) declaration);

      throw new Exception();
    }

    private Ranges GetUnitTestElementLocation(IClassDeclaration declaration)
    {
      var navigationRange = declaration.GetNameDocumentRange();
      var containingRange = declaration.GetDocumentRange();
      return new Ranges(navigationRange, containingRange);
    }

    private Ranges GetUnitTestElementLocation(IExpressionStatement statement)
    {
      var textRange = statement.GetDocumentRange();
      return new Ranges(textRange, textRange);
    }

    private Ranges GetUnitTestElementLocation(IInvocationExpression invocation)
    {
      var reference = (invocation.InvokedExpression as IReferenceExpression)
          .NotNull("invocationExpression.InvokedExpression is not a IReferenceExpression");

      var startOffset = reference.NameIdentifier.GetDocumentStartOffset();
      var endOffset = invocation.GetDocumentRange().EndOffsetRange();
      var textRange = startOffset.JoinRight(endOffset);

      return new Ranges(textRange, textRange);
    }
  }
}