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
using System.Xml;
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner.Tasks
{
  [Serializable]
  public class DynamicTask : Task
  {
    private const string c_parentGuid = "parentAbsoluteId";
    private const string c_text = "text";

    [UsedImplicitly]
    public DynamicTask (XmlElement element)
        : base(element)
    {
      ParentGuid = GetXmlAttribute(element, c_parentGuid);
      Text = GetXmlAttribute(element, c_text);
    }

    public DynamicTask (string parentGuid, IIdentity identity, [CanBeNull] string text)
        : base(identity)
    {
      ParentGuid = parentGuid;
      Text = text;
    }

    public override void SaveXml (XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, c_parentGuid, ParentGuid);
      SetXmlAttribute(element, c_text, Text);
    }

    public override bool IsMeaningfulTask => true;

    public string ParentGuid { get; }

    public string Text { get; }
  }
}