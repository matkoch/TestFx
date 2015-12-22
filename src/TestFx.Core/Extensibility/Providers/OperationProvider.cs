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
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface IOperationProvider : IProvider
  {
    Type Descriptor { get; }
    OperationType Type { get; }
    Action Action { get; }

    [CanBeNull]
    IOperationProvider CleanupProvider { get; }
  }

  public class OperationProvider : Provider, IOperationProvider
  {
    public static readonly Action NotImplemented = () => { };
    private static readonly IIdentity s_identity = new Identity("<OPERATION>");

    public static OperationProvider Create<T> (OperationType type, string text, Action action, IOperationProvider cleanupProvider = null)
        where T : IOperationDescriptor
    {
      return new OperationProvider(text, typeof (T), type, action, cleanupProvider);
    }

    private readonly Type _descriptor;
    private readonly OperationType _type;
    private readonly Action _action;
    private readonly IOperationProvider _cleanupProvider;

    private OperationProvider (
        string text,
        Type descriptor,
        OperationType type,
        Action action,
        [CanBeNull] IOperationProvider cleanupProvider)
        : base(s_identity, text, ignored: false)
    {
      _descriptor = descriptor;
      _type = type;
      _action = action;
      _cleanupProvider = cleanupProvider;
    }

    public Type Descriptor
    {
      get { return _descriptor; }
    }

    public OperationType Type
    {
      get { return _type; }
    }

    public Action Action
    {
      get { return _action; }
    }

    [CanBeNull]
    public IOperationProvider CleanupProvider
    {
      get { return _cleanupProvider; }
    }
  }
}