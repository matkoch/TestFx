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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestFx.Evaluation.Results;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Reporting
{
  public static class ReportingUtility
  {
    public static IEnumerable<IExceptionDescriptor> GetExceptions (this IOutputResult result)
    {
      return result.GetOperationResults().Select(x => x.Exception).WhereNotNull();
    }

    public static IEnumerable<IOperationResult> GetOperationResults (this IOutputResult result)
    {
      return result is ITestResult
          ? ((ITestResult) result).OperationResults
          : ((ISuiteResult) result).SetupResults.Concat(new FillingOperationResult()).Concat(((ISuiteResult) result).CleanupResults);
    }

    public static string GetBriefSummary (this IOutputResult result)
    {
      var operationsCount = result.GetOperationResults().Count(x => !(x is FillingOperationResult));
      var exceptions = result.GetExceptions().ToList();
      return exceptions.Count == 0
          ? $"{operationsCount} {(operationsCount > 1 ? "operations" : "operation")} run"
          : exceptions.Count == 1
              ? exceptions.Single().Name
              : $"exceptions.Count {(exceptions.Count > 1 ? "exceptions" : "exception")} thrown";
    }

    public static string GetDetailedSummary (this IOutputResult result, ISymbolProvider symbolProvider = null, bool includeExceptions = true)
    {
      symbolProvider = symbolProvider ?? new DefaultSymbolProvider();
      var builder = new StringBuilder();

      AppendOperations(result.GetOperationResults(), builder, symbolProvider);
      AppendOutput(result.OutputEntries, builder, symbolProvider);
      if (includeExceptions)
        AppendExceptions(result.GetExceptions(), builder);

      return builder.ToString();
    }

    private static void AppendOperations (IEnumerable<IOperationResult> results, StringBuilder builder, ISymbolProvider symbolProvider)
    {
      builder.AppendLine("Operations:");
      foreach (var result in results)
      {
        if (result is FillingOperationResult)
        {
          builder.AppendLine(".. InnerOperations ..");
          continue;
        }

        builder.Append($"{symbolProvider.GetSymbol(result.State)} {result.Text}");

        if (result.Exception != null)
          builder.Append($" ({result.Exception.Name})");

        builder.Append("\r\n");
      }
    }

    private static void AppendOutput (IEnumerable<OutputEntry> entries, StringBuilder builder, ISymbolProvider symbolProvider)
    {
      var entriesList = entries.ToList();
      if (entriesList.Count != 0)
      {
        builder.AppendLine().AppendLine("Output:");
        entriesList.ForEach(x => builder.Append($"{symbolProvider.GetSymbol(x.Type)} {x.Message}\r\n"));
      }
    }

    private static void AppendExceptions (IEnumerable<IExceptionDescriptor> exceptions, StringBuilder builder)
    {
      foreach (var exception in exceptions)
      {
        builder.AppendLine().AppendLine();

        builder.Append($"{exception.FullName}:").AppendLine();
        builder.Append(exception.Message).AppendLine();
        builder.Append(exception.StackTrace);
      }
    }
  }
}