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
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public JobResponse GetAzureSiteRecoveryJobDetails(string jobId)
        {
            return this.GetSiteRecoveryClient().Jobs.Get(jobId, this.GetRequestHeaders());
        }

        public JobListResponse GetAzureSiteRecoveryJob()
        {
            return this.GetSiteRecoveryClient().Jobs.List(this.GetRequestHeaders());
        }

        public void StopAzureSiteRecoveryJob(
            string jobId)
        {
            this.GetSiteRecoveryClient().Jobs.Cancel(jobId, this.GetRequestHeaders());
        }

        public JobResponse RestartAzureSiteRecoveryJob(
            string jobId)
        {
            return this.GetSiteRecoveryClient().Jobs.Restart(jobId, this.GetRequestHeaders());
        }

        public JobResponse ResumeAzureSiteRecoveryJob(
            string jobId,
            ResumeJobParams resumeJobParams)
        {
            return this.GetSiteRecoveryClient().Jobs.Resume(jobId, resumeJobParams, this.GetRequestHeaders());
        }
    }
}