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
    /// Used to initiate a commit operation.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "AzureSiteRecoveryTestFailover")]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class StartAzureSiteRecoveryTestFailover : RecoveryServicesCmdletBase
    {
        protected const string ByRpId = "ByRpId";
        protected const string ByVmId = "ByVmId";

        #region Parameters
        /// <summary>
        /// ID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ByRpId, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string RpId
        {
            get { return this.rpId; }
            set { this.rpId = value; }
        }
        private string rpId;

        /// <summary>
        /// Failover direction for the recovery plan.
        /// </summary>
        [Parameter(ParameterSetName = ByRpId, Mandatory = true)]
        [ValidateSet(
          PSRecoveryServicesClient.PrimaryToSecondary,
          PSRecoveryServicesClient.SecondaryToPrimary)]
        public string FailoverDirection
        {
            get { return this.failoverDirection; }
            set { this.failoverDirection = value; }
        }
        private string failoverDirection;

        /// <summary>
        /// This is required to wait for job completion.
        /// </summary>
        [Parameter]
        public SwitchParameter WaitForCompletion
        {
            get { return this.waitForCompletion; }
            set { this.waitForCompletion = value; }
        }
        private bool waitForCompletion;
        #endregion Parameters

        private JobResponse jobResponse = null;
        private bool stopProcessing = false;

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ByRpId:
                        StartRpTestFailover();
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
            stopProcessing = true;
        }

        private void StartRpTestFailover()
        {
            RpTestFailoverRequest rpTestFailoverRequest = new RpTestFailoverRequest();
            rpTestFailoverRequest.FailoverDirection = this.FailoverDirection;
            jobResponse = RecoveryServicesClient.StartAzureSiteRecoveryTestFailover(
                this.RpId, 
                rpTestFailoverRequest);

            WriteJob(jobResponse.Job);

            string jobId = jobResponse.Job.ID;
            while (waitForCompletion)
            {
                if (jobResponse.Job.Completed || stopProcessing)
                {
                    break;
                }

                Thread.Sleep(PSRecoveryServicesClient.TimeToSleepBeforeFetchingJobDetailsAgain);
                jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(jobResponse.Job.ID);
                WriteObject("JobState: " + jobResponse.Job.State);
            }
        }

        private void WriteJob(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job job)
        {
            WriteObject(new ASRJob(job));
        }
    }
}