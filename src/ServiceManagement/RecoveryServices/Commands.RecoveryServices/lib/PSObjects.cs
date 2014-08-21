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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Azure Site Recovery Vault Settings.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRVaultSettings
    {
        public ASRVaultSettings()
        {
        }

        public ASRVaultSettings(string resourceName, string cloudServiceName)
        {
            this.ResourceName = resourceName;
            this.CloudServiceName = cloudServiceName;
        }

        #region Properties
        public string ResourceName { get; set; }

        public string CloudServiceName { get; set; }
        #endregion Properties
    }

    /// <summary>
    /// Azure Site Recovery Server.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRServer
    {
        public ASRServer()
        {
        }

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

        #region Properties
        public string ServerId { get; set; }

        public string Name { get; set; }

        public DateTime LastHeartbeat { get; set; }

        public string ProviderVersion { get; set; }

        public string ServerVersion { get; set; }
        #endregion
    }

    /// <summary>
    /// Azure Site Recovery Protection Container.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRProtectionContainer
    {
        /// <summary>
        /// Empty argument constructor.
        /// </summary>
        public ASRProtectionContainer()
        {
        }

        /// <summary>
        /// Parameterised constructor.
        /// </summary>
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

        #region Properties
        public string ProtectionContainerId { get; set; }

        public string Name { get; set; }

        public string ConfigurationStatus { get; set; }

        public string ReplicationProviderSettings { get; set; }

        public string ServerId { get; set; }
        #endregion
    }

    /// <summary>
    /// Azure Site Recovery Virtual Machine.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRVirtualMachine : ASRProtectionEntity
    {
        public ASRVirtualMachine()
        {
        }

        public ASRVirtualMachine(
            string id,
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
                id,
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

        public string ReplicationProviderSettings { get; set; }
    }

    /// <summary>
    /// Azure Site Recovery Virtual Machine Group.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRVirtualMachineGroup : ASRProtectionEntity
    {
        public ASRVirtualMachineGroup()
        {
        }

        public ASRVirtualMachineGroup(
            string id,
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
                id,
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

        public string ReplicationProviderSettings { get; set; }

        public List<ASRVirtualMachine> VirtualMachineList { get; set; }
    }

    /// <summary>
    /// Azure Site Recovery Protection Entity.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRProtectionEntity
    {
        public ASRProtectionEntity()
        {
        }

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
    }

    /// <summary>
    /// Azure Site Recovery Job.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all related objects together.")]
    public class ASRJob
    {
        public ASRJob()
        {
        }

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
    }
}