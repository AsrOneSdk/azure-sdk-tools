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
    /// Retrieves Azure Site Recovery Protection Entity.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectionEntity", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(IEnumerable<ASRProtectionEntity>))]
    public class GetAzureSiteRecoveryProtectionEntity : RecoveryServicesCmdletBase
    {
        #region Parameters
        private string id;
        private string name;
        private string protectionContainerId;
        private ASRProtectionContainer protectionContainer;

        /// <summary>
        /// ID of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithId, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Name of the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// ID of the ProtectionContainer containing the Virtual Machine.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByIDs, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByIDsWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProtectionContainerId
        {
            get { return this.protectionContainerId; }
            set { this.protectionContainerId = value; }
        }

        /// <summary>
        /// Server Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRProtectionContainer ProtectionContainer
        {
            get { return this.protectionContainer; }
            set { this.protectionContainer = value; }
        }
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (this.ParameterSetName)
                {
                    case ASRParameterSets.ByObject:
                    case ASRParameterSets.ByObjectWithId:
                    case ASRParameterSets.ByObjectWithName:
                        this.protectionContainerId = this.ProtectionContainer.ProtectionContainerId;
                        break;
                    case ASRParameterSets.ByIDs:
                    case ASRParameterSets.ByIDsWithId:
                    case ASRParameterSets.ByIDsWithName:
                        break;
                }

                if (this.id != null)
                {
                    this.GetById();
                }
                else if (this.name != null)
                {
                    this.GetByName();
                }
                else
                {
                    this.GetAll();
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetByName()
        {
            ProtectionEntityListResponse protectionEntityListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                this.protectionContainerId);

            bool found = false;
            foreach (ProtectionEntity pe in protectionEntityListResponse.ProtectionEntities)
            {
                if (0 == string.Compare(this.name, pe.Name, true))
                {
                    this.WriteProtectionEntity(pe);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineNotFound,
                    this.name,
                    this.protectionContainerId));
            }
        }

        private void GetById()
        {
            ProtectionEntityResponse protectionEntityResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                this.protectionContainerId,
                this.id);

            this.WriteProtectionEntity(protectionEntityResponse.ProtectionEntity);
        }

        private void GetAll()
        {
            ProtectionEntityListResponse protectionEntityListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                this.protectionContainerId);

            this.WriteProtectionEntities(protectionEntityListResponse.ProtectionEntities);
        }

        private void WriteProtectionEntities(IList<ProtectionEntity> protectionEntities)
        {
            foreach (ProtectionEntity pe in protectionEntities)
            {
                this.WriteProtectionEntity(pe);
            }
        }

        private void WriteProtectionEntity(ProtectionEntity pe)
        {
            this.WriteObject(
                new ASRProtectionEntity(
                    pe.ID,
                    pe.ServerId,
                    pe.ProtectionContainerId,
                    pe.Name,
                    pe.Type,
                    pe.FabricObjectId,
                    pe.Protected,
                    pe.CanCommit,
                    pe.CanFailover,
                    pe.CanReverseReplicate,
                    pe.IsRelationshipReversed,
                    pe.ProtectionState,
                    pe.TestFailoverState,
                    pe.ReplicationHealth,
                    pe.ReplicationProvider),
                true);
        }
    }
}