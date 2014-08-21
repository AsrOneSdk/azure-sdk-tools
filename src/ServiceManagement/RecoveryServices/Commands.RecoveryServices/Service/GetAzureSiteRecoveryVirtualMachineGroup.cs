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
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachineGroup", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(IEnumerable<ASRVirtualMachineGroup>))]
    public class GetAzureSiteRecoveryVirtualMachineGroup : RecoveryServicesCmdletBase
    {
        #region Parameters
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
        private string id;

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
        private string name;

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
        private string protectionContainerId;

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
        private ASRProtectionContainer protectionContainer;

        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ASRParameterSets.ByObject:
                    case ASRParameterSets.ByObjectWithId:
                    case ASRParameterSets.ByObjectWithName:
                        protectionContainerId = protectionContainer.ProtectionContainerId;
                        break;
                    case ASRParameterSets.ByIDs:
                    case ASRParameterSets.ByIDsWithId:
                    case ASRParameterSets.ByIDsWithName:
                        break;
                }

                if (id != null)
                {
                    GetById();
                }
                else if (name != null)
                {
                    GetByName();
                }
                else
                {
                    GetAll();
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetByName()
        {
            VirtualMachineGroupListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                protectionContainerId);

            bool found = false;
            foreach (var vmGroup in vmListResponse.VmGroups)
            {
                if (0 == string.Compare(name, vmGroup.Name, true))
                {
                    WriteVirtualMachineGroup(vmGroup);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineGroupNotFound,
                    name,
                    protectionContainerId));
            }
        }

        private void GetById()
        {
            var vmgResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                protectionContainerId, 
                id);

            WriteVirtualMachineGroup(vmgResponse.VmGroup);
        }

        private void GetAll()
        {
            VirtualMachineGroupListResponse vmgListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachineGroup(
                protectionContainerId);

            WriteVirtualMachineGroups(vmgListResponse.VmGroups);
        }

        private void WriteVirtualMachineGroups(IList<VirtualMachineGroup> vmgs)
        {
            foreach (var vmg in vmgs)
            {
                WriteVirtualMachineGroup(vmg);
            }
        }

        private void WriteVirtualMachineGroup(VirtualMachineGroup vmg)
        {
            WriteObject(
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
                    vmg.ReplicationProvider,
                    vmg.ReplicationProviderSettings,
                    vmg.VirtualMachineList),
                true);
        }
    }
}