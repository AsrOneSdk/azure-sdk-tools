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
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using System;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public const string PrimaryToSecondary = "PrimaryToSecondary";
        public const string SecondaryToPrimary = "SecondaryToPrimary";

        public RecoveryPlanListResponse GetAzureSiteRecoveryRecoveryPlan()
        {
            return GetSiteRecoveryClient().RecoveryPlan.List(GetRequestHeaders());
        }

        public RecoveryPlanResponse GetAzureSiteRecoveryRecoveryPlan(string recoveryPlanId)
        {
            return GetSiteRecoveryClient().RecoveryPlan.Get(recoveryPlanId, GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryCommitFailover(string recoveryPlanId)
        {
            return GetSiteRecoveryClient().RecoveryPlan.Commit(recoveryPlanId, GetRequestHeaders());
        }

        public JobResponse UpdateAzureSiteRecoveryProtection(string recoveryPlanId)
        {
            return GetSiteRecoveryClient().RecoveryPlan.Reprotect(recoveryPlanId, GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryPlannedFailover(
            string recoveryPlanId, 
            RpPlannedFailoverRequest rpPlannedFailoverRequest)
        {
            return GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanPlannedFailover(
                recoveryPlanId,
                rpPlannedFailoverRequest, GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryUnplannedFailover(
            string recoveryPlanId,
            RpUnplannedFailoverRequest rpUnPlannedFailoverRequest)
        {
            return GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanUnplannedFailover(
                recoveryPlanId,
                rpUnPlannedFailoverRequest, 
                GetRequestHeaders());
        }

        public JobResponse StartAzureSiteRecoveryTestFailover(
            string recoveryPlanId,
            RpTestFailoverRequest rpTestFailoverRequest)
        {
            return GetSiteRecoveryClient().RecoveryPlan.RecoveryPlanTestFailover(
                recoveryPlanId,
                rpTestFailoverRequest, 
                GetRequestHeaders());
        }
    }
}