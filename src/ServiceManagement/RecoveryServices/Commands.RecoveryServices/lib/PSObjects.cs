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
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
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
        public DateTime LastHeartbeat { get; set; }
        public string ProviderVersion { get; set; }
        public string ServerVersion { get; set; }
        #endregion

        public ASRServer() { }
        public ASRServer(
            string serverId,
            string name,
            DateTime lastHeartbeat,
            string providerVersion,
            string serverVersion)
        {
            this.ServerId = serverId;
            this.Name = name;
            this.LastHeartbeat = lastHeartbeat;
            this.ProviderVersion = providerVersion;
            this.ServerVersion = serverVersion;
        }
    }

    public class ASRProtectionContainer
    {
        #region Properties
        public string ProtectionContainerId { get; set; }
        public string Name { get; set; }
        public string ConfigurationStatus { get; set; }
        public string ReplicationProviderSettings { get; set; }

        /// <summary>
        /// Gets or sets the Server Id.
        /// </summary>
        [DataMember]
        public string ServerId { get; set; }
        #endregion

        public ASRProtectionContainer() { }
        public ASRProtectionContainer(
            string protectionContainerId,
            string name,
            string configurationStatus,
            string replicationProviderSettings,
            string serverId)
        {
            this.ProtectionContainerId = protectionContainerId;
            this.Name = name;
            this.ConfigurationStatus = configurationStatus;
            this.ReplicationProviderSettings = replicationProviderSettings;
            this.ServerId = serverId;
        }
    }

    public class ASRVirtualMachine : ASRProtectionEntity
    {
        public string ReplicationProviderSettings { get; set; }

        public ASRVirtualMachine() { }
        public ASRVirtualMachine(
            string Id,
            string serverId,
            string protectionContainerId,
            string name,
            string type,
            string fabricObjectId,
            bool protectedOrNot,
            bool canCommit,
            bool canFailover,
            bool canReverseReplicate,
            bool isRelationshipReversed,
            string protectionState,
            string testFailoverState,
            string replicationProvider,
            string replicationProviderSettings)
            : base(
                Id,
                serverId,
                protectionContainerId,
                name,
                type,
                fabricObjectId,
                protectedOrNot,
                canCommit,
                canFailover,
                canReverseReplicate,
                isRelationshipReversed,
                protectionState,
                testFailoverState,
                replicationProvider)
        {
            this.ReplicationProviderSettings = replicationProviderSettings;
        }
    }

    public class ASRVirtualMachineGroup : ASRProtectionEntity
    {
        public string ReplicationProviderSettings { get; set; }
        public List<ASRVirtualMachine> VirtualMachineList { get; set; }

        public ASRVirtualMachineGroup() { }
        public ASRVirtualMachineGroup(
            string Id,
            string serverId,
            string protectionContainerId,
            string name,
            string type,
            string fabricObjectId,
            bool protectedOrNot,
            bool canCommit,
            bool canFailover,
            bool canReverseReplicate,
            bool isRelationshipReversed,
            string protectionState,
            string testFailoverState,
            string replicationProvider,
            string replicationProviderSettings,
            IList<VirtualMachine> virtualMachineList)
            : base(
                Id,
                serverId,
                protectionContainerId,
                name,
                type,
                fabricObjectId,
                protectedOrNot,
                canCommit,
                canFailover,
                canReverseReplicate,
                isRelationshipReversed,
                protectionState,
                testFailoverState,
                replicationProvider)
        {
            this.ReplicationProviderSettings = replicationProviderSettings;
            this.VirtualMachineList = new List<ASRVirtualMachine>();
            foreach (var vm in virtualMachineList)
            {
                this.VirtualMachineList.Add(
                    new ASRVirtualMachine(
                    vm.ID,
                    vm.ServerId,
                    vm.ProtectionContainerId,
                    vm.Name,
                    vm.Type,
                    vm.FabricObjectId,
                    vm.Protected,
                    vm.CanCommit,
                    vm.CanFailover,
                    vm.CanReverseReplicate,
                    vm.IsRelationshipReversed,
                    vm.ProtectionState,
                    vm.TestFailoverState,
                    vm.ReplicationProvider,
                    vm.ReplicationProviderSettings));
            }
        }
    }

    public class ASRProtectionEntity
    {
        public string ID { get; set; }
        public string ServerId { get; set; }
        public string ProtectionContainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FabricObjectId { get; set; }
        public bool Protected { get; set; }
        public bool CanCommit { get; set; }
        public bool CanFailover { get; set; }
        public bool CanReverseReplicate { get; set; }
        public bool IsRelationshipReversed { get; set; }
        public string ProtectionState { get; set; }
        public string TestFailoverState { get; set; }
        public string ReplicationProvider { get; set; }

        public ASRProtectionEntity() { }
        public ASRProtectionEntity(
            string protectionEntityId,
            string serverId,
            string protectionContainerId,
            string name,
            string type,
            string fabricObjectId,
            bool protectedOrNot,
            bool canCommit,
            bool canFailover,
            bool canReverseReplicate,
            bool isRelationshipReversed,
            string protectionState,
            string testFailoverState,
            string replicationProvider)
        {
            this.ID = protectionEntityId;
            this.ServerId = serverId;
            this.ProtectionContainerId = protectionContainerId;
            this.Name = name;
            this.Type = type;
            this.FabricObjectId = fabricObjectId;
            this.Protected = protectedOrNot;
            this.ProtectionState = protectionState;
            this.CanCommit = canCommit;
            this.CanFailover = canFailover;
            this.CanReverseReplicate = canReverseReplicate;
            this.ReplicationProvider = replicationProvider;
            this.IsRelationshipReversed = isRelationshipReversed;
            this.TestFailoverState = testFailoverState;
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