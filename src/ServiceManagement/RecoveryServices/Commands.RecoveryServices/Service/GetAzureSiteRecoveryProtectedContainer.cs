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
    using System;
    using System.Management.Automation;
    using System.Collections.Generic;
    using Microsoft.Azure.Management.SiteRecovery.Models;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectedContainer", DefaultParameterSetName = Default)]
    [OutputType (typeof(PSProtectedContainer))]
    public class GetAzureSiteRecoveryProtectedContainer : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// ID of the Protected Container.
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
        /// Name of the Protected Container.
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
        /// ID of the Protected Container management server.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = ByName, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = Default, Mandatory = true, ValueFromPipelineByPropertyName = true)]
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
            try
            {
                switch (ParameterSetName)
                {
                    case ByName:
                        GetByName();
                        break;
                    case ById:
                        GetById();
                        break;
                    case Default:
                        GetByDefault();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetByName()
        {
            ProtectedContainerListResponse protectedContainerListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectedContainer(serverId);

            bool found = false;
            foreach (
                ProtectedContainer protectedContainer in 
                protectedContainerListResponse.ProtectedContainers)
            {
                if (0 == string.Compare(name, protectedContainer.Name, true))
                {
                    WriteProtectedContainer(protectedContainer);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.ProtectedContainerNotFound,
                    name,
                    serverId));
            }
        }

        private void GetById()
        {
            ProtectedContainerResponse protectedContainerResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectedContainer(serverId, id);

            WriteProtectedContainer(protectedContainerResponse.ProtectedContainer);
        }

        private void GetByDefault()
        {
            ProtectedContainerListResponse protectedContainerListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectedContainer(serverId);

            WriteProtectedContainers(protectedContainerListResponse.ProtectedContainers);
        }

        private void WriteProtectedContainers (IList<ProtectedContainer> protectedContainers)
        {
            foreach (ProtectedContainer protectedContainer in protectedContainers)
            {
                WriteProtectedContainer(protectedContainer);
            }
        }

        private void WriteProtectedContainer (ProtectedContainer protectedContainer)
        {
            WriteObject(
                new PSProtectedContainer(
                    protectedContainer.ID,
                    protectedContainer.Name,
                    protectedContainer.Type,
                    protectedContainer.Configured,
                    protectedContainer.ReplicationProvider,
                    protectedContainer.ReplicationProviderSettings,
                    protectedContainer.ServerId));
        }
    }
}