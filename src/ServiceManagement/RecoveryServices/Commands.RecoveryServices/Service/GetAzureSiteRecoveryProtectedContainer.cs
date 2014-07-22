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
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using System;
    using System.Management.Automation;
    using System.Collections.Generic;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryProtectedContainer", DefaultParameterSetName = Default)]
    [OutputType (typeof(PSCloud))]
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
            CloudListResponse cloudListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryCloud(serverId);

            bool found = false;
            foreach (Cloud cloud in cloudListResponse.Clouds)
            {
                if (0 == string.Compare(name, cloud.Name, true))
                {
                    WriteCloud(cloud);
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
            CloudResponse cloudResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryCloud(serverId, id);

            WriteCloud(cloudResponse.Cloud);
        }

        private void GetByDefault()
        {
            CloudListResponse cloudListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryCloud(serverId);

            WriteClouds(cloudListResponse.Clouds);
        }

        private void WriteClouds(IList<Cloud> clouds)
        {
            foreach (Cloud cloud in clouds)
            {
                WriteCloud(cloud);
            }
        }

        private void WriteCloud(Cloud cloud)
        {
            WriteObject(
                new PSCloud(
                    cloud.Id,
                    cloud.Name,
                    cloud.Type,
                    cloud.Configured,
                    cloud.ReplicationProvider,
                    cloud.ReplicationProviderSettings,
                    cloud.ServerId));
        }
    }
}