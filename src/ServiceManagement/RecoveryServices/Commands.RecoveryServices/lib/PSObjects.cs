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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVaultSettings" /> class.
        /// </summary>
        public ASRVaultSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVaultSettings" /> class with Resource
        /// and Cloud Service names.
        /// </summary>
        /// <param name="resourceName">Resource Name</param>
        /// <param name="cloudServiceName">Cloud Service Name</param>
        public ASRVaultSettings(string resourceName, string cloudServiceName)
        {
            this.ResourceName = resourceName;
            this.CloudServiceName = cloudServiceName;
        }

        #region Properties
        /// <summary>
        /// Gets or sets Resource Name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets Cloud Service Name.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRServer" /> class.
        /// </summary>
        public ASRServer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRServer" /> class with required 
        /// parameters.
        /// </summary>
        /// <param name="serverId">Server ID</param>
        /// <param name="name">Name of the Server</param>
        /// <param name="lastHeartbeat">Last communicated date time</param>
        /// <param name="providerVersion">Provider Version</param>
        /// <param name="serverVersion">Server version</param>
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
        /// <summary>
        /// Gets or sets Server ID.
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// Gets or sets Name of the Server.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Last communicated time.
        /// </summary>
        public DateTime LastHeartbeat { get; set; }

        /// <summary>
        /// Gets or sets Provider version.
        /// </summary>
        public string ProviderVersion { get; set; }

        /// <summary>
        /// Gets or sets Server version.
        /// </summary>
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
        /// Initializes a new instance of the <see cref="ASRProtectionContainer" /> class.
        /// </summary>
        public ASRProtectionContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRProtectionContainer" /> class with 
        /// required parameters.
        /// </summary>
        /// <param name="protectionContainerId">Protection container ID</param>
        /// <param name="name">Name of the Protection container</param>
        /// <param name="configurationStatus">Configuration Status</param>
        /// <param name="replicationProviderSettings">Replication provider settings</param>
        /// <param name="serverId">Server ID</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRProtectionContainer" /> class with 
        /// required parameters.
        /// </summary>
        /// <param name="pc">Protection container object</param>
        public ASRProtectionContainer(ProtectionContainer pc)
        {
            this.ProtectionContainerId = pc.ID;
            this.Name = pc.Name;
            this.ConfigurationStatus = pc.ConfigurationStatus;
            this.Role = pc.Role;
            this.PairedTo = pc.PairedTo;
            this.ReplicationProviderSettings = pc.ReplicationProviderSettings;
            this.ServerId = pc.ServerId;
        }

        #region Properties
        /// <summary>
        /// Gets or sets Protection container ID.
        /// </summary>
        public string ProtectionContainerId { get; set; }

        /// <summary>
        /// Gets or sets name of the Protection container.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets configuration status.
        /// </summary>
        public string ConfigurationStatus { get; set; }

        /// <summary>
        /// Gets or sets a PairedTo of protection container.
        /// </summary>
        [DataMember]
        public string PairedTo { get; set; }

        /// <summary>
        /// Gets or sets a role of the protection container.
        /// </summary>
        [DataMember]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets replication provider settings.
        /// </summary>
        public string ReplicationProviderSettings { get; set; }

        /// <summary>
        /// Gets or sets Server ID.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVirtualMachine" /> class.
        /// </summary>
        public ASRVirtualMachine()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVirtualMachine" /> class with required 
        /// parameters.
        /// </summary>
        /// <param name="id">Virtual Machine ID</param>
        /// <param name="serverId">Server ID</param>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <param name="name">Name of the Virtual Machine</param>
        /// <param name="type">Virtual Machine type</param>
        /// <param name="fabricObjectId">Fabric object ID</param>
        /// <param name="protectedOrNot">Can protected or not</param>
        /// <param name="canCommit">Can commit or not</param>
        /// <param name="canFailover">Can failover or not</param>
        /// <param name="canReverseReplicate">Can reverse replicate or not</param>
        /// <param name="activeLocation">Active location</param>
        /// <param name="protectionStateDescription">Protection state</param>
        /// <param name="testFailoverStateDescription">Test fail over state</param>
        /// <param name="replicationHealth">Replication health</param>
        /// <param name="replicationProvider">Replication provider</param>
        /// <param name="replicationProviderSettings">Replication provider Settings</param>
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
            string activeLocation,
            string protectionStateDescription,
            string testFailoverStateDescription,
            string replicationHealth,
            string replicationProvider,
            string replicationProviderSettings)
            : base(
                id,
                serverId,
                protectionContainerId,
                name,
                type,
                fabricObjectId.ToUpper(),
                protectedOrNot,
                canCommit,
                canFailover,
                canReverseReplicate,
                activeLocation,
                protectionStateDescription,
                testFailoverStateDescription,
                replicationHealth,
                replicationProvider)
        {
            this.ReplicationProviderSettings = replicationProviderSettings;
        }

        /// <summary>
        /// Gets or sets Replication provider settings.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVirtualMachineGroup" /> class.
        /// </summary>
        public ASRVirtualMachineGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRVirtualMachineGroup" /> class with 
        /// required parameters.
        /// </summary>
        /// <param name="id">Virtual Machine group ID</param>
        /// <param name="serverId">Server ID</param>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <param name="name">Name of the Virtual Machine</param>
        /// <param name="type">Virtual Machine type</param>
        /// <param name="fabricObjectId">Fabric object ID</param>
        /// <param name="protectedOrNot">Can protected or not</param>
        /// <param name="canCommit">Can commit or not</param>
        /// <param name="canFailover">Can failover or not</param>
        /// <param name="canReverseReplicate">Can reverse replicate or not</param>
        /// <param name="activeLocation">Active location</param>
        /// <param name="protectionState">Protection state</param>
        /// <param name="testFailoverState">Test fail over state</param>
        /// <param name="replicationHealth">Replication health</param>
        /// <param name="replicationProvider">Replication provider</param>
        /// <param name="replicationProviderSettings">Replication provider Settings</param>
        /// <param name="virtualMachineList">List of Virtual Machines</param>
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
            string activeLocation,
            string protectionState,
            string testFailoverState,
            string replicationHealth,
            string replicationProvider,
            string replicationProviderSettings,
            IList<VirtualMachine> virtualMachineList)
            : base(
                id,
                serverId,
                protectionContainerId,
                name,
                type,
                fabricObjectId.ToUpper(),
                protectedOrNot,
                canCommit,
                canFailover,
                canReverseReplicate,
                activeLocation,
                protectionState,
                testFailoverState,
                replicationHealth,
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
                    vm.FabricObjectId.ToUpper(),
                    vm.Protected,
                    vm.CanCommit,
                    vm.CanFailover,
                    vm.CanReverseReplicate,
                    vm.ActiveLocation,
                    vm.ProtectionStateDescription,
                    vm.TestFailoverStateDescription,
                    vm.ReplicationHealth,
                    vm.ReplicationProvider,
                    vm.ReplicationProviderSettings));
            }
        }

        /// <summary>
        /// Gets or sets Replication provider settings.
        /// </summary>
        public string ReplicationProviderSettings { get; set; }

        /// <summary>
        /// Gets or sets Virtual Machine list.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRProtectionEntity" /> class.
        /// </summary>
        public ASRProtectionEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRProtectionEntity" /> class with 
        /// required parameters.
        /// </summary>
        /// <param name="protectionEntityId">Protection Entity ID</param>
        /// <param name="serverId">Server ID</param>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <param name="name">Name of the Virtual Machine</param>
        /// <param name="type">Virtual Machine type</param>
        /// <param name="fabricObjectId">Fabric object ID</param>
        /// <param name="protectedOrNot">Can protected or not</param>
        /// <param name="canCommit">Can commit or not</param>
        /// <param name="canFailover">Can failover or not</param>
        /// <param name="canReverseReplicate">Can reverse replicate or not</param>
        /// <param name="activeLocation">Active location</param>
        /// <param name="protectionStateDescription">Protection state</param>
        /// <param name="testFailoverStateDescription">Test fail over state</param>
        /// <param name="replicationHealth">Replication health</param>
        /// <param name="replicationProvider">Replication provider</param>
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
            string activeLocation,
            string protectionStateDescription,
            string testFailoverStateDescription,
            string replicationHealth,
            string replicationProvider)
        {
            this.ID = protectionEntityId;
            this.ServerId = serverId;
            this.ProtectionContainerId = protectionContainerId;
            this.Name = name;
            this.Type = type;
            this.FabricObjectId = fabricObjectId;
            this.Protected = protectedOrNot;
            this.ProtectionState = protectionStateDescription;
            this.CanCommit = canCommit;
            this.CanFailover = canFailover;
            this.CanReverseReplicate = canReverseReplicate;
            this.ReplicationProvider = replicationProvider;
            this.ActiveLocation = activeLocation;
            this.ReplicationHealth = replicationHealth;
            this.TestFailoverState = testFailoverStateDescription;
        }

        /// <summary>
        /// Gets or sets Protection entity ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets Server ID.
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// Gets or sets Protection container ID.
        /// </summary>
        public string ProtectionContainerId { get; set; }

        /// <summary>
        /// Gets or sets Name of the Protection entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets type of the Protection entity.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets fabric object ID.
        /// </summary>
        public string FabricObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is protected or not.
        /// </summary>
        public bool Protected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it can be committed or not.
        /// </summary>
        public bool CanCommit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it can be failed over or not.
        /// </summary>
        public bool CanFailover { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it can be reverse replicated or not.
        /// </summary>
        public bool CanReverseReplicate { get; set; }

        /// <summary>
        /// Gets or sets a active location of protection entity.
        /// </summary>
        public string ActiveLocation { get; set; }

        /// <summary>
        /// Gets or sets protection state.
        /// </summary>
        public string ProtectionState { get; set; }

        /// <summary>
        /// Gets or sets Replication health.
        /// </summary>
        public string ReplicationHealth { get; set; }

        /// <summary>
        /// Gets or sets test failover state.
        /// </summary>
        public string TestFailoverState { get; set; }

        /// <summary>
        /// Gets or sets Replication provider.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRJob" /> class.
        /// </summary>
        public ASRJob()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRJob" /> class with required parameters.
        /// </summary>
        /// <param name="job">ASR Job object</param>
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
        /// <summary>
        /// Gets or sets Job ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets Activity ID.
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// Gets or sets State of the Job.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets Start timestamp.
        /// </summary>
        public string StartTimestamp { get; set; }

        /// <summary>
        /// Gets or sets End timestamp.
        /// </summary>
        public string EndTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Job is completed or not.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets list of allowed actions.
        /// </summary>
        public List<string> AllowedActions { get; set; }

        /// <summary>
        /// Gets or sets Job display name.
        /// </summary>
        public string JobDisplayName { get; set; }

        /// <summary>
        /// Gets or sets list of Jobs.
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets list of tasks.
        /// </summary>
        public List<AsrTask> Tasks { get; set; }

        /// <summary>
        /// Gets or sets list of Errors.
        /// </summary>
        public List<ErrorDetails> Errors { get; set; }
        #endregion
    }
}