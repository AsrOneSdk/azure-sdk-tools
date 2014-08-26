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
    /// Used to initiate a recovery plan update operation.
    /// </summary>
    [Cmdlet(VerbsData.Update, "AzureSiteRecoveryRecoveryPlan")]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class UpdateAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// Recovery Plan XML file path.
        /// </summary>
        private string file;

        /// <summary>
        /// Gets or sets XML file path of the Recovery Plan.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string File
        {
            get { return this.file; }
            set { this.file = value; }
        }
        #endregion Parameters

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            try
            {
                string recoveryPlanXml = System.IO.File.ReadAllText(this.File);

                RecoveryServicesClient.UpdateAzureSiteRecoveryRecoveryPlan(recoveryPlanXml);
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }
    }
}