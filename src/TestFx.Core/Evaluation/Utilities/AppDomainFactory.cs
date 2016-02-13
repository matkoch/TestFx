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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using JetBrains.Annotations;

namespace TestFx.Evaluation.Utilities
{
  public interface IAppDomainFactory
  {
    IAppDomain Create (
        Assembly baseAssembly,
        string shadowCopyPath = null,
        IEnumerable<IPermission> permissions = null,
        IEnumerable<Assembly> fullTrustAssemblies = null);
  }

  internal class AppDomainFactory : IAppDomainFactory
  {
    public IAppDomain Create (
        Assembly baseAssembly,
        string shadowCopyPath = null,
        IEnumerable<IPermission> permissions = null,
        IEnumerable<Assembly> fullTrustAssemblies = null)
    {
      if (shadowCopyPath != null)
        Directory.CreateDirectory(shadowCopyPath);

      var appDomainSetup = CreateAppDomainSetup(baseAssembly, shadowCopyPath);
      var permissionSet = GetPermissionSet(permissions);
      var trustAssemblies = GetStrongNames(fullTrustAssemblies);

      var appDomain = System.AppDomain.CreateDomain(baseAssembly.GetName().Name, null, appDomainSetup, permissionSet, trustAssemblies);
      return new AppDomain(appDomain);
    }

    private AppDomainSetup CreateAppDomainSetup (Assembly assembly, [CanBeNull] string shadowCopyPath)
    {
      return new AppDomainSetup
             {
                 ApplicationBase = Path.GetDirectoryName(assembly.Location),
                 ApplicationName = assembly.GetName().Name,
                 ConfigurationFile = assembly.Location + ".config",
                 ShadowCopyFiles = shadowCopyPath != null ? "true" : "false",
                 CachePath = shadowCopyPath
             };
    }

    private PermissionSet GetPermissionSet ([CanBeNull] IEnumerable<IPermission> permissions)
    {
      if (permissions == null)
        return new PermissionSet(PermissionState.Unrestricted);

      var permissionSet = new PermissionSet(PermissionState.None);
      foreach (var permission in permissions)
        permissionSet.AddPermission(permission);
      return permissionSet;
    }

    private StrongName[] GetStrongNames ([CanBeNull] IEnumerable<Assembly> fullTrustAssemblies)
    {
      if (fullTrustAssemblies == null)
        return new StrongName[0];

      return fullTrustAssemblies.Select(GetStrongName).ToArray();
    }

    private StrongName GetStrongName (Assembly fullTrustAssembly)
    {
      var assemblyName = fullTrustAssembly.GetName();
      var publicKey = assemblyName.GetPublicKey();
      Debug.Assert(publicKey.Length == 0, "Full trust assembly must have public key.");

      var strongNamePublicKeyBlob = new StrongNamePublicKeyBlob(publicKey);
      return new StrongName(strongNamePublicKeyBlob, assemblyName.Name, assemblyName.Version);
    }
  }
}