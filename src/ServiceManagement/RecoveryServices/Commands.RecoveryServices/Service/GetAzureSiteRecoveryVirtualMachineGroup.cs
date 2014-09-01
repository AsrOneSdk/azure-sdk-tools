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
    using System.Collections.Generic;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Retrieves Azure Site Recovery Virtual Machine group.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachineGroup", DefaultParameterSetName = ASRParameterSets.ByRPObject)]
    [OutputType(typeof(IEnumerable<ASRVirtualMachineGroup>))]
    public class GetAzureSiteRecoveryVirtualMachineGroup : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// Virtual Machine ID.
        /// </summary>
        private string id;

        /// <summary>
        /// Name of the Virtual Machine.
        /// </summary>
        private string name;

        /// <summary>
        /// Protection container ID.
        /// </summary>
        private string protectionContainerId;

        /// <summary>
        /// Protection container object.
        /// </summary>
        private ASRProtectionContainer protectionContainer;

        /// <summary>
        /// Gets or sets ID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithId, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets name of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets ID of the ProtectionContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByIDs, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectionContainerId
        {
            get { return this.protectionContainerId; }
            set { this.protectionContainerId = value; }
        }

        /// <summary>
        /// Gets or sets Protection Container Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByRPObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionContainer ProtectionContainer
        {
            get { return this.protectionContainer; }
            set { this.protectionContainer = value; }
        }

        #endregion Parameters

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ASRParameterSets.ByRPObject:
                    case ASRParameterSets.ByObjectWithId:
                    case ASRParameterSets.ByObjectWithName:
                        this.protectionContainerId = this.protectionContainer.ProtectionContainerId;
                        break;
                    case ASRParameterSets.ByIDs:
                    case ASRParameterSets.ByIDsWithId:
                    case ASRParameterSets.ByIDsWithName:
                        break;
                }

                if (this.id != null)
                {
                    this.GetById();
                }
                else if (this.name != null)
                {
                    this.GetByName();
                }
                else
                {
                    this.GetAll();
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Queries by name.
        /// </summary>
        private void GetByName()
        {
            VirtualMachineGroupListResponse virtualMachineListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                this.protectionContainerId);

            bool found = false;
            foreach (var virtualMachineGroup in virtualMachineListResponse.VmGroups)
            {
                if (0 == string.Compare(this.name, virtualMachineGroup.Name, true))
                {
                    this.WriteVirtualMachineGroup(virtualMachineGroup);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineGroupNotFound,
                    this.name,
                    this.protectionContainerId));
            }
        }

        /// <summary>
        /// Queries by ID.
        /// </summary>
        private void GetById()
        {
            var vmgResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                this.protectionContainerId,
                this.id);

            this.WriteVirtualMachineGroup(vmgResponse.VmGroup);
        }

        /// <summary>
        /// Queries all.
        /// </summary>
        private void GetAll()
        {
            VirtualMachineGroupListResponse vmgListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                this.protectionContainerId);

            this.WriteVirtualMachineGroups(vmgListResponse.VmGroups);
        }

        /// <summary>
        /// Writes Virtual Machine groups.
        /// </summary>
        /// <param name="vmgs">List of Virtual Machine group</param>
        private void WriteVirtualMachineGroups(IList<VirtualMachineGroup> vmgs)
        {
            foreach (var vmg in vmgs)
            {
                this.WriteVirtualMachineGroup(vmg);
            }
        }

        /// <summary>
        /// Writes Virtual Machine group.
        /// </summary>
        /// <param name="vmg">Virtual Machine group</param>
        private void WriteVirtualMachineGroup(VirtualMachineGroup vmg)
        {
            this.WriteObject(
                new ASRVirtualMachineGroup(
                    vmg.ID,
                    vmg.ServerId,
                    vmg.ProtectionContainerId,
                    vmg.Name,
                    vmg.Type,
                    vmg.FabricObjectId,
                    vmg.Protected,
                    vmg.CanCommit,
                    vmg.CanFailover,
                    vmg.CanReverseReplicate,
                    vmg.IsRelationshipReversed,
                    vmg.ProtectionStateDescription,
                    vmg.TestFailoverStateDescription,
                    vmg.ReplicationHealth,
                    vmg.ReplicationProvider,
                    vmg.ReplicationProviderSettings,
                    vmg.VirtualMachineList),
                true);
        }
    }
}