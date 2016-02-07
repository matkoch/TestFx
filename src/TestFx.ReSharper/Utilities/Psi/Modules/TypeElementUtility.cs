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
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Modules;

namespace TestFx.ReSharper.Utilities.Psi.Modules
{
  public interface ITypeElementUtility
  {
    [CanBeNull]
    ITypeElement GetTypeElement (IPsiServices psiServices, IClrTypeName clrTypeName);

    [CanBeNull]
    ITypeElement GetTypeElement (IPsiModule psiModule, IClrTypeName clrTypeName);
  }

  public class TypeElementUtility : ITypeElementUtility
  {
    public static ITypeElementUtility Instance = new TypeElementUtility();

    [CanBeNull]
    public ITypeElement GetTypeElement (IPsiServices psiServices, IClrTypeName clrTypeName)
    {
      var symbolCache = psiServices.Symbols;
      var symbolScope = symbolCache.GetSymbolScope(LibrarySymbolScope.FULL, caseSensitive: false);
      return symbolScope.GetTypeElementByCLRName(clrTypeName);
    }

    [CanBeNull]
    public ITypeElement GetTypeElement (IPsiModule psiModule, IClrTypeName clrTypeName)
    {
      var symbolCache = psiModule.GetPsiServices().Symbols;
      var symbolScope = symbolCache.GetSymbolScope(psiModule, withReferences: true, caseSensitive: true);
      return symbolScope.GetTypeElementByCLRName(clrTypeName);
    }
  }
}