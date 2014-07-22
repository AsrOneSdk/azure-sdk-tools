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
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml;
    #endregion

    class PSRecoveryServiceClient
    {
        private RecoveryServicesManagementClient recoveryServicesClient;
        private string subscriptionId;
        private X509Certificate2 certificate;
        private Uri serviceEndPoint;

        public static ResourceCredentials resourceCredentials = new ResourceCredentials();

        public PSRecoveryServiceClient(WindowsAzureSubscription currentSubscription)
        {
            recoveryServicesClient = 
                currentSubscription.CreateClient<RecoveryServicesManagementClient>();
            subscriptionId = currentSubscription.SubscriptionId;
            certificate = currentSubscription.Certificate;
            serviceEndPoint = currentSubscription.ServiceEndpoint;
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

            if (null == siteRecoveryClient)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Servers.List();
        }

        public ServerResponse GetAzureSiteRecoveryServer(string serverId)
        {
            SiteRecoveryManagementClient siteRecoveryClient =
                GetSiteRecoveryClient();

            if (null == siteRecoveryClient)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Servers.Get(serverId);
        }

        public CloudListResponse GetAzureSiteRecoveryCloud(string serverId)
        {
            SiteRecoveryManagementClient siteRecoveryClient =
                GetSiteRecoveryClient();

            if (siteRecoveryClient == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Clouds.List(serverId);
        }

        public CloudResponse GetAzureSiteRecoveryCloud(string serverId, string protectedContainerId)
        {
            SiteRecoveryManagementClient siteRecoveryClient =
                GetSiteRecoveryClient();

            if (siteRecoveryClient == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Clouds.Get(serverId, protectedContainerId);
        }

        public VirtualMachineListResponse GetAzureSiteRecoveryVirtualMachine(
            string serverId, 
            string containerId)
        {
            SiteRecoveryManagementClient siteRecoveryClient =
                GetSiteRecoveryClient();

            if (siteRecoveryClient == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Vm.List(serverId, containerId);
        }

        public VirtualMachineResponse GetAzureSiteRecoveryVirtualMachine(
            string serverId,
            string containerId,
            string virtualMachineId)
        {
            SiteRecoveryManagementClient siteRecoveryClient =
                GetSiteRecoveryClient();

            if (siteRecoveryClient == null)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient.Vm.Get(serverId, containerId, virtualMachineId);
        }

        private SiteRecoveryManagementClient GetSiteRecoveryClient()
        {
            CloudServiceListResponse services = recoveryServicesClient.CloudServices.List();
            this.ValidateVaultSettings(
                resourceCredentials.resourceName,
                resourceCredentials.cloudServiceName,
                services);

            string stampId = string.Empty;
            CloudService selectedCloudService = null;
            Vault selectedResource = null;

            foreach (CloudService cloudService in services)
            {
                if (cloudService.Name == resourceCredentials.cloudServiceName)
                {
                    selectedCloudService = cloudService;
                }
            }

            if (null == selectedCloudService)
            {
                throw new ArgumentException(Properties.Resources.InvalidCloudService);
            }

            foreach (Vault vault in selectedCloudService.Resources)
            {
                if (vault.Name == resourceCredentials.resourceName)
                {
                    selectedResource = vault;
                }
            }

            if (null == selectedResource)
            {
                throw new ArgumentException(Properties.Resources.InvalidResource);
            }

            foreach (OutputItem item in selectedResource.OutputItems)
            {
                if (item.Key.Equals("BackendStampId"))
                {
                    stampId = item.Value;
                }
            }

            if (string.IsNullOrEmpty(stampId))
            {
                throw new InvalidDataException(Properties.Resources.MissingBackendStampId);
            }

            SiteRecoveryManagementClient siteRecoveryClient = 
                new SiteRecoveryManagementClient(
                    resourceCredentials.cloudServiceName,
                    resourceCredentials.resourceName,
                    stampId, 
                    new CertificateCloudCredentials(
                        subscriptionId, 
                        certificate), 
                    serviceEndPoint);

            return siteRecoveryClient;
        }

        public bool ValidateVaultSettings(string resourceName, string cloudServiceName, CloudServiceListResponse services = null)
        {

            if (string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(cloudServiceName))
            {
                throw new InvalidOperationException(Properties.Resources.MissingVaultSettings);
            }

            if (null == services)
            {
                services = recoveryServicesClient.CloudServices.List();
            }

            string stampId = string.Empty;

            CloudService selectedCloudService = null;
            Vault selectedResource = null;

            foreach (CloudService cloudService in services)
            {
                if (cloudService.Name == cloudServiceName)
                {
                    selectedCloudService = cloudService;
                }
            }

            if (null == selectedCloudService)
            {
                throw new ArgumentException(Properties.Resources.InvalidCloudService);
            }

            foreach (Vault vault in selectedCloudService.Resources)
            {
                if (vault.Name == resourceName)
                {
                    selectedResource = vault;
                }
            }

            if (null == selectedResource)
            {
                throw new ArgumentException(Properties.Resources.InvalidResource);
            }

            return true;
        }

        public void ThrowCloudExceptionDetails(CloudException cloudException)
        {
            Error psError = null;
            try
            {
                using (Stream stream = new MemoryStream())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(cloudException.ErrorMessage);
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;

                    var deserializer = new DataContractSerializer(typeof(Error));
                    psError = (Error)deserializer.ReadObject(stream);
                }
            }
            catch (XmlException)
            {
                throw new XmlException(
                    string.Format(
                    Properties.Resources.InvalidCloudExceptionErrorMessage,
                    cloudException.ErrorMessage));
            }
            catch (SerializationException)
            {
                throw new SerializationException(
                    string.Format(
                    Properties.Resources.InvalidCloudExceptionErrorMessage,
                    cloudException.ErrorMessage));
            }

            throw new InvalidOperationException(
                string.Format(
                Properties.Resources.CloudExceptionDetails, "\n",
                psError.Message, "\n",
                psError.PossibleCauses, "\n",
                psError.RecommendedAction));
        }
    }
}