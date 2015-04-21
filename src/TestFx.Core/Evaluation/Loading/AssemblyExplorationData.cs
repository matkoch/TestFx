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
  public interface IAssemblyExplorationData
  {
    IDictionary<Type, ITypeLoader> TypeLoaders { get; }
    IEnumerable<Type> SuiteTypes { get; }
    IEnumerable<IAssemblySetup> AssemblySetups { get; }
  }

  public class AssemblyExplorationData : IAssemblyExplorationData
  {
    private readonly IDictionary<Type, ITypeLoader> _typeLoaders;
    private readonly IEnumerable<Type> _suiteTypes;
    private readonly IEnumerable<IAssemblySetup> _assemblySetups;

    public AssemblyExplorationData (
        IDictionary<Type, ITypeLoader> typeLoaders,
        IEnumerable<Type> suiteTypes,
        IEnumerable<IAssemblySetup> assemblySetups)
    {
      _typeLoaders = typeLoaders;
      _suiteTypes = suiteTypes;
      _assemblySetups = assemblySetups;
    }

    public IDictionary<Type, ITypeLoader> TypeLoaders
    {
      get { return _typeLoaders; }
    }

    public IEnumerable<Type> SuiteTypes
    {
      get { return _suiteTypes; }
    }

    public IEnumerable<IAssemblySetup> AssemblySetups
    {
      get { return _assemblySetups; }
    }
  }
}