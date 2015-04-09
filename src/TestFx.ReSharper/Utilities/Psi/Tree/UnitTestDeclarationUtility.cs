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
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.Model.Tree;
using TestFx.Utilities;

namespace TestFx.ReSharper.Utilities.Psi.Tree
{
  public interface IUnitTestDeclarationUtility
  {
    UnitTestElementLocation GetUnitTestElementLocation (IUnitTestDeclaration declaration);
  }

  public class UnitTestDeclarationUtility : IUnitTestDeclarationUtility
  {
    public static IUnitTestDeclarationUtility Instance = new UnitTestDeclarationUtility();

    public UnitTestElementLocation GetUnitTestElementLocation (IUnitTestDeclaration declaration)
    {
      if (declaration is IClassDeclaration)
        return GetUnitTestElementLocation((IClassDeclaration) declaration);
      if (declaration is IExpressionStatement)
        return GetUnitTestElementLocation((IExpressionStatement) declaration);
      if (declaration is IInvocationExpression)
        return GetUnitTestElementLocation((IInvocationExpression) declaration);

      throw new Exception();
    }

    private UnitTestElementLocation GetUnitTestElementLocation (IClassDeclaration declaration)
    {
      var navigationRange = declaration.GetNameDocumentRange().TextRange;
      var containingRange = declaration.GetDocumentRange().TextRange;
      var projectFile = declaration.GetContainingFile().AssertNotNull().GetSourceFile().ToProjectFile();
      return new UnitTestElementLocation(projectFile, navigationRange, containingRange);
    }

    private UnitTestElementLocation GetUnitTestElementLocation (IExpressionStatement statement)
    {
      var projectFile = statement.GetSourceFile().AssertNotNull().ToProjectFile();
      var textRange = statement.GetDocumentRange().TextRange;
      return new UnitTestElementLocation(projectFile, textRange, textRange);
    }

    private UnitTestElementLocation GetUnitTestElementLocation (IInvocationExpression invocation)
    {
      var projectFile = invocation.GetSourceFile().ToProjectFile();
      var reference = (invocation.InvokedExpression as IReferenceExpression)
          .AssertNotNull("invocationExpression.InvokedExpression is not a IReferenceExpression");

      var startOffset = reference.NameIdentifier.GetDocumentStartOffset();
      var endOffset = invocation.GetDocumentRange().EndOffsetRange();
      var textRange = startOffset.JoinRight(endOffset).TextRange;

      return new UnitTestElementLocation(projectFile, textRange, textRange);
    }
  }
}