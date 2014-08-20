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
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectionEntity", DefaultParameterSetName = ASRParameterSets.ByObject)]
    [OutputType(typeof(IEnumerable<ASRProtectionEntity>))]
    public class GetAzureSiteRecoveryProtectedContainer : RecoveryServicesCmdletBase
    {
        #region Parameters
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
        private string id;

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
        private string name;

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
        private string protectionContainerId;

        private string serverId;

        /// <summary>
        /// Server Object.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByObject, Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithId, Mandatory = true)]
        [Parameter(ParameterSetName = ASRParameterSets.ByObjectWithName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ASRServer Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        private ASRServer server;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            try
            {
                switch (ParameterSetName)
                {
                    case ASRParameterSets.ByObject:
                    case ASRParameterSets.ByObjectWithId:
                    case ASRParameterSets.ByObjectWithName:
                        serverId = server.ServerId;
                        break;
                    case ASRParameterSets.ByIDs:
                    case ASRParameterSets.ByIDsWithId:
                    case ASRParameterSets.ByIDsWithName:
                        break;
                }

                if (id != null)
                {
                    GetById();
                }
                else if (name != null)
                {
                    GetByName();
                }
                else
                {
                    GetAll();
                }
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        private void GetByName()
        {
            ProtectionEntityListResponse peListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                protectionContainerId);

            bool found = false;
            foreach (ProtectionEntity pe in peListResponse.ProtectionEntities)
            {
                if (0 == string.Compare(name, pe.Name, true))
                {
                    WriteProtectionEntity(pe);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.VirtualMachineNotFound,
                    name,
                    protectionContainerId));
            }
        }

        private void GetById()
        {
            ProtectionEntityResponse peResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                protectionContainerId, 
                id);

            WriteProtectionEntity(peResponse.ProtectionEntity);
        }

        private void GetAll()
        {
            ProtectionEntityListResponse peListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryProtectionEntity(
                protectionContainerId);

            WriteProtectionEntities(peListResponse.ProtectionEntities);
        }

        private void WriteProtectionEntities(IList<ProtectionEntity> protectionEntities)
        {
            foreach (ProtectionEntity pe in protectionEntities)
            {
                WriteProtectionEntity(pe);
            }
        }

        private void WriteProtectionEntity(ProtectionEntity pe)
        {
            WriteObject(
                new ASRProtectionEntity(
                    pe.ID,
                    pe.ServerId,
                    pe.ProtectionContainerId,
                    pe.Name,
                    pe.Protected,
                    pe.IsRelationshipReversed,
                    pe.ProtectionState,
                    pe.TestFailoverState,
                    pe.ReplicationProvider),
                true);
        }
    }
}