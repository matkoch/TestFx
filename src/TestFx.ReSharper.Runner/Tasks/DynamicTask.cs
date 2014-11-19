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
using System.Xml;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner.Tasks
{
  [Serializable]
  public abstract class DynamicTask : Task
  {
    private const string c_parentAbsoluteId = "parentAbsoluteId";
    private const string c_text = "text";

    private readonly string _parentAbsoluteId;
    private readonly string _text;

    protected DynamicTask (XmlElement element)
        : base(element)
    {
      _parentAbsoluteId = GetXmlAttribute(element, c_parentAbsoluteId);
      _text = GetXmlAttribute(element, c_text);
    }

    protected DynamicTask (IIdentity identity, [CanBeNull] string text)
        : base(identity)
    {
      _parentAbsoluteId = identity.Parent != null ? identity.Parent.Absolute : null;
      _text = text;
    }

    public override void SaveXml (XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, c_parentAbsoluteId, _parentAbsoluteId);
      SetXmlAttribute(element, c_text, _text);
    }

    public override bool IsMeaningfulTask
    {
      get { return true; }
    }

    public string ParentAbsoluteId
    {
      get { return _parentAbsoluteId; }
    }

    public string Text
    {
      get { return _text; }
    }
  }
}