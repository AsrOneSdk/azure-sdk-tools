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
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachine", DefaultParameterSetName = Default)]
    public class GetAzureSiteRecoveryVirtualMachine : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// ID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
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
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private string name;

        /// <summary>
        /// ID of the ProtectedContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = ByName, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = Default, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectedContainerId
        {
            get { return this.protectedContainerId; }
            set { this.protectedContainerId = value; }
        }
        private string protectedContainerId;

        /// <summary>
        /// GUID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = ByName, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = Default, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }
        private string serverId;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ByName:
                        GetByName();
                        break;
                    case ById:
                        GetById();
                        break;
                    case Default:
                        GetByDefault();
                        break;
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
                serverId,
                protectedContainerId);

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
                    protectedContainerId));
            }
        }

        private void GetById()
        {
            VirtualMachineResponse vmResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                serverId, 
                protectedContainerId, 
                id);

            WriteVirtualMachine(vmResponse.Vm);
        }

        private void GetByDefault()
        {
            VirtualMachineListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                serverId,
                protectedContainerId);

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
                    vm.Name,
                    vm.Type,
                    vm.Protected,
                    vm.ProtectedContainerId,
                    vm.ReplicationProvider,
                    vm.ReplicationProviderSettings,
                    vm.ServerId,
                    vm.Name),
                true);
        }
    }
}