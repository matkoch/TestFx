﻿// Copyright 2014, 2013 Matthias Koch
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
using System.Globalization;
using System.Xml;
using TestFx.Utilities;

namespace TestFx.ReSharper.Runner.Tasks
{
  [Serializable]
  public class RunTask : Task
  {
    private const string c_visualStudioProcessId = "visualStudioProcessId";

    private readonly int _visualStudioProcessId;

    public RunTask (XmlElement element)
        : base(element)
    {
      _visualStudioProcessId = int.Parse(GetXmlAttribute(element, c_visualStudioProcessId));
    }

    public RunTask ()
        : base(new Identity("runTask"))
    {
      _visualStudioProcessId = Process.GetCurrentProcess().Id;
    }

    public override void SaveXml (XmlElement element)
    {
      base.SaveXml(element);
      SetXmlAttribute(element, c_visualStudioProcessId, _visualStudioProcessId.ToString(CultureInfo.InvariantCulture));
    }

    public override bool IsMeaningfulTask
    {
      get { return false; }
    }

    public int VisualStudioProcessId
    {
      get { return _visualStudioProcessId; }
    }
  }
}