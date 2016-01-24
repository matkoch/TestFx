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
using System.Xml;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner.Tasks
{
  [Serializable]
  public class DynamicTask : Task
  {
    private const string c_elementTypeFullName = "elementTypeFullName";
    private const string c_parentGuid = "parentAbsoluteId";
    private const string c_text = "text";

    private readonly string _taskTypeFullName;
    private readonly string _parentGuid;
    private readonly string _text;

    [UsedImplicitly]
    public DynamicTask (XmlElement element)
        : base(element)
    {
      _taskTypeFullName = GetXmlAttribute(element, c_elementTypeFullName);
      _parentGuid = GetXmlAttribute(element, c_parentGuid);
      _text = GetXmlAttribute(element, c_text);
    }

    public DynamicTask (Type taskType, string parentGuid, IIdentity identity, [CanBeNull] string text)
        : base(identity)
    {
      _taskTypeFullName = taskType.FullName;
      _parentGuid = parentGuid;
      _text = text;
    }

    public override void SaveXml (XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, c_elementTypeFullName, _taskTypeFullName);
      SetXmlAttribute(element, c_parentGuid, _parentGuid);
      SetXmlAttribute(element, c_text, _text);
    }

    public override bool IsMeaningfulTask
    {
      get { return true; }
    }

    public string TaskTypeFullName
    {
      get { return _taskTypeFullName; }
    }

    public string ParentGuid
    {
      get { return _parentGuid; }
    }

    public string Text
    {
      get { return _text; }
    }
  }
}