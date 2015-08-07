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

namespace TestFx.Evaluation.Loading
{
  public class AssemblyExplorationData
  {
    private readonly IDictionary<Type, ITypeLoader> _typeLoaders;
    private readonly IEnumerable<Type> _suiteTypes;
    private readonly IEnumerable<Type> _bootstrapTypes;

    public AssemblyExplorationData (
        IDictionary<Type, ITypeLoader> typeLoaders,
        IEnumerable<Type> suiteTypes,
        IEnumerable<Type> bootstrapTypes)
    {
      _typeLoaders = typeLoaders;
      _suiteTypes = suiteTypes;
      _bootstrapTypes = bootstrapTypes;
    }

    public IDictionary<Type, ITypeLoader> TypeLoaders
    {
      get { return _typeLoaders; }
    }

    public IEnumerable<Type> SuiteTypes
    {
      get { return _suiteTypes; }
    }

    public IEnumerable<Type> BootstrapTypes
    {
      get { return _bootstrapTypes; }
    }
  }
}