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
    using Microsoft.WindowsAzure;
    using Microsoft.Azure.Management.SiteRecovery.Models;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public JobResponse GetAzureSiteRecoveryJobDetails(string jobId)
        {
            return GetSiteRecoveryClient().Jobs.Get(jobId, GetRequestHeaders());
        }

        public JobListResponse GetAzureSiteRecoveryJob()
        {
            return GetSiteRecoveryClient().Jobs.List(GetRequestHeaders());
        }

        public void StopAzureSiteRecoveryJob(
            string jobId
            )
        {
            GetSiteRecoveryClient().Jobs.Cancel(jobId, GetRequestHeaders());
        }

        public JobResponse RestartAzureSiteRecoveryJob(
            string jobId)
        {
            return GetSiteRecoveryClient().Jobs.Restart(jobId, GetRequestHeaders());
        }

        public JobResponse ResumeAzureSiteRecoveryJob(
            string jobId,
            ResumeJobParams resumeJobParams)
        {
            return GetSiteRecoveryClient().Jobs.Resume(jobId, resumeJobParams, GetRequestHeaders());
        }
    }
}