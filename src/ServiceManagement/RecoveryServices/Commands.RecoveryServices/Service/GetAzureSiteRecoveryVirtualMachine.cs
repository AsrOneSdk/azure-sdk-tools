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
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using System;
    using System.Management.Automation;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachine", DefaultParameterSetName = None)]
    public class GetAzureSiteRecoveryVirtualMachine : RecoveryServicesCmdletBase
    {
        protected const string None = "None";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// GUID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private System.Guid id;

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
        /// GUID of the ProtectedContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [Parameter(ParameterSetName = None, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid ProtectedContianer
        {
            get { return this.protectedContainer; }
            set { this.protectedContainer = value; }
        }
        private System.Guid protectedContainer;

        /// <summary>
        /// GUID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [Parameter(ParameterSetName = None, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        private System.Guid server;
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
                    case None:
                        GetByDefault();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                // Log errors from SRS (good to deserialize the Error Message & print as object)
                WriteObject("ErrorCode: " + cloudException.ErrorCode);
                WriteObject("ErrorMessage: " + cloudException.ErrorMessage);
            }
        }

        private void GetByName()
        {
            VirtualMachineListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                server.ToString(),
                protectedContainer.ToString());

            bool found = false;
            foreach (VirtualMachine vm in vmListResponse.Vms)
            {
                if (0 == string.Compare(name, vm.Name, true))
                {
                    WriteObject(vm);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineNotFound,
                    name,
                    protectedContainer));
            }
        }

        private void GetById()
        {
            VirtualMachineResponse vmResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                server.ToString(), 
                protectedContainer.ToString(), 
                id.ToString());

            WriteObject(vmResponse.Vm);
        }

        private void GetByDefault()
        {
            VirtualMachineListResponse vmListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryVirtualMachine(
                server.ToString(),
                protectedContainer.ToString());

            WriteObject(vmListResponse.Vms, true);
        }
    }
}