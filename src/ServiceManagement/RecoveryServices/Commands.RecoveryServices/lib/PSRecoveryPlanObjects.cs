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
    using System.Runtime.Serialization;
    #endregion

    /// <summary>
    /// Azure Site Recovery Recovery Plan.
    /// </summary>
    public class ASRRecoveryPlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASRRecoveryPlan" /> class.
        /// </summary>
        public ASRRecoveryPlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASRRecoveryPlan" /> class with required
        /// parameters.
        /// </summary>
        /// <param name="recoveryPlanId">Recovery plan ID</param>
        /// <param name="name">Name of the Recovery plan</param>
        /// <param name="serverId">Server ID</param>
        /// <param name="targetServerId">Target Server ID</param>
        /// <param name="recoveryPlanXml">Recovery plan xml</param>
        public ASRRecoveryPlan(
            string recoveryPlanId,
            string name,
            string serverId,
            string targetServerId,
            string recoveryPlanXml)
        {
            this.RpId = recoveryPlanId;
            this.Name = name;
            this.ServerId = serverId;
            this.TargetServerId = targetServerId;
            this.RecoveryPlanXml = recoveryPlanXml;
        }

        #region Properties
        /// <summary>
        /// Gets or sets Recovery plan ID.
        /// </summary>
        public string RpId { get; set; }

        /// <summary>
        /// Gets or sets name of the Recovery Plan.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets type of the Recovery Plan.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets to Server ID.
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// Gets or sets target Server ID.
        /// </summary>
        public string TargetServerId { get; set; }

        /// <summary>
        /// Gets or sets recovery plan xml. The value for RecoveryPlanXml will be set only if we 
        /// use the powershell cmdlet with id parameter 'Get-AzureSiteRecoveryRecoveryPlan -id'.
        /// </summary>
        public string RecoveryPlanXml { get; set; }
        
        #endregion
    }
}