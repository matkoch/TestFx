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
using System.Runtime.Serialization;
using System.Text;

namespace TestFx.Evaluation
{
  [Serializable]
  public class DiagnosticException : Exception
  {
    protected DiagnosticException (SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public DiagnosticException (string message, string detailMessage = null)
        : base(FormatMessage(message, detailMessage))
    {
    }

    private static string FormatMessage (string message, string detailMessage)
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.AppendFormat(string.IsNullOrEmpty(message) ? "<no message>" : message);

      if (!string.IsNullOrEmpty(detailMessage))
        stringBuilder.AppendFormat("({0}).", detailMessage);

      return stringBuilder.ToString();
    }
  }
}