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

namespace TestFx.Evaluation.Loading
{
  public interface IAssemblyExplorationData
  {
    IDictionary<Type, TypeLoaderFactory> TypeLoaderFactories { get; }
    IEnumerable<Type> SuiteTypes { get; }
    IEnumerable<Type> AssemblySetupTypes { get; }
  }

  internal class AssemblyExplorationData : IAssemblyExplorationData
  {
    private readonly IDictionary<Type, TypeLoaderFactory> _typeLoaderFactories;
    private readonly IEnumerable<Type> _suiteTypes;
    private readonly IEnumerable<Type> _assemblySetupTypes;

    public AssemblyExplorationData (
        IDictionary<Type, TypeLoaderFactory> typeLoaderFactories,
        IEnumerable<Type> suiteTypes,
        IEnumerable<Type> assemblySetupTypes)
    {
      _typeLoaderFactories = typeLoaderFactories;
      _suiteTypes = suiteTypes;
      _assemblySetupTypes = assemblySetupTypes;
    }

    public IDictionary<Type, TypeLoaderFactory> TypeLoaderFactories
    {
      get { return _typeLoaderFactories; }
    }

    public IEnumerable<Type> SuiteTypes
    {
      get { return _suiteTypes; }
    }

    public IEnumerable<Type> AssemblySetupTypes
    {
      get { return _assemblySetupTypes; }
    }
  }
}