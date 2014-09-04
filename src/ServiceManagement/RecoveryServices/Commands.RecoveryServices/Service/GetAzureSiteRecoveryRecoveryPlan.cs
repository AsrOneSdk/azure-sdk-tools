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
    using System.Collections.Generic;
    using System.Management.Automation;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Retrieves Azure Site Recovery Recovery Plan.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryRecoveryPlan", DefaultParameterSetName = Default)]
    [OutputType(typeof(IEnumerable<ASRRecoveryPlan>))]
    public class GetAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        /// <summary>
        /// When nothing is passed to the command.
        /// </summary>
        protected const string Default = "Default";

        /// <summary>
        /// When only Name is passed to the command.
        /// </summary>
        protected const string ByName = "ByName";

        /// <summary>
        /// When only ID is passed to the command.
        /// </summary>
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// Recovery Plan ID.
        /// </summary>
        private string id;

        /// <summary>
        /// Name of the Recovery Plan.
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets Recovery Plan ID.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets name of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
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
                    case ByName:
                        this.GetByName();
                        break;
                    case ById:
                        this.GetById();
                        break;
                    case Default:
                        this.GetByDefault();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Queries by name.
        /// </summary>
        private void GetByName()
        {
            RecoveryPlanListResponse recoveryPlanListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan();

            bool found = false;
            foreach (RecoveryPlan recoveryPlan in recoveryPlanListResponse.RecoveryPlans)
            {
                if (0 == string.Compare(this.name, recoveryPlan.Name, true))
                {
                    this.WriteRecoveryPlan(recoveryPlan);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.RecoveryPlanNotFound,
                    this.name,
                    PSRecoveryServicesClient.asrVaultCreds.ResourceName));
            }
        }

        /// <summary>
        /// Queries by ID.
        /// </summary>
        private void GetById()
        {
            RecoveryPlanResponse recoveryPlanResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan(this.id);

            this.WriteRecoveryPlan(recoveryPlanResponse.RecoveryPlan);
        }

        /// <summary>
        /// Queries all / by default.
        /// </summary>
        private void GetByDefault()
        {
            RecoveryPlanListResponse recoveryPlanListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan();

            this.WriteRecoveryPlans(recoveryPlanListResponse.RecoveryPlans);
        }

        /// <summary>
        /// Writes Recovery Plans.
        /// </summary>
        /// <param name="recoveryPlans">List of Recovery Plans</param>
        private void WriteRecoveryPlans(IList<RecoveryPlan> recoveryPlans)
        {
            foreach (RecoveryPlan recoveryPlan in recoveryPlans)
            {
                this.WriteRecoveryPlan(recoveryPlan);
            }
        }

        /// <summary>
        /// Writes Recovery Plan.
        /// </summary>
        /// <param name="recoveryPlan">Recovery Plan</param>
        private void WriteRecoveryPlan(RecoveryPlan recoveryPlan)
        {
            this.WriteObject(
                new ASRRecoveryPlan(
                    recoveryPlan.ID,
                    recoveryPlan.Name,
                    recoveryPlan.ServerId,
                    recoveryPlan.TargetServerId,
                    recoveryPlan.RecoveryPlanXml),
                true);
        }
    }
}