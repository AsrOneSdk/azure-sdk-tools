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

    public class ASRRecoveryPlan
    {
         #region Properties
        public string RpId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ServerId { get; set; }
        public string TargetServerId { get; set; }
        #endregion

        public ASRRecoveryPlan() { }
        public ASRRecoveryPlan(
            string rpId,
            string name,
            string serverId,
            string targetServerId)
        {
            this.RpId = rpId;
            this.Name = name;
            this.ServerId = serverId;
            this.TargetServerId = targetServerId;
        }
    }
}