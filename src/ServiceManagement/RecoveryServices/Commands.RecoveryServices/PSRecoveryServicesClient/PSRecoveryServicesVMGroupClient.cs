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
        public VirtualMachineGroupListResponse GetAzureSiteRecoveryVirtualMachineGroup(
            string protectionContainerId)
        {
            return GetSiteRecoveryClient().VmGroup.List(protectionContainerId, GetRequestHeaders());
        }

        public VirtualMachineGroupResponse GetAzureSiteRecoveryVirtualMachineGroup(
            string protectionContainerId,
            string virtualMachineGroupId)
        {
            return GetSiteRecoveryClient().VmGroup.Get(protectionContainerId, virtualMachineGroupId, GetRequestHeaders());
        }
    }
}