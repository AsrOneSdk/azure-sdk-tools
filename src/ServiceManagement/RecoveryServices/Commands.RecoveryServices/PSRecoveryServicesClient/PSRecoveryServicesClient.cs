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
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using Microsoft.WindowsAzure.Management.RecoveryServices;
    using Microsoft.WindowsAzure.Management.RecoveryServices.Models;
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    public partial class PSRecoveryServicesClient
    {
        public const int TimeToSleepBeforeFetchingJobDetailsAgain = 5000;
        public static ResourceCredentials ResourceCreds = new ResourceCredentials();

        private RecoveryServicesManagementClient recoveryServicesClient;
        private string subscriptionId;
        private X509Certificate2 certificate;
        private Uri serviceEndPoint;

        public PSRecoveryServicesClient(WindowsAzureSubscription currentSubscription)
        {
            // Temp code.
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            this.recoveryServicesClient = 
                currentSubscription.CreateClient<RecoveryServicesManagementClient>();
            this.subscriptionId = currentSubscription.SubscriptionId;
            this.certificate = currentSubscription.Certificate;
            this.serviceEndPoint = currentSubscription.ServiceEndpoint;
        }

        public PSRecoveryServicesClient()
        {
        }

        public CloudServiceListResponse GetAzureCloudServicesSyncInt()
        {
            return this.recoveryServicesClient.CloudServices.List();
        }

        public bool ValidateVaultSettings(
            string resourceName,
            string cloudServiceName,
            CloudServiceListResponse services = null)
        {
            if (string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(cloudServiceName))
            {
                throw new InvalidOperationException(Properties.Resources.MissingVaultSettings);
            }

            if (null == services)
            {
                services = this.recoveryServicesClient.CloudServices.List();
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
            Error error = null;
            try
            {
                using (Stream stream = new MemoryStream())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(cloudException.ErrorMessage);
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;

                    var deserializer = new DataContractSerializer(typeof(Error));
                    error = (Error)deserializer.ReadObject(stream);
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
                Properties.Resources.CloudExceptionDetails, 
                "\n",
                error.Message, 
                "\n",
                error.PossibleCauses, 
                "\n",
                error.RecommendedAction));
        }

        /// <summary>
        /// Site Recovery requests that go to on-premise components (like the Provider installed
        /// in VMM) require an authentication token that is signed with the vault key to indicate
        /// that the request indeed originated from the end-user client.
        /// Generating that authentication token here and sending it via http headers.
        /// </summary>
        /// <param name="clientRequestId">Unique identifier for the client's request</param>
        /// <returns>The authentication token for the provider</returns>
        public string GenerateAgentAuthenticationHeader(string clientRequestId)
        {
            CikTokenDetails cikTokenDetails = new CikTokenDetails();

            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddHours(-1);
            cikTokenDetails.NotBeforeTimestamp = TimeZoneInfo.ConvertTimeToUtc(currentDateTime);
            cikTokenDetails.NotAfterTimestamp = cikTokenDetails.NotBeforeTimestamp.AddHours(6);
            cikTokenDetails.ClientRequestId = clientRequestId;
            cikTokenDetails.Version = new Version(1, 2);
            cikTokenDetails.PropertyBag = new Dictionary<string, object>();

            string shaInput = new JavaScriptSerializer().Serialize(cikTokenDetails);

            HMACSHA256 sha = new HMACSHA256(Encoding.UTF8.GetBytes(ResourceCreds.Key));
            cikTokenDetails.Hmac =
                Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(shaInput)));
            cikTokenDetails.HashFunction = CikSupportedHashFunctions.HMACSHA256.ToString();

            return new JavaScriptSerializer().Serialize(cikTokenDetails);
        }

        public CustomRequestHeaders GetRequestHeaders()
        {
            return new CustomRequestHeaders()
            {
                // ClientRequestId is a unique ID for every request to Azure Site Recovery.
                // It is useful when diagnosing failures in API calls.
                ClientRequestId = Guid.NewGuid().ToString() + "_PS"
            };
        }

        private SiteRecoveryManagementClient GetSiteRecoveryClient()
        {
            CloudServiceListResponse services = this.recoveryServicesClient.CloudServices.List();
            this.ValidateVaultSettings(
                ResourceCreds.ResourceName,
                ResourceCreds.CloudServiceName,
                services);

            string stampId = string.Empty;
            CloudService selectedCloudService = null;
            Vault selectedResource = null;

            foreach (CloudService cloudService in services)
            {
                if (cloudService.Name == ResourceCreds.CloudServiceName)
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
                if (vault.Name == ResourceCreds.ResourceName)
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
                    ResourceCreds.CloudServiceName,
                    ResourceCreds.ResourceName,
                    stampId,
                    new CertificateCloudCredentials(
                        this.subscriptionId,
                        this.certificate),
                    this.serviceEndPoint);

            if (null == siteRecoveryClient)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient;
        }
    }
}