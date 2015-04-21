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
using TestFx.Utilities;

namespace TestFx.Evaluation.Results
{
  // TODO: only interface? why should result be base type?
  public interface IOutputResult : IResult
  {
    IEnumerable<OutputEntry> OutputEntries { get; }
  }

  [Serializable]
  public abstract class OutputResult : Result, IOutputResult
  {
    private readonly ICollection<OutputEntry> _outputEntries;

    protected OutputResult (IIdentity identity, string text, State state, ICollection<OutputEntry> outputEntries)
        : base(identity, text, state)
    {
      _outputEntries = outputEntries;
    }

    public IEnumerable<OutputEntry> OutputEntries
    {
      get { return _outputEntries; }
    }
  }
}