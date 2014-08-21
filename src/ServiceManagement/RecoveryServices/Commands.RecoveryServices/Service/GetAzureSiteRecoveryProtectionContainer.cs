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
    using System.Collections.Generic;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Retrieves Azure Site Recovery Protection Container.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectionContainer", DefaultParameterSetName = Default)]
    [OutputType(typeof(IEnumerable<ASRProtectionContainer>))]
    public class GetAzureSiteRecoveryProtectionContainer : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        private string id;
        private string name;

        /// <summary>
        /// ID of the Protection Container.
        /// </summary>
        [Parameter(ParameterSetName = ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Name of the Protection Container.
        /// </summary>
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ByName:
                        this.GetByName();
                        break;
                    case ById:
                        this.GetById();
                        break;
                    case Default:
                        this.GetByDefault();
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
            ProtectionContainerListResponse protectionContainerListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionContainer();

            bool found = false;
            foreach (
                ProtectionContainer protectionContainer in 
                protectionContainerListResponse.ProtectionContainers)
            {
                if (0 == string.Compare(this.name, protectionContainer.Name, true))
                {
                    this.WriteProtectionContainer(protectionContainer);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.ProtectionContainerNotFound,
                    this.name));
            }
        }

        private void GetById()
        {
            ProtectionContainerResponse protectionContainerResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionContainer(this.id);

            this.WriteProtectionContainer(protectionContainerResponse.ProtectionContainer);
        }

        private void GetByDefault()
        {
            ProtectionContainerListResponse protectionContainerListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionContainer();

            this.WriteProtectionContainers(protectionContainerListResponse.ProtectionContainers);
        }

        private void WriteProtectionContainers(IList<ProtectionContainer> protectionContainers)
        {
            foreach (ProtectionContainer protectionContainer in protectionContainers)
            {
                this.WriteProtectionContainer(protectionContainer);
            }
        }

        private void WriteProtectionContainer(ProtectionContainer protectionContainer)
        {
            this.WriteObject(
                new ASRProtectionContainer(
                    protectionContainer.ID,
                    protectionContainer.Name,
                    protectionContainer.ConfigurationStatus,
                    protectionContainer.ReplicationProviderSettings,
                    protectionContainer.ServerId),
                true);
        }
    }
}