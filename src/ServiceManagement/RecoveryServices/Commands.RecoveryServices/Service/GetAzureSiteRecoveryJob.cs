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
    using Microsoft.WindowsAzure;
    using System;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryJob")]
    public class GetAzureSiteRecoveryJob : RecoveryServicesCmdletBase
    {
        protected const string ById = "ById";
        protected const string ByParam = "ByParam";

        #region Parameters

        /// <summary>
        /// Job ID.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private string id;

        /// <summary>
        /// Allows to filter the list of jobs started after the given starttime.
        /// </summary>
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "Start time of job should be greater than this.")]
        [ValidateNotNullOrEmpty]
        public System.DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }
        private System.DateTime startTime;

        /// <summary>
        /// Allows to filter the list of jobs started after the given starttime.
        /// </summary>
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "End time of job should be less than this.")]
        [ValidateNotNullOrEmpty]
        public System.DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }
        private System.DateTime endTime;
		
        /// <summary>
        /// Take string input for possible States of ASR Job. Use this parameter to get filtered 
        /// view of Jobs
        /// </summary>
        [Parameter(ParameterSetName = "ByParam", HelpMessage = "State of job to return.")]
        [ValidateNotNullOrEmpty]
        // Considered Valid states from WorkflowStatus enum in SRS (WorkflowData.cs)
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
        private string state;

        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ById:
                        GetById();
                        break;

                    case ByParam:
                    default:
                        GetByParam();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetById()
        {
            WriteObject(RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(id).Job);
        }

        private void GetByParam()
        {
            WriteObject(RecoveryServicesClient.GetAzureSiteRecoveryJob().Jobs);
        }
    }
}