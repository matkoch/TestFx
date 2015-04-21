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
using System.Collections.Generic;
using JetBrains.Metadata.Reader.API;

namespace TestFx.ReSharper.Model.Utilities
{
  public static class MetadataEntityCollection
  {
    public static IEnumerable<TDestination> Create<TSource, TDestination> (
        IEnumerable<TSource> sources,
        Func<TSource, TDestination> converter,
        Func<bool> notInterrupted)
        where TSource : IMetadataEntity
        where TDestination : class, IMetadataEntity
    {
      return new MetadataEntityCollection<TSource, TDestination>(sources, converter, notInterrupted);
    }
  }

  public class MetadataEntityCollection<TSource, TDestination> : BufferingNodeCollection<TSource, TDestination>
      where TSource : IMetadataEntity
      where TDestination : class, IMetadataEntity
  {
    public MetadataEntityCollection (IEnumerable<TSource> sources, Func<TSource, TDestination> convert, Func<bool> notInterrupted)
        : base(sources, convert, notInterrupted)
    {
    }

    protected override bool IsInvalid (TDestination item)
    {
      return false;
    }
  }
}