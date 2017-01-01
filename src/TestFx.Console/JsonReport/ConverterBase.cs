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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TestFx.Console.JsonReport
{
  public abstract class ConverterBase<T> : JsonConverter
  {
    public abstract void WriteJson (T value, JsonWriter writer, JsonSerializer serializer);

    public override bool CanConvert ([NotNull] Type objectType)
    {
      return typeof(T).IsAssignableFrom(objectType);
    }

    public override void WriteJson ([NotNull] JsonWriter writer, [NotNull] object value, [NotNull] JsonSerializer serializer)
    {
      writer.WriteStartObject ();
      WriteJson ((T) value, writer, serializer);
      writer.WriteEndObject ();
    }

    public override object ReadJson (
      [NotNull] JsonReader reader,
      [NotNull] Type objectType,
      [NotNull] object existingValue,
      [NotNull] JsonSerializer serializer)
    {
      throw new NotSupportedException();
    }

    protected void Write (string propertyName, [CanBeNull] object value, JsonWriter writer, JsonSerializer serializer)
    {
      if (value == null)
        return;

      writer.WritePropertyName(propertyName);
      serializer.Serialize(writer, value);
    }

    protected void Write (string propertyName, string value, JsonWriter writer, JsonSerializer serializer)
    {
      Write(propertyName, (object) WebUtility.HtmlEncode(value), writer, serializer);
    }

    protected void Write (string propertyName, Enum value, JsonWriter writer, JsonSerializer serializer)
    {
      Write(propertyName, value.ToString(), writer, serializer);
    }

    protected void Write<TItem> (string propertyName, IEnumerable<TItem> collection, JsonWriter writer, JsonSerializer serializer)
    {
      var list = collection.ToList();
      if (list.Count == 0)
        return;

      writer.WritePropertyName(propertyName);
      writer.WriteStartArray();
      list.ForEach(x => serializer.Serialize(writer, x));
      writer.WriteEndArray();
    }
  }
}