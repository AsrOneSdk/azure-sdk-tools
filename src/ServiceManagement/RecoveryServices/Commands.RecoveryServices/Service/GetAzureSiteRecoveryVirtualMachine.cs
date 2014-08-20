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

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachine", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(IEnumerable<ASRVirtualMachine>))]
    public class GetAzureSiteRecoveryVirtualMachine : RecoveryServicesCmdletBase
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
        /// GUID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByIDs, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }
        private string serverId;

        /// <summary>
        /// Protected Container Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionContainer ProtectedContainer
        {
            get { return this.protectedContainer; }
            set { this.protectedContainer = value; }
        }
        private ASRProtectionContainer protectedContainer;

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
                        serverId = protectedContainer.ServerId;
                        protectionContainerId = protectedContainer.ProtectionContainerId;
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
            VirtualMachineListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                protectionContainerId);

            bool found = false;
            foreach (VirtualMachine vm in vmListResponse.Vms)
            {
                if (0 == string.Compare(name, vm.Name, true))
                {
                    WriteVirtualMachine(vm);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineNotFound,
                    name,
                    protectionContainerId));
            }
        }

        private void GetById()
        {
            VirtualMachineResponse vmResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                protectionContainerId, 
                id);

            WriteVirtualMachine(vmResponse.Vm);
        }

        private void GetAll()
        {
            VirtualMachineListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                protectionContainerId);

            WriteVirtualMachines(vmListResponse.Vms);
        }

        private void WriteVirtualMachines(IList<VirtualMachine> vms)
        {
            foreach (VirtualMachine vm in vms)
            {
                WriteVirtualMachine(vm);
            }
        }

        private void WriteVirtualMachine(VirtualMachine vm)
        {
            WriteObject(
                new ASRVirtualMachine(
                    vm.ID,
                    vm.ServerId,
                    vm.ProtectionContainerId,
                    vm.Name,
                    vm.Protected,
                    vm.IsRelationshipReversed,
                    vm.ProtectionState,
                    vm.TestFailoverState,
                    vm.ReplicationProvider,
                    vm.ReplicationProviderSettings),
                true);
        }
    }
}