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
    using System;
    using System.Diagnostics;
    using System.Management.Automation;
    using System.Threading;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Used to initiate a commit operation.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "AzureSiteRecoveryTestFailover", DefaultParameterSetName = ASRParameterSets.ByRPObject)]
    [OutputType(typeof(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job))]
    public class StartAzureSiteRecoveryTestFailover : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// ID of the Recovery Plan.
        /// </summary>
        private string recoveryPlanId;

        /// <summary>
        /// Recovery Plan object.
        /// </summary>
        private ASRRecoveryPlan recoveryPlan;

        /// <summary>
        /// Failover direction for the recovery plan.
        /// </summary>
        private string failoverDirection;

        /// <summary>
        /// ID of the PE object to start failover on.
        /// </summary>
        private string protectionEntityId;

        /// <summary>
        /// Protection container ID of the object to start failover on.
        /// </summary>
        private string protectionContainerId;

        /// <summary>
        /// Recovery Plan object.
        /// </summary>
        private ASRProtectionEntity protectionEntity;

        /// <summary>
        /// This is required to wait for job completion.
        /// </summary>
        private bool waitForCompletion;

        /// <summary>
        /// Job response.
        /// </summary>
        private JobResponse jobResponse = null;

        /// <summary>
        /// Stop processing, enables on pressing Ctrl-C.
        /// </summary>
        private bool stopProcessing = false;

        /// <summary>
        /// Network ID.
        /// </summary>
        private string networkId = string.Empty;

        /// <summary>
        /// Network Type (Logical network or VM network).
        /// </summary>
        private string networkType = string.Empty;

        /// <summary>
        /// Gets or sets ID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByRPId, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string RpId
        {
            get { return this.recoveryPlanId; }
            set { this.recoveryPlanId = value; }
        }

        /// <summary>
        /// Gets or sets Recovery Plan object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByRPObject, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public ASRRecoveryPlan RecoveryPlan
        {
            get { return this.recoveryPlan; }
            set { this.recoveryPlan = value; }
        }

        /// <summary>
        /// Gets or sets failover direction for the recovery plan.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByRPObject, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByRPId, Mandatory = true)]
        [ValidateSet(
          PSRecoveryServicesClient.PrimaryToSecondary,
          PSRecoveryServicesClient.SecondaryToPrimary)]
        public string FailoverDirection
        {
            get { return this.failoverDirection; }
            set { this.failoverDirection = value; }
        }

        /// <summary>
        /// Gets or sets ID of the PE.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByPEId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithLogicalNetworkID, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithVMNetworkID, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectionEntityId
        {
            get { return this.protectionEntityId; }
            set { this.protectionEntityId = value; }
        }

        /// <summary>
        /// Gets or sets ID of the Recovery Plan.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByPEId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithLogicalNetworkID, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithVMNetworkID, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectionContainerId
        {
            get { return this.protectionContainerId; }
            set { this.protectionContainerId = value; }
        }

        /// <summary>
        /// Gets or sets Protection Entity object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByPEObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEObjectWithLogicalNetworkID, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEObjectWithVMNetworkID, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionEntity ProtectionEntity
        {
            get { return this.protectionEntity; }
            set { this.protectionEntity = value; }
        }

        /// <summary>
        /// Gets or sets switch parameter. This is required to wait for job completion.
        /// </summary>
        [Parameter]
        public SwitchParameter WaitForCompletion
        {
            get { return this.waitForCompletion; }
            set { this.waitForCompletion = value; }
        }

        /// <summary>
        /// Gets or sets Logical network ID.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByPEObjectWithLogicalNetworkID, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithLogicalNetworkID, Mandatory = true)]
        public string LogicalNetworkId
        {
            get { return this.networkId; }
            set { this.networkId = value; }
        }

        /// <summary>
        /// Gets or sets VM network ID.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByPEObjectWithVMNetworkID, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByPEIdWithVMNetworkID, Mandatory = true)]
        public string VmNetworkId
        {
            get { return this.networkId; }
            set { this.networkId = value; }
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
                    case ASRParameterSets.ByRPObject:
                        this.recoveryPlanId = this.recoveryPlan.RpId;
                        this.StartRpTestFailover();
                        break;
                    case ASRParameterSets.ByRPId:
                        this.StartRpTestFailover();
                        break;
                    case ASRParameterSets.ByPEObject:
                        this.networkType = "DisconnectedVMNetworkTypeForTestFailover";
                        this.UpdateRequiredParametersAndStartFailover();
                        break;
                    case ASRParameterSets.ByPEObjectWithLogicalNetworkID:
                        this.networkType = "CreateVMNetworkTypeForTestFailover";
                        this.UpdateRequiredParametersAndStartFailover();
                        break;
                    case ASRParameterSets.ByPEObjectWithVMNetworkID:
                        this.networkType = "UseVMNetworkTypeForTestFailover";
                        this.UpdateRequiredParametersAndStartFailover();
                        break;
                    case ASRParameterSets.ByPEId:
                        this.StartPETestFailover();
                        break;
                    case ASRParameterSets.ByPEIdWithLogicalNetworkID:
                        this.networkType = "CreateVMNetworkTypeForTestFailover";
                        this.StartPETestFailover();
                        break;
                    case ASRParameterSets.ByPEIdWithVMNetworkID:
                        this.networkType = "UseVMNetworkTypeForTestFailover";
                        this.StartPETestFailover();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Handles interrupts.
        /// </summary>
        protected override void StopProcessing()
        {
            // Ctrl + C and etc
            base.StopProcessing();
            this.stopProcessing = true;
        }

        /// <summary>
        /// Starts RP test failover.
        /// </summary>
        private void StartRpTestFailover()
        {
            RpTestFailoverRequest recoveryPlanTestFailoverRequest = new RpTestFailoverRequest();
            recoveryPlanTestFailoverRequest.FailoverDirection = this.FailoverDirection;
            this.jobResponse = RecoveryServicesClient.StartAzureSiteRecoveryTestFailover(
                this.RpId, 
                recoveryPlanTestFailoverRequest);

            this.WriteJob(this.jobResponse.Job);

            string jobId = this.jobResponse.Job.ID;
            while (this.waitForCompletion)
            {
                if (this.jobResponse.Job.Completed || this.stopProcessing)
                {
                    break;
                }

                Thread.Sleep(PSRecoveryServicesClient.TimeToSleepBeforeFetchingJobDetailsAgain);
                this.jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(this.jobResponse.Job.ID);
                this.WriteObject("JobState: " + this.jobResponse.Job.State);
            }
        }

        /// <summary>
        /// Starts PE Test failover.
        /// </summary>
        private void StartPETestFailover()
        {
            var tfoReqeust = new TestFailoverRequest();
            tfoReqeust.NetworkID = this.networkId;
            tfoReqeust.NetworkType = this.networkType;
            tfoReqeust.ReplicationProvider = string.Empty;
            tfoReqeust.ReplicationProviderSettings = string.Empty;

            this.jobResponse =
                RecoveryServicesClient.StartAzureSiteRecoveryTestFailover(
                this.protectionContainerId,
                this.ProtectionEntityId,
                tfoReqeust);
            this.WriteJob(this.jobResponse.Job);

            string jobId = this.jobResponse.Job.ID;
            while (this.waitForCompletion)
            {
                if (this.jobResponse.Job.Completed || this.stopProcessing)
                {
                    break;
                }

                Thread.Sleep(PSRecoveryServicesClient.TimeToSleepBeforeFetchingJobDetailsAgain);
                this.jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(this.jobResponse.Job.ID);
                this.WriteObject("JobState: " + this.jobResponse.Job.State);
            }
        }

        /// <summary>
        /// Updates required parameters and starts test failover.
        /// </summary>
        private void UpdateRequiredParametersAndStartFailover()
        {
            if (!this.protectionEntity.Protected)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.ProtectionEntityNotProtected,
                    this.protectionEntity.Name));
            }

            this.protectionContainerId = this.protectionEntity.ProtectionContainerId;
            this.protectionEntityId = this.protectionEntity.ID;
            this.StartPETestFailover();
        }

        /// <summary>
        /// Writes Job.
        /// </summary>
        /// <param name="job">JOB object</param>
        private void WriteJob(Microsoft.WindowsAzure.Management.SiteRecovery.Models.Job job)
        {
            this.WriteObject(new ASRJob(job));
        }
    }
}