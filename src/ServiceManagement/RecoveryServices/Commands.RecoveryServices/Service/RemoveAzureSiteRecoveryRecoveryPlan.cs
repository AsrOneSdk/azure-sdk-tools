﻿// ----------------------------------------------------------------------------------
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
    using System.Diagnostics;
    using System.Management.Automation;
    using System.Threading;
    #endregion

    /// <summary>
    /// Remove a Recovery Plan from the current Azure Site Recovery Vault.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureSiteRecoveryRecoveryPlan")]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class RemoveAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// ID of the Server.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private string id;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                RemoveRecoveryPlanById();
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void RemoveRecoveryPlanById()
        {
            RecoveryServicesClient.RemoveAzureSiteRecoveryRecoveryPlan(this.Id);
        }
    }
}