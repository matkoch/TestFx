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
    private static Dictionary<ChangeType, char> s_changeSymbol =
        new Dictionary<ChangeType, char>
        {
          { ChangeType.Inserted, '+' },
          { ChangeType.Deleted, '-' }
        };

    public static string GetFormattedDiff (string oldText, string newText)
    {
      var differ = new Differ();
      var diff = GetDiff(differ, oldText, newText);
      var changedLines = diff.Lines.Where(x => x.Type != ChangeType.Unchanged).ToList();

      var stringBuilder = new StringBuilder();
      foreach (var line in changedLines)
      {
        stringBuilder.Append(s_changeSymbol[line.Type])
            .Append(" ")
            .Append(line.Text)
            .AppendLine();
      }

      return stringBuilder.ToString();
    }

    private static DiffPaneModel GetDiff (IDiffer differ, string oldText, string newText)
    {
      var inlineBuilder = new InlineDiffBuilder(differ);
      return inlineBuilder.BuildDiffModel(oldText, newText);
    }
  }
}