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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public const string PrimaryToSecondary = "PrimaryToSecondary";
        public const string SecondaryToPrimary = "SecondaryToPrimary";

        public RecoveryPlanListResponse GetAzureSiteRecoveryRecoveryPlan()
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.List(this.GetRequestHeaders());
        }

        public RecoveryPlanResponse GetAzureSiteRecoveryRecoveryPlan(string recoveryPlanId)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.Get(recoveryPlanId, this.GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryCommitFailover(string recoveryPlanId)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.Commit(recoveryPlanId, this.GetRequestHeaders());
        }

        public JobResponse UpdateAzureSiteRecoveryProtection(string recoveryPlanId)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.Reprotect(recoveryPlanId, this.GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryPlannedFailover(
            string recoveryPlanId, 
            RpPlannedFailoverRequest recoveryPlanPlannedFailoverRequest)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanPlannedFailover(
                recoveryPlanId,
                recoveryPlanPlannedFailoverRequest, 
                this.GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryUnplannedFailover(
            string recoveryPlanId,
            RpUnplannedFailoverRequest recoveryPlanUnPlannedFailoverRequest)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanUnplannedFailover(
                recoveryPlanId,
                recoveryPlanUnPlannedFailoverRequest, 
                this.GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryTestFailover(
            string recoveryPlanId,
            RpTestFailoverRequest recoveryPlanTestFailoverRequest)
        {
            return this.GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanTestFailover(
                recoveryPlanId,
                recoveryPlanTestFailoverRequest, 
                this.GetRequestHeaders());
        }
    }
}