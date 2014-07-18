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
    using Microsoft.Azure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryRecoveryPlan", DefaultParameterSetName = None)]
    public class GetAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        protected const string None = "None";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// GUID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private System.Guid id;

        /// <summary>
        /// Name of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private string name;

        /// <summary>
        /// GUID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public System.Guid Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        private System.Guid server;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            switch (ParameterSetName)
            {
                case ByName:
                    GetByName();
                    break;
                case ById:
                    GetById();
                    break;
                case None:
                    GetByDefault();
                    break;
            }
        }

        private void GetByName()
        {
            WriteObject("No API Yet.");
        }

        private void GetById()
        {
        }

        private void GetByDefault()
        {
            RecoveryPlanListResponse recoveryPlanList =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan();
            if (recoveryPlanList == null)
            {
                WriteObject("Error");
            }
            else
            {
                foreach (RecoveryPlan recoveryPlan in recoveryPlanList.RecoveryPlans)
                {
                    WriteObject(recoveryPlan);
                }
            }
        }
    }
}