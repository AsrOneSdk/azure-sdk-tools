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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Recovery services convenience client.
    /// </summary>
    public partial class PSRecoveryServicesClient
    {
        /// <summary>
        /// Represents Enable protection.
        /// </summary>
        public const string EnableProtection = "Enable";

        /// <summary>
        /// Represents Disable protection.
        /// </summary>
        public const string DisableProtection = "Disable";

        /// <summary>
        /// Retrieves Protection Entity.
        /// </summary>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <returns>Protection entity list response</returns>
        public ProtectionEntityListResponse GetAzureSiteRecoveryProtectionEntity(
            string protectionContainerId)
        {
            return 
                this
                .GetSiteRecoveryClient()
                .ProtectionEntity
                .List(protectionContainerId, this.GetRequestHeaders());
        }

        /// <summary>
        /// Retrieves Protection Entity.
        /// </summary>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <param name="virtualMachineId">Virtual Machine ID</param>
        /// <returns>Protection entity response</returns>
        public ProtectionEntityResponse GetAzureSiteRecoveryProtectionEntity(
            string protectionContainerId,
            string virtualMachineId)
        {
            return 
                this
                .GetSiteRecoveryClient()
                .ProtectionEntity
                .Get(protectionContainerId, virtualMachineId, this.GetRequestHeaders());
        }

        /// <summary>
        /// Sets protection on Protection entity.
        /// </summary>
        /// <param name="protectionContainerId">Protection Container ID</param>
        /// <param name="virtualMachineId">Virtual Machine ID</param>
        /// <param name="protection">Protection state to set</param>
        /// <returns>Job response</returns>
        public JobResponse SetProtectionOnProtectionEntity(
            string protectionContainerId,
            string virtualMachineId,
            string protection)
        {
            var requestHeaders = this.GetRequestHeaders();
            requestHeaders.AgentAuthenticationHeader = this.GenerateAgentAuthenticationHeader(requestHeaders.ClientRequestId);
            
            JobResponse jobResponse = null;

            if (0 == string.Compare(EnableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    this.GetSiteRecoveryClient().ProtectionEntity.EnableProtection(
                    protectionContainerId,
                    virtualMachineId,
                    requestHeaders);
            }
            else if (0 == string.Compare(DisableProtection, protection, StringComparison.OrdinalIgnoreCase))
            {
                jobResponse =
                    this.GetSiteRecoveryClient().ProtectionEntity.DisableProtection(
                    protectionContainerId,
                    virtualMachineId,
                    requestHeaders);
            }

            return jobResponse;
        }
    }
}