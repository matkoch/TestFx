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
using System.Diagnostics;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner.Tasks
{
  [Serializable]
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public abstract class Task : RemoteTask, IEquatable<Task>, IIdentifiable
  {
    private const string c_absoluteIdField = "absoluteId";

    private readonly string _absoluteId;

    protected Task (XmlElement element)
        : base(element)
    {
      _absoluteId = GetXmlAttribute(element, c_absoluteIdField);
    }

    protected Task (IIdentity identity)
        : this(identity.Absolute)
    {
    }

    protected Task (string absoluteId)
        : base(RecursiveRemoteTaskRunner.ID)
    {
      _absoluteId = absoluteId;
    }

    public override void SaveXml (XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, c_absoluteIdField, _absoluteId);
    }

    public IIdentity Identity
    {
      get { return Utilities.Identity.Parse(_absoluteId); }
    }

    public bool Equals (Task other)
    {
      return Equals(RunnerID, other.RunnerID) &&
             Equals(Identity, other.Identity);
    }

    public override bool Equals (RemoteTask other)
    {
      return Equals(other as Task);
    }

    public override bool Equals (object other)
    {
      return Equals(other as Task);
    }

    public override int GetHashCode ()
    {
      return Identity.Absolute.GetHashCode();
    }
  }
}