using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using TestFx.ReSharper.Model.Metadata.Wrapper;
using TestFx.Utilities;

namespace TestFx.ReSharper.Model.Metadata
{
  [DebuggerDisplay (Identifiable.DebuggerDisplay)]
  public class AssemblySuiteMetadata : MetadataAssemblyBase, ISuiteMetadata
  {
    private readonly IIdentity _identity;
    private readonly IProject _project;
    private readonly string _text;
    private readonly IEnumerable<ISuiteMetadata> _suiteMetadatas;

    public AssemblySuiteMetadata (
        IIdentity identity,
        IProject project,
        string text,
        IEnumerable<ISuiteMetadata> suiteMetadatas,
        IMetadataAssembly metadataAssembly)
        : base(metadataAssembly)
    {
      _identity = identity;
      _project = project;
      _text = text;
      _suiteMetadatas = suiteMetadatas;
    }

    public IIdentity Identity
    {
      get { return _identity; }
    }

    public IProject Project
    {
      get { return _project; }
    }

    public string Text
    {
      get { return _text; }
    }

    public IEnumerable<ISuiteMetadata> SuiteMetadatas
    {
      get { return _suiteMetadatas; }
    }

    public IEnumerable<ISuiteEntity> SuiteEntities
    {
      get { return _suiteMetadatas.Cast<ISuiteEntity>(); }
    }

    public IEnumerable<ITestEntity> TestEntities
    {
      get { yield break; }
    }
  }
}