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
    using Microsoft.WindowsAzure;
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryRecoveryPlan", DefaultParameterSetName = Default)]
    [OutputType(typeof(IEnumerable<ASRRecoveryPlan>))]
    public class GetAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// ID of the Server.
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
        /// Name of the Server.
        /// </summary>
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private string name;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ByName:
                        GetByName();
                        break;
                    case ById:
                        GetById();
                        break;
                    case Default:
                        GetByDefault();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetByName()
        {
            RecoveryPlanListResponse recoveryPlanListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan();

            bool found = false;
            foreach (RecoveryPlan recoveryPlan in recoveryPlanListResponse.RecoveryPlans)
            {
                if(0 == string.Compare(name, recoveryPlan.Name, true))
                {
                    WriteRecoveryPlan(recoveryPlan);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.RecoveryPlanNotFound,
                    name,
                    PSRecoveryServicesClient.resourceCredentials.ResourceName));
            }
        }

        private void GetById()
        {
            RecoveryPlanResponse recoveryPlanResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan(id);

            WriteRecoveryPlan(recoveryPlanResponse.RecoveryPlan);
        }

        private void GetByDefault()
        {
            RecoveryPlanListResponse recoveryPlanListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryRecoveryPlan();

            WriteRecoveryPlans(recoveryPlanListResponse.RecoveryPlans);
        }

        private void WriteRecoveryPlans(IList<RecoveryPlan> recoveryPlans)
        {
            foreach (RecoveryPlan recoveryPlan in recoveryPlans)
            {
                WriteRecoveryPlan(recoveryPlan);
            }
        }

        private void WriteRecoveryPlan(RecoveryPlan recoveryPlan)
        {
            WriteObject(
                new ASRRecoveryPlan(
                    recoveryPlan.ID,
                    recoveryPlan.Name,
                    recoveryPlan.Type,
                    recoveryPlan.ServerId,
                    recoveryPlan.TargetServerId),
                true);
        }
    }
}