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
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using TestFx.Extensibility;
using TestFx.ReSharper.Utilities.Metadata;

namespace TestFx.ReSharper.Model.Metadata.Aggregation
{
  public interface IMetadataPresenter
  {
    [CanBeNull]
    string Present (IMetadataTypeInfo type);
  }

  public class MetadataPresenter : IMetadataPresenter
  {
    private readonly IntrospectionPresenter _introspectionPresenter;

    public MetadataPresenter ()
    {
      _introspectionPresenter = new IntrospectionPresenter();
    }

    [CanBeNull]
    public string Present (IMetadataTypeInfo type)
    {
      var subjectAttributeData = type.GetAttributeData<SubjectAttributeBase>();
      if (subjectAttributeData == null)
        return null;

      var subjectAttribute = subjectAttributeData.ToCommon();
      var displayFormatAttribute = subjectAttributeData.UsedConstructor.GetAttributeData<DisplayFormatAttribute>().ToCommon();
      return _introspectionPresenter.Present(displayFormatAttribute, subjectAttribute);
    }
  }
}