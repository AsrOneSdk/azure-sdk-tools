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
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachineGroup", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(IEnumerable<ASRVirtualMachineGroup>))]
    public class GetAzureSiteRecoveryVirtualMachineGroup : RecoveryServicesCmdletBase
    {
        #region Parameters
        private string id;
        private string name;
        private string protectionContainerId;
        private ASRProtectionContainer protectionContainer;

        /// <summary>
        /// ID of the Virtual Machine.
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
        /// Name of the Virtual Machine.
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
        /// ID of the ProtectionContainer containing the Virtual Machine.
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
        /// Protection Container Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionContainer ProtectionContainer
        {
            get { return this.protectionContainer; }
            set { this.protectionContainer = value; }
        }

        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ASRParameterSets.ByObject:
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

        private void GetById()
        {
            var vmgResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                this.protectionContainerId,
                this.id);

            this.WriteVirtualMachineGroup(vmgResponse.VmGroup);
        }

        private void GetAll()
        {
            VirtualMachineGroupListResponse vmgListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                this.protectionContainerId);

            this.WriteVirtualMachineGroups(vmgListResponse.VmGroups);
        }

        private void WriteVirtualMachineGroups(IList<VirtualMachineGroup> vmgs)
        {
            foreach (var vmg in vmgs)
            {
                this.WriteVirtualMachineGroup(vmg);
            }
        }

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
                    vmg.ProtectionState,
                    vmg.TestFailoverState,
                    vmg.ReplicationHealth,
                    vmg.ReplicationProvider,
                    vmg.ReplicationProviderSettings,
                    vmg.VirtualMachineList),
                true);
        }
    }
}