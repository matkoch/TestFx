// Copyright 2014, 2013 Matthias Koch
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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;
#if R9
using JetBrains.Metadata.Reader.API;
#endif

namespace TestFx.ReSharper.Utilities.Psi.Modules
{
  public static class TypeElementExtensions
  {
    [CanBeNull]
    public static ITypeElement GetTypeElement (this IPsiServices psiServices, IClrTypeName clrTypeName)
    {
      return TypeElementUtility.Instance.GetTypeElement(psiServices, clrTypeName);
    }

    [CanBeNull]
    public static ITypeElement GetTypeElement (this IPsiModule psiModule, IClrTypeName clrTypeName)
    {
      return TypeElementUtility.Instance.GetTypeElement(psiModule, clrTypeName);
    }
  }
}