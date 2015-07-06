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
using System.IO;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;

namespace TestFx.ReSharper.Utilities.Psi.Caches
{
  public abstract class SimpleCacheBase<TCacheData> : ICache
      where TCacheData : class
  {
    private readonly ISolution _solution;
    private readonly int _cacheFormatVersion;
    private readonly string _cacheName;
    private readonly IShellLocks _shellLocks;
    private readonly IPsiConfiguration _psiConfiguration;
    private readonly IPersistentIndexManager _persistentIndexManager;
    private readonly object _lockObject = new object();
    private readonly JetHashSet<IPsiSourceFile> _dirtyFiles = new JetHashSet<IPsiSourceFile>();
    private SimplePersistentCache<TCacheData> _persistentCache;

    protected SimpleCacheBase (
        ISolution solution,
        IShellLocks shellLocks,
        IPsiConfiguration psiConfiguration,
        IPersistentIndexManager persistentIndexManager,
        int cacheFormatVersion,
        string cacheName)
    {
      _solution = solution;
      _cacheFormatVersion = cacheFormatVersion;
      _cacheName = cacheName;
      _shellLocks = shellLocks;
      _psiConfiguration = psiConfiguration;
      _persistentIndexManager = persistentIndexManager;
    }

    public void MarkAsDirty (IPsiSourceFile sourceFile)
    {
      lock (_lockObject)
        _dirtyFiles.Add(sourceFile);
    }

    public object Load (IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence || _cacheFormatVersion < 0)
        return null;

      Assertion.Assert(_persistentCache == null, "persistentCache == null");
      _persistentCache = new SimplePersistentCache<TCacheData>(
          _shellLocks,
          _cacheFormatVersion,
          _cacheName,
          _psiConfiguration);

      if (_persistentCache.Load(progress, _persistentIndexManager, ReadData, Merge) != LoadResult.OK)
      {
        lock (_lockObject)
        {
          ClearCache();
          return null;
        }
      }

      // Any returned value will be passed to MergeLoaded, but we've already
      // merged as part of the load above
      return null;
    }

    public void MergeLoaded (object data)
    {
      // Can be called to merge a data item returned from Load. We don't return
      // anything, but handle merging as part of loading. So this shouldn't
      // get called. Instead, we mark the file as dirty for ALL caches
      // TODO: I don't know why we do this (copied from ConditionalNamesCache)
      foreach (var sourceFile in _dirtyFiles)
        _solution.GetPsiServices().Caches.MarkAsDirty(sourceFile);
    }

    public void Save (IProgressIndicator progress, bool enablePersistence)
    {
      if (!enablePersistence || _cacheFormatVersion < 0)
        return;

      Assertion.Assert(_persistentCache != null, "persistentCache != null");
      lock (_lockObject)
      {
        _persistentCache.Save(progress, _persistentIndexManager, WriteData);
      }

      // Apparently, Save also implies throws away
      _persistentCache = null;
    }

    public bool UpToDate (IPsiSourceFile sourceFile)
    {
      if (!Accepts(sourceFile))
        return true;

      lock (_lockObject)
        return !_dirtyFiles.Contains(sourceFile) && IsCached(sourceFile);
    }

    public object Build (IPsiSourceFile sourceFile, bool isStartup)
    {
      return BuildData(sourceFile);
    }

    public void Merge (IPsiSourceFile sourceFile, object builtPart)
    {
      var data = (TCacheData) builtPart;
      lock (_lockObject)
      {
        AddFileToCache(sourceFile, data);
        if (_persistentCache != null)
          _persistentCache.AddDataToSave(sourceFile, data);
        _dirtyFiles.Remove(sourceFile);
      }
    }

    public void Drop (IPsiSourceFile sourceFile)
    {
      lock (_lockObject)
      {
        RemoveFileFromCache(sourceFile);
        if (_persistentCache != null)
          _persistentCache.MarkDataToDelete(sourceFile);
      }
    }

    public void OnPsiChange (ITreeNode elementContainingChanges, PsiChangedElementType type)
    {
      if (elementContainingChanges != null)
      {
        var sourceFile = elementContainingChanges.GetSourceFile();
        if (sourceFile != null)
          MarkAsDirty(sourceFile);
      }
    }

    public void OnDocumentChange (IPsiSourceFile sourceFile, ProjectFileDocumentCopyChange change)
    {
      MarkAsDirty(sourceFile);
    }

    public void SyncUpdate (bool underTransaction)
    {
      if (underTransaction)
        return;

      lock (_lockObject)
      {
        foreach (var sourceFile in _dirtyFiles.ToList())
        {
          if (sourceFile.IsValid())
            Merge(sourceFile, BuildData(sourceFile));
        }
        _dirtyFiles.Clear();
      }
    }

    public bool HasDirtyFiles
    {
      get
      {
        lock (_lockObject)
          return !_dirtyFiles.IsEmpty();
      }
    }

    protected virtual bool Accepts (IPsiSourceFile sourceFile)
    {
      using (ReadLockCookie.Create())
      {
        var projectFile = sourceFile.ToProjectFile();
        if (projectFile == null)
          return false;

        return sourceFile.Properties.ShouldBuildPsi;
      }
    }

    protected abstract TCacheData BuildData (IPsiSourceFile sourceFile);
    protected abstract TCacheData ReadData (IPsiSourceFile sourceFile, BinaryReader reader);
    protected abstract void WriteData (BinaryWriter writer, IPsiSourceFile sourceFile, TCacheData data);
    protected abstract void AddFileToCache (IPsiSourceFile sourceFile, TCacheData data);
    protected abstract void RemoveFileFromCache (IPsiSourceFile sourceFile);
    protected abstract bool IsCached (IPsiSourceFile sourceFile);
    protected abstract void ClearCache ();
  }
}