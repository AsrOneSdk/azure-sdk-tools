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
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using System;
    using System.Management.Automation;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AzureSiteRecoveryProtection")]
    public class SetAzureSiteRecoveryProtection : RecoveryServicesCmdletBase
    {

        #region Parameters
        /// <summary>
        /// ID of the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string VM
        {
            get { return this.vm; }
            set { this.vm = value; }
        }
        private string vm;

        /// <summary>
        /// ID of the ProtectedContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectedContianerId
        {
            get { return this.protectedContainerId; }
            set { this.protectedContainerId = value; }
        }
        private string protectedContainerId;

        /// <summary>
        /// ID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }
        private string serverId;

        /// <summary>
        /// Bool value to either to say either enable or disable protection.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public bool Protection
        {
            get { return this.protection; }
            set { this.protection = value; }
        }
        private bool protection;

        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
            }
            catch (CloudException cloudException)
            {
                // Log errors from SRS (good to deserialize the Error Message & print as object)
                WriteObject("ErrorCode: " + cloudException.ErrorCode);
                WriteObject("ErrorMessage: " + cloudException.ErrorMessage);
            }
        }
    }
}