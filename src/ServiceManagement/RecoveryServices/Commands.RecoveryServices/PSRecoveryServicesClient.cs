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
    using Microsoft.Azure.Management.SiteRecovery;
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using System;
    using System.Security.Cryptography.X509Certificates;
    #endregion

    class PSRecoveryServiceClient
    {
        private RecoveryServicesManagementClient recoveryServicesClient;
        private string subscriptionId;
        private X509Certificate2 certificate;
        private Uri serviceEndPoint;
        private string resourceName;
        private string cloudServiceName;

        public PSRecoveryServiceClient(WindowsAzureSubscription currentSubscription)
        {
            recoveryServicesClient = 
                currentSubscription.CreateClient<RecoveryServicesManagementClient>();
            subscriptionId = currentSubscription.SubscriptionId;
            certificate = currentSubscription.Certificate;
            serviceEndPoint = currentSubscription.ServiceEndpoint;
            resourceName = currentSubscription.AzureSiteRecoveryResourceName;
            cloudServiceName = currentSubscription.AzureSiteRecoveryCloudServiceName;
        }
        public PSRecoveryServiceClient() { }

        public CloudServiceListResponse GetAzureCloudServicesSyncInt()
        {
            return recoveryServicesClient.CloudServices.List();
        }

        public ServerListResponse GetAzureSiteRecoveryServer()
        {
            SiteRecoveryManagementClient siteRecoveryClient = 
                GetSiteRecoveryClient();

            if (siteRecoveryClient == null)
            {
                return null;
            }

            var serverList = new ServerListResponse();
            try
            {
                serverList = siteRecoveryClient.Servers.List();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return serverList;
        }

        private SiteRecoveryManagementClient GetSiteRecoveryClient()
        {
            CloudServiceListResponse services = recoveryServicesClient.CloudServices.List();
            string stampId = string.Empty;

            CloudService selectedCloudService = new CloudService();
            Vault selectedResource = null;

            foreach (CloudService cloudService in services)
            {
                if (cloudService.Name == cloudServiceName)
                {
                    selectedCloudService = cloudService;
                }
            }
            foreach (Vault vault in selectedCloudService.Resources)
            {
                if (vault.Name == resourceName)
                {
                    selectedResource = vault;
                }
            }
            foreach (OutputItem item in selectedResource.OutputItems)
            {
                if (item.Key.Equals("BackendStampId"))
                {
                    stampId = item.Value;
                }
            }
            if (string.IsNullOrEmpty(selectedCloudService.Name) || selectedResource == null || string.IsNullOrEmpty(stampId))
            {
                return null;
            }

            SiteRecoveryManagementClient siteRecoveryClient = 
                new SiteRecoveryManagementClient(
                    cloudServiceName, 
                    resourceName, 
                    stampId, 
                    new CertificateCloudCredentials(
                        subscriptionId, 
                        certificate), 
                    serviceEndPoint);

            return siteRecoveryClient;
        }
    }
}