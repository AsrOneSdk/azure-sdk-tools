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
    using System.Diagnostics;
    using System.Management.Automation;
    using System.Threading;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Remove a Recovery Plan from the current Azure Site Recovery Vault.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureSiteRecoveryRecoveryPlan", DefaultParameterSetName = ASRParameterSets.ByRPObject)]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class RemoveAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// ID of the Recovery Plan.
        /// </summary>
        private string recoveryPlanId;

        /// <summary>
        /// Recovery Plan object.
        /// </summary>
        private ASRRecoveryPlan recoveryPlan;

        /// <summary>
        /// Gets or sets ID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string RpId
        {
            get { return this.recoveryPlanId; }
            set { this.recoveryPlanId = value; }
        }

        /// <summary>
        /// Gets or sets Recovery Plan object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByRPObject, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public ASRRecoveryPlan RecoveryPlan
        {
            get { return this.recoveryPlan; }
            set { this.recoveryPlan = value; }
        }
        #endregion Parameters

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ASRParameterSets.ByRPObject:
                        this.recoveryPlanId = this.recoveryPlan.RpId;
                        break;
                    case ASRParameterSets.ById:
                        break;
                }

                this.RemoveRecoveryPlanById();
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Invoke remove Azure Site Recovery recovery plan.
        /// </summary>
        private void RemoveRecoveryPlanById()
        {
            RecoveryServicesClient.RemoveAzureSiteRecoveryRecoveryPlan(this.recoveryPlanId);
        }
    }
}