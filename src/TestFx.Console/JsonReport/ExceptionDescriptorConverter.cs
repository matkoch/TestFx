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
using System.Linq;
using Newtonsoft.Json;
using TestFx.Evaluation.Results;

namespace TestFx.Console.JsonReport
{
  public class ExceptionDescriptorConverter : ConverterBase<IExceptionDescriptor>
  {
    public override void WriteJson (IExceptionDescriptor value, JsonWriter writer, JsonSerializer serializer)
    {
      Write("name", value.Name, writer, serializer);
      Write("message", value.Message, writer, serializer);
      Write("fullname", value.FullName, writer, serializer);
      Write("stacktrace", value.StackTrace, writer, serializer);
    }
  }
}