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

        public ProtectionEntityListResponse GetAzureSiteRecoveryProtectionEntity(
            string protectionContainerId)
        {
            return GetSiteRecoveryClient().ProtectionEntity.List(protectionContainerId, GetRequestHeaders());
        }

        public ProtectionEntityResponse GetAzureSiteRecoveryProtectionEntity(
            string protectionContainerId,
            string virtualMachineId)
        {
            return GetSiteRecoveryClient().ProtectionEntity.Get(protectionContainerId, virtualMachineId, GetRequestHeaders());
        }

        public JobResponse SetProtectionOnProtectionEntity(
            string protectionContainerId,
            string virtualMachineId,
            string protection)
        {

            var requestHeaders = GetRequestHeaders();
            requestHeaders.AgentAuthenticationHeader = GenerateAgentAuthenticationHeader(requestHeaders.ClientRequestId);
            
            JobResponse jobResponse = null;

            if(0 == String.Compare(EnableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    GetSiteRecoveryClient().ProtectionEntity.EnableProtection(
                    protectionContainerId,
                    virtualMachineId,
                    requestHeaders);
            }
            else if(0 == String.Compare(DisableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    GetSiteRecoveryClient().ProtectionEntity.DisableProtection(
                    protectionContainerId,
                    virtualMachineId,
                    requestHeaders);
            }

            return jobResponse;
        }
    }
}