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
    using System.Management.Automation;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryRecoveryPlan", DefaultParameterSetName = Default)]
    public class GetAzureSiteRecoveryRecoveryPlan : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// ID of the Virtual Machine.
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
        /// ID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public string ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }
        private string serverId;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            switch(ParameterSetName)
            {
                case ByName:
                    WriteObject("ByName: " + name);
                    break;
                case ById:
                    WriteObject("ById: " + id);
                    break;
                case Default:
                    WriteObject("none");
                    break;
            }
        }
    }
}