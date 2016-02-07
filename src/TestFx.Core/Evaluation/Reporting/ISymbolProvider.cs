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
using TestFx.Evaluation.Results;

namespace TestFx.Evaluation.Reporting
{
  /// <summary>
  ///   Returns symbols for <see cref="State" />s and <see cref="OutputType" />s.
  /// </summary>
  public interface ISymbolProvider
  {
    string GetSymbol (State state);
    string GetSymbol (OutputType outputType);
  }
}