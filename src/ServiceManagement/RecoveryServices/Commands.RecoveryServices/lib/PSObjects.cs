﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.RecoveryServices
{
    #region Using directives
    using System;
    using System.Runtime.Serialization;
    #endregion

    public class PSVaultSettings
    {
        #region Properties
        public string ResourceName { get; set; }
        public string CloudServiceName { get; set; }
        #endregion Properties

        public PSVaultSettings() { }
        public PSVaultSettings(string resourceName, string cloudServiceName)
        {
            ResourceName = resourceName;
            CloudServiceName = cloudServiceName;
        }
    }

    public class PSServer
    {
        #region Properties
        public string Id;
        public string Name;
        public string Type;
        public DateTime LastHeartbeat;
        public string ProviderVersion;
        public string ServerVersion;
        #endregion

        public PSServer() { }
    }
}