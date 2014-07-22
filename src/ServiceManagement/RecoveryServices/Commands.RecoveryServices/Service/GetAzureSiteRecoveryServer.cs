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
    using System.Collections.Generic;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureSiteRecoveryServer", DefaultParameterSetName = Default)]
    [OutputType(typeof(IEnumerable<PSServer>))]
    public class GetAzureSiteRecoveryServer : RecoveryServicesCmdletBase
    {
        protected const string Default = "Default";
        protected const string ByName = "ByName";
        protected const string ById = "ById";

        #region Parameters
        /// <summary>
        /// ID of the Server.
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
        /// Name of the Server.
        /// </summary>
        [Parameter(ParameterSetName = ByName, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private string name;
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
            ServerListResponse serverListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryServer();

            bool found = false;
            foreach (Server server in serverListResponse.Servers)
            {
                if(0 == string.Compare(name, server.Name, true))
                {
                    WriteServer(server);
                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                    Properties.Resources.ServerNotFound,
                    name,
                    CurrentSubscription.AzureSiteRecoveryResourceName));
            }
        }

        private void GetById()
        {
            ServerResponse serverResponse = 
                RecoveryServicesClient.GetAzureSiteRecoveryServer(id);

            WriteServer(serverResponse.Server);
        }

        private void GetByDefault()
        {
            ServerListResponse serverListResponse =
                RecoveryServicesClient.GetAzureSiteRecoveryServer();

            WriteServers(serverListResponse.Servers);
        }

        private void WriteServers(IList<Server> servers)
        {
            foreach (Server server in servers)
            {
                WriteServer(server);
            }
        }

        private void WriteServer(Server server)
        {
            WriteObject(
                new PSServer(
                    server.Id,
                    server.Name,
                    server.Type,
                    server.LastHeartbeat,
                    server.ProviderVersion,
                    server.ServerVersion));
        }
    }
}