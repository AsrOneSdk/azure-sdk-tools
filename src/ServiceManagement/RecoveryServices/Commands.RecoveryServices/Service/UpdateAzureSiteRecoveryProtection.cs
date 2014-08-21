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
    /// Used to initiate a recovery protection operation.
    /// </summary>
    [Cmdlet(VerbsData.Update, "AzureSiteRecoveryProtection")]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class UpdateAzureSiteRecoveryProtection : RecoveryServicesCmdletBase
    {
        protected const string ByRpId = "ByRpId";
        protected const string ByVmId = "ByVmId";

        #region Parameters
        private string recoveryPlanId;
        private bool waitForCompletion;
        private JobResponse jobResponse = null;
        private bool stopProcessing = false;

        /// <summary>
        /// ID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ByRpId, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string RpId
        {
            get { return this.recoveryPlanId; }
            set { this.recoveryPlanId = value; }
        }

        /// <summary>
        /// This is required to wait for job completion.
        /// </summary>
        [Parameter]
        public SwitchParameter WaitForCompletion
        {
            get { return this.waitForCompletion; }
            set { this.waitForCompletion = value; }
        }
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ByRpId:
                        this.SetRpReprotect();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        protected override void StopProcessing()
        {
            // Ctrl + C and etc
            base.StopProcessing();
            this.stopProcessing = true;
        }

        private void SetRpReprotect()
        {
            this.jobResponse = RecoveryServicesClient.UpdateAzureSiteRecoveryProtection(this.RpId);

            this.WriteJob(this.jobResponse.Job);

            string jobId = this.jobResponse.Job.ID;
            while (this.waitForCompletion)
            {
                if (this.jobResponse.Job.Completed || this.stopProcessing)
                {
                    break;
                }

                Thread.Sleep(PSRecoveryServicesClient.TimeToSleepBeforeFetchingJobDetailsAgain);
                this.jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(this.jobResponse.Job.ID);
                this.WriteObject("JobState: " + this.jobResponse.Job.State);
            }
        }

        private void WriteJob(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job job)
        {
            this.WriteObject(new ASRJob(job));
        }
    }
}