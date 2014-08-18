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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using System;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public const string EnableProtection = "Enable";
        public const string DisableProtection = "Disable";

        public VirtualMachineListResponse GetAzureSiteRecoveryVirtualMachine(
            string serverId,
            string protectedContainerId)
        {
            return GetSiteRecoveryClient().Vm.List(
                serverId,
                protectedContainerId,
                GetRequestHeaders());
        }

        public VirtualMachineResponse GetAzureSiteRecoveryVirtualMachine(
            string serverId,
            string protectedContainerId,
            string virtualMachineId)
        {
            return GetSiteRecoveryClient().Vm.Get(
                serverId,
                protectedContainerId,
                virtualMachineId,
                GetRequestHeaders());
        }

        public JobResponse SetProtectionOnVirtualMachine(
            string serverId,
            string protectedContainerId,
            string virtualMachineId,
            string protection)
        {

            var requestHeaders = GetRequestHeaders();
            requestHeaders.AgentAuthenticationHeader =
                GenerateAgentAuthenticationHeader(requestHeaders.ClientRequestId);
            
            JobResponse jobResponse = null;

            if(0 == String.Compare(EnableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    GetSiteRecoveryClient().Vm.EnableProtection(
                    serverId,
                    protectedContainerId,
                    virtualMachineId,
                    requestHeaders);
            }
            else if(0 == String.Compare(DisableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    GetSiteRecoveryClient().Vm.DisableProtection(
                    serverId,
                    protectedContainerId,
                    virtualMachineId,
                    requestHeaders);
            }

            return jobResponse;
        }
    }
}