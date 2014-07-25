// ----------------------------------------------------------------------------------
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
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public string ProviderVersion { get; set; }
        public string ServerVersion { get; set; }
        #endregion

        public PSServer() { }
        public PSServer(
            string serverId,
            string name,
            string type,
            DateTime lastHeartbeat,
            string providerVersion,
            string serverVersion)
        {
            this.ServerId = serverId;
            this.Name = name;
            this.Type = type;
            this.LastHeartbeat = lastHeartbeat;
            this.ProviderVersion = providerVersion;
            this.ServerVersion = serverVersion;
        }
    }

    public class PSProtectedContainer
    {
        #region Properties
        public string ProtectedContainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Configured { get; set; }
        public string ReplicationProvider { get; set; }
        public string ReplicationProviderSettings { get; set; }
        public string ServerId { get; set; }
        #endregion

        public PSProtectedContainer() { }
        public PSProtectedContainer(
            string protectedContainerId,
            string name,
            string type,
            bool configured,
            string replicationProvider,
            string replicationProviderSettings,
            string serverId)
        {
            this.ProtectedContainerId = protectedContainerId;
            this.Name = name;
            this.Type = type;
            this.Configured = configured;
            this.ReplicationProvider = replicationProvider;
            this.ReplicationProviderSettings = replicationProviderSettings;
            this.ServerId = serverId;
        }
    }

    public class PSVirtualMachine
    {
        public string VirtualMachineId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Protected { get; set; }
        public string ProtectedContainerId { get; set; }
        public string ReplicationProvider { get; set; }
        public string ReplicationProviderSettings { get; set; }
        public string ServerId { get; set; }
        public string ServerName { get; set; }

        public PSVirtualMachine() { }
        public PSVirtualMachine(
            string virtualMachineId,
            string name,
            string type,
            bool protectedOrNot,
            string protectedContainerId,
            string replicationProvider,
            string replicationProviderSettings,
            string serverId,
            string serverName)
        {
            this.VirtualMachineId = virtualMachineId;
            this.Name = name;
            this.Type = type;
            this.Protected = protectedOrNot;
            this.ProtectedContainerId = protectedContainerId;
            this.ReplicationProvider = replicationProvider;
            this.ReplicationProviderSettings = replicationProviderSettings;
            this.ServerId = serverId;
            this.ServerName = serverName;
        }
    }

    public class PSJob
    {
        public string ID { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public bool Completed { get; set; }

        public PSJob() {}
        public PSJob(string id, string state, string type, bool completed)
        {
            this.ID = id;
            this.State = state;
            this.Type = type;
            this.Completed = completed;
        }
    }
}