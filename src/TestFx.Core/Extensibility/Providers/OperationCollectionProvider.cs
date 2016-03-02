﻿// Copyright 2016, 2015, 2014 Matthias Koch
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
using System.Linq;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface IOperationCollectionProvider : IProvider
  {
    IEnumerable<IOperationProvider> OperationProviders { get; }
  }

  public class OperationCollectionProvider : Provider, IOperationCollectionProvider
  {
    private IEnumerable<IOperationProvider> _operationProviders;

    protected OperationCollectionProvider (IIdentity identity, string text, [CanBeNull] string ignoreReason)
        : base(identity, text, ignoreReason)
    {
      _operationProviders = new IOperationProvider[0];
    }

    public IEnumerable<IOperationProvider> OperationProviders
    {
      get { return _operationProviders; }
      set { _operationProviders = value.ToList(); }
    }
  }
}