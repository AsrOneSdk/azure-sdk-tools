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
    using System.Collections.Generic;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Retrieves Azure Site Recovery Protection Container.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectionContainer", DefaultParameterSetName = ASRParameterSets.Default)]
    [OutputType(typeof(IEnumerable<ASRProtectionContainer>))]
    public class GetAzureSiteRecoveryProtectionContainer : RecoveryServicesCmdletBase
    {
        #region Parameters

        /// <summary>
        /// Protection container ID.
        /// </summary>
        private string id;

        /// <summary>
        /// Name of the Protection container.
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets ID of the Protection Container.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ById, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets name of the Protection Container.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
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
                    case ASRParameterSets.ByName:
                        this.GetByName();
                        break;
                    case ASRParameterSets.ById:
                        this.GetById();
                        break;
                    case ASRParameterSets.Default:
                        this.GetByDefault();
                        break;
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        /// <summary>
        /// Queries by name.
        /// </summary>
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
                    this.name,
                    PSRecoveryServicesClient.asrVaultCreds.ResourceName));
            }
        }

        /// <summary>
        /// Queries by ID.
        /// </summary>
        private void GetById()
        {
            ProtectionContainerResponse protectionContainerResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionContainer(this.id);

            this.WriteProtectionContainer(protectionContainerResponse.ProtectionContainer);
        }

        /// <summary>
        /// Queries all, by default.
        /// </summary>
        private void GetByDefault()
        {
            ProtectionContainerListResponse protectionContainerListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionContainer();

            this.WriteProtectionContainers(protectionContainerListResponse.ProtectionContainers);
        }

        /// <summary>
        /// Writes Protection Containers.
        /// </summary>
        /// <param name="protectionContainers">List of Protection Containers</param>
        private void WriteProtectionContainers(IList<ProtectionContainer> protectionContainers)
        {
            foreach (ProtectionContainer protectionContainer in protectionContainers)
            {
                this.WriteProtectionContainer(protectionContainer);
            }
        }

        /// <summary>
        /// Write Protection Container.
        /// </summary>
        /// <param name="protectionContainer">Protection Container</param>
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