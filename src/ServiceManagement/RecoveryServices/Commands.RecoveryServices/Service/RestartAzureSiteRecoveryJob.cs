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
    using System;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    #endregion

    /// <summary>
    /// Restarts Azure Site Recovery Job.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Restart, "AzureSiteRecoveryJob", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(ASRJob))]
    public class RestartAzureSiteRecoveryJob : RecoveryServicesCmdletBase
    {
        #region Parameters

        /// <summary>
        /// Job ID.
        /// </summary>
        private string id;

        /// <summary>
        /// Job object.
        /// </summary>
        private ASRJob job;

        /// <summary>
        /// Gets or sets Job ID.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets Job Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public ASRJob Job
        {
            get { return this.job; }
            set { this.job = value; }
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
                    case ASRParameterSets.ByObject:
                        this.id = this.job.ID;
                        this.GetById();
                        break;

                    case ASRParameterSets.ById:
                        this.GetById();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Queries by ID.
        /// </summary>
        private void GetById()
        {
            this.WriteJob(RecoveryServicesClient.RestartAzureSiteRecoveryJob(this.id).Job);
        }

        /// <summary>
        /// Writes Job.
        /// </summary>
        /// <param name="job">JOB object</param>
        private void WriteJob(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job job)
        {
            this.WriteObject(new ASRJob(job));
        }
    }
}