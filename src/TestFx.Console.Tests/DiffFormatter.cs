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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace TestFx.Console.Tests
{
  /// <summary>
  /// Formats a diff of two strings using https://github.com/mmanela/diffplex. Used to format the error output when tests fail.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public static class DiffFormatter
  {
    public static string GetFormattedDiff (string oldText, string newText)
    {
      var differ = new Differ();
      var diff = GetDiff(differ, oldText, newText);

      return diff.Lines.Any(x => x.Type != ChangeType.Unchanged)
          ? FormatDiff(diff)
          : string.Empty;
    }

    private static DiffPaneModel GetDiff (IDiffer differ, string oldText, string newText)
    {
      var inlineBuilder = new InlineDiffBuilder(differ);
      return inlineBuilder.BuildDiffModel(oldText, newText);
    }

    private static string FormatDiff (DiffPaneModel diff)
    {
      var sb = new StringBuilder();
      foreach (var line in diff.Lines)
        AppendLine(sb, line);

      return sb.ToString();
    }

    private static void AppendLine (StringBuilder sb, DiffPiece line)
    {
      switch (line.Type)
      {
        case ChangeType.Inserted:
          sb.Append("+ ");
          break;
        case ChangeType.Deleted:
          sb.Append("- ");
          break;
        default:
          sb.Append("  ");
          break;
      }

      sb.AppendLine(line.Text);
    }
  }
}