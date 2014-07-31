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
    using System.Diagnostics;
    using System.Management.Automation;
    using System.Threading;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureSiteRecoveryVirtualMachine")]
    [OutputType(typeof(Microsoft.Azure.Management.SiteRecovery.Models.Job))]
    public class SetAzureSiteRecoveryVirtualMachine : RecoveryServicesCmdletBase
    {

        #region Parameters
        /// <summary>
        /// ID of the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private string id;

        /// <summary>
        /// ID of the ProtectedContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectedContianerId
        {
            get { return this.protectedContainerId; }
            set { this.protectedContainerId = value; }
        }
        private string protectedContainerId;

        /// <summary>
        /// ID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }
        private string serverId;

        /// <summary>
        /// Bool value to either to say either enable or disable protection.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            PSRecoveryServicesClient.EnableProtection,
            PSRecoveryServicesClient.DisableProtection)]
        public string Protection
        {
            get { return this.protection; }
            set { this.protection = value; }
        }
        private string protection;

        /// <summary>
        /// Bool value to either to say either enable or disable protection.
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
                jobResponse =
                    RecoveryServicesClient.SetProtectionOnVirtualMachine(
                    serverId,
                    protectedContainerId,
                    id,
                    protection);

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

        private void WriteJob(Microsoft.Azure.Management.SiteRecovery.Models.Job job)
        {
            WriteObject(new ASRJob(job.ID, job.State, job.Completed));
        }
    }
}