using System;
using System.Collections.Generic;

namespace TestFx.ReSharper.Model.Metadata
{
  public interface ITestMetadataHolder
  {
    IEnumerable<ITestMetadata> TestMetadatas { get; }  
  }
}