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
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure.Management.RecoveryServices.Models;
    using System;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsLifecycle.Restart, "AzureSiteRecoveryJob")]
    public class RestartAzureSiteRecoveryJob : RecoveryServicesCmdletBase
    {
        #region Parameters

        /// <summary>
        /// Job ID.
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
            RecoveryServicesClient.RestartAzureSiteRecoveryJob(Id);
        }
    }
}