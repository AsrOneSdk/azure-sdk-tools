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
    using System.Management.Automation;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryVirtualMachine", DefaultParameterSetName = None)]
    public class GetAzureSiteRecoveryVirtualMachine : RecoveryServicesCmdletBase
    {
        protected const string None = "None";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// GUID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        private System.Guid id;

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
        /// GUID of the ProtectedContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById)]
        [Parameter(ParameterSetName = ByName)]
        [Parameter(ParameterSetName = None, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid ProtectedContianer
        {
            get { return this.protectedcontianer; }
            set { this.protectedcontianer = value; }
        }
        private System.Guid protectedcontianer;

        /// <summary>
        /// GUID of the Server managing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ById)]
        [Parameter(ParameterSetName = ByName)]
        [Parameter(ParameterSetName = None, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public System.Guid Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        private System.Guid server;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            if (protectedcontianer != Guid.Empty)
            {
                WriteObject("Protected container value present: " + protectedcontianer);
            }
            if (server != Guid.Empty)
            {
                WriteObject("Server value present: " + server);
            }
            switch(ParameterSetName)
            {
                case ByName:
                    WriteObject("ByName: " + name);
                    break;
                case ById:
                    WriteObject("ById: " + id);
                    break;
                case None:
                    WriteObject("none");
                    break;
            }
        }
    }
}