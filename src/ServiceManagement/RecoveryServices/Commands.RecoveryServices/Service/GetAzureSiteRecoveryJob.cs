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
    using System.Management.Automation;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    #endregion

    /// <summary>
    /// Retrieves Azure site Recovery Job.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryJob")]
    public class GetAzureSiteRecoveryJob : RecoveryServicesCmdletBase
    {
        /// <summary>
        /// When ID is passed to the command.
        /// </summary>
        protected const string ById = "ById";

        /// <summary>
        /// When parameters are passed to the command.
        /// </summary>
        protected const string ByParam = "ByParam";

        #region Parameters
        /// <summary>
        /// Job ID.
        /// </summary>
        private string id;

        /// <summary>
        /// Represents Start time for querying the jobs.
        /// </summary>
        private System.DateTime startTime;

        /// <summary>
        /// Represents End time for querying the jobs.
        /// </summary>
        private System.DateTime endTime;

        /// <summary>
        /// Represents State of the Job for querying.
        /// </summary>
        private string state;

        /// <summary>
        /// Gets or sets Job ID.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets start time. Allows to filter the list of jobs started after the given 
        /// start time.
        /// </summary>
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "Start time of job should be greater than this.")]
        [ValidateNotNullOrEmpty]
        public System.DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        /// <summary>
        /// Gets or sets end time. Allows to filter the list of jobs started after the given start 
        /// time.
        /// </summary>
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "End time of job should be less than this.")]
        [ValidateNotNullOrEmpty]
        public System.DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        /// <summary>
        /// Gets or sets state. Take string input for possible States of ASR Job. Use this parameter 
        /// to get filtered view of Jobs
        /// </summary>
        /// Considered Valid states from WorkflowStatus enum in SRS (WorkflowData.cs)
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "State of job to return.")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            "Aborted", 
            "Cancelled", 
            "Cancelling", 
            "Completed", 
            "Failed", 
            "InProgress", 
            "PartiallySucceeded", 
            "CompletedWithInformation", 
            "RolledBack", 
            "Skipped", 
            "Waiting", 
            "WaitingForFinalizeProtection", 
            "WaitingForManualAction", 
            "WaitingForStopTestFailover", 
            "WaitingForUserInputAfterDataSync", 
            "NotStarted", 
            "Unknown")]
        public string State
        {
            get { return this.state; }
            set { this.state = value; }
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
                    case ById:
                        this.GetById();
                        break;

                    case ByParam:
                    default:
                        this.GetByParam();
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
            this.WriteObject(RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(this.id).Job);
        }

        /// <summary>
        /// Queries by Parameters.
        /// </summary>
        private void GetByParam()
        {
            this.WriteObject(RecoveryServicesClient.GetAzureSiteRecoveryJob().Jobs);
        }
    }
}