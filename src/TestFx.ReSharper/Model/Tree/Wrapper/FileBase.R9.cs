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
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace TestFx.ReSharper.Model.Tree.Wrapper
{
  public abstract partial class FileBase
  {
    public IExternAliasDirective AddExternAliasAfter (IExternAliasDirective externAlias, IExternAliasDirective anchor)
    {
      return _file.AddExternAliasAfter(externAlias, anchor);
    }

    public IExternAliasDirective AddExternAliasBefore (IExternAliasDirective externAlias, IExternAliasDirective anchor)
    {
      return _file.AddExternAliasBefore(externAlias, anchor);
    }

    public void RemoveExternAlias (IExternAliasDirective externAlias)
    {
      _file.RemoveExternAlias(externAlias);
    }
  }
}