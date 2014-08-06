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

namespace Microsoft.Azure.Commands.RecoveryServices.SiteRecovery
{
    #region Using directives
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    #endregion

    public class ASRVaultSettings
    {
        #region Properties
        public string ResourceName { get; set; }
        public string CloudServiceName { get; set; }
        #endregion Properties

        public ASRVaultSettings() { }
        public ASRVaultSettings(string resourceName, string cloudServiceName)
        {
            ResourceName = resourceName;
            CloudServiceName = cloudServiceName;
        }
    }

    public class ASRServer
    {
        #region Properties
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public string ProviderVersion { get; set; }
        public string ServerVersion { get; set; }
        #endregion

        public ASRServer() { }
        public ASRServer(
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

    public class ASRProtectedContainer
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

        public ASRProtectedContainer() { }
        public ASRProtectedContainer(
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

    public class ASRVirtualMachine
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

        public ASRVirtualMachine() { }
        public ASRVirtualMachine(
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

    public class ASRJob
    {
        #region Properties
        public string ID { get; set; }
        public string ActivityId { get; set; }
        public string State { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool Completed { get; set; }
        public List<string> AllowedActions { get; set; }
        public string JobDisplayName { get; set; }
        public List<Job> Jobs { get; set; }
        public List<AsrTask> Tasks { get; set; }
        public List<ErrorDetails> Errors { get; set; }
        #endregion

        public ASRJob() { }
        public ASRJob(Job job)
        {
            this.ID = job.ID;
            this.ActivityId = job.ActivityId;
            this.State = job.State;
            this.EndTimestamp = job.EndTimestamp;
            this.StartTimestamp = job.StartTimestamp;
            this.Completed = job.Completed;
            this.AllowedActions = job.AllowedActions as List<string>;
            this.JobDisplayName = job.JobDisplayName;
            this.Jobs = job.Jobs as List<Job>;
            this.Tasks = job.Tasks as List<AsrTask>;
            this.Errors = job.Errors as List<ErrorDetails>;
        }
    }
}