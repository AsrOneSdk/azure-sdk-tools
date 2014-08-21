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
    /// Set Protection Entity protection state.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureSiteRecoveryProtectionEntity", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class SetAzureSiteRecoveryProtectionEntity : RecoveryServicesCmdletBase
    {
        #region Parameters
        private string id;
        private string protectionContainerId;
        private ASRProtectionEntity protectionEntity;
        private string protection;
        private bool waitForCompletion;
        private JobResponse jobResponse = null;
        private bool stopProcessing = false;

        /// <summary>
        /// ID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByIDs, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// ID of the ProtectionContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByIDs, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectionContianerId
        {
            get { return this.protectionContainerId; }
            set { this.protectionContainerId = value; }
        }

        /// <summary>
        /// Protection Entity Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionEntity ProtectionEntity
        {
            get { return this.protectionEntity; }
            set { this.protectionEntity = value; }
        }

        /// <summary>
        /// Protection to set, either enable or disable.
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

        /// <summary>
        /// On passing, command waits till completion.
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
                    case ASRParameterSets.ByObject:
                        this.id = this.protectionEntity.ID;
                        this.protectionContainerId = this.protectionEntity.ProtectionContainerId;
                        break;
                    case ASRParameterSets.ByIDs:
                        break;
                }

                this.jobResponse =
                    RecoveryServicesClient.SetProtectionOnProtectionEntity(
                    this.protectionContainerId,
                    this.id,
                    this.protection);

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

        private void WriteJob(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job job)
        {
            this.WriteObject(new ASRJob(job));
        }
    }
}