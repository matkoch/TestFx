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
using TestFx.Evaluation.Results;

namespace TestFx.Evaluation.Reporting
{
  public static class ResultExtensions
  {
    private static readonly Dictionary<State, string> s_stateSymbols;
    private static readonly Dictionary<OutputType, string> s_outputTypeSymbols;

    static ResultExtensions ()
    {
      s_stateSymbols =
          new Dictionary<State, string>
          {
              { State.Passed, ":white_check_mark:" },
              { State.Failed, ":no_entry:" },
              { State.Inconclusive, ":children_crossing:" }
          };

      s_outputTypeSymbols =
          new Dictionary<OutputType, string>
          {
              { OutputType.Standard, ":small_blue_diamond:" },
              { OutputType.Debug, ":small_orange_diamond:" },
              { OutputType.Error, ":small_red_triangle:" }
          };
    }

    public static string GetSymbol (this State state)
    {
      return s_stateSymbols[state];
    }

    public static string GetSymbol (this OutputType outputType)
    {
      return s_outputTypeSymbols[outputType];
    }
  }
}