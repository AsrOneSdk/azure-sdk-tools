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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml;
    using Microsoft.Azure.Portal.RecoveryServices.Models.Common;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using Microsoft.WindowsAzure.Management.RecoveryServices;
    using Microsoft.WindowsAzure.Management.RecoveryServices.Models;
    using Microsoft.WindowsAzure.Management.SiteRecovery;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    using Microsoft.WindowsAzure.Commands.Utilities.Common.Authentication;
    #endregion

    /// <summary>
    /// Recovery services convenience client.
    /// </summary>
    public partial class PSRecoveryServicesClient
    {
        /// <summary>
        /// Amount of time to sleep before fetching job details again.
        /// </summary>
        public const int TimeToSleepBeforeFetchingJobDetailsAgain = 30000;

        /// <summary>
        /// Resource credentials holds vault, cloud service name, vault key and other details.
        /// </summary>
        [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1401:FieldsMustBePrivate",
        Justification = "For Resource Credentials.")]
        public static ASRVaultCreds asrVaultCreds = new ASRVaultCreds();

        /// <summary>
        /// Recovery Services client.
        /// </summary>
        private RecoveryServicesManagementClient recoveryServicesClient;

        /// <summary>
        /// Windows Azure Subscription
        /// </summary>
        private WindowsAzureSubscription windowsAzureSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="PSRecoveryServicesClient" /> class.
        /// </summary>
        public PSRecoveryServicesClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PSRecoveryServicesClient" /> class with 
        /// required current subscription.
        /// </summary>
        /// <param name="currentSubscription">Current Subscription</param>
        public PSRecoveryServicesClient(WindowsAzureSubscription currentSubscription)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                IgnoreCertificateErrorHandler;

            windowsAzureSubscription = currentSubscription;

            this.recoveryServicesClient = 
                currentSubscription.CreateClient<RecoveryServicesManagementClient>();
        }

        private static bool IgnoreCertificateErrorHandler
           (object sender,
           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
           System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Retrieves Azure Cloud services.
        /// </summary>
        /// <returns>Cloud service list response</returns>
        public CloudServiceListResponse GetAzureCloudServicesSyncInt()
        {
            return this.recoveryServicesClient.CloudServices.List();
        }

        /// <summary>
        /// Validates current in-memory Vault Settings.
        /// </summary>
        /// <param name="resourceName">Resource Name</param>
        /// <param name="cloudServiceName">Cloud Service Name</param>
        /// <param name="services">Cloud Services</param>
        /// <returns>Whether Vault settings are valid or not</returns>
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

        /// <summary>
        /// Throws meaningful exception details extracted from Cloud exception.
        /// </summary>
        /// <param name="cloudException">Cloud exception thrown by service</param>
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

            HMACSHA256 sha = new HMACSHA256(Encoding.UTF8.GetBytes(asrVaultCreds.ChannelIntegrityKey));
            cikTokenDetails.Hmac =
                Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(shaInput)));
            cikTokenDetails.HashFunction = CikSupportedHashFunctions.HMACSHA256.ToString();

            return new JavaScriptSerializer().Serialize(cikTokenDetails);
        }

        /// <summary>
        /// Gets request headers.
        /// </summary>
        /// <returns>Custom request headers</returns>
        public CustomRequestHeaders GetRequestHeaders()
        {
            string clientRequestId = Guid.NewGuid().ToString() + "_PS";
            return new CustomRequestHeaders()
            {
                // ClientRequestId is a unique ID for every request to Azure Site Recovery.
                // It is useful when diagnosing failures in API calls.
                ClientRequestId = clientRequestId,
                AgentAuthenticationHeader = this.GenerateAgentAuthenticationHeader(clientRequestId)
            };
        }

        /// <summary>
        /// Gets Site Recovery client.
        /// </summary>
        /// <returns>Site Recovery Management client</returns>
        private SiteRecoveryManagementClient GetSiteRecoveryClient()
        {
            CloudServiceListResponse services = this.recoveryServicesClient.CloudServices.List();
            this.ValidateVaultSettings(
                asrVaultCreds.ResourceName,
                asrVaultCreds.CloudServiceName,
                services);

            CloudService selectedCloudService = null;
            Vault selectedResource = null;

            foreach (CloudService cloudService in services)
            {
                if (cloudService.Name == asrVaultCreds.CloudServiceName)
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
                if (vault.Name == asrVaultCreds.ResourceName)
                {
                    selectedResource = vault;
                }
            }

            if (null == selectedResource)
            {
                throw new ArgumentException(Properties.Resources.InvalidResource);
            }

            SubscriptionCloudCredentials subscriptionCloudCredentials = null;

            if (windowsAzureSubscription.ActiveDirectoryUserId == null)
            {
                subscriptionCloudCredentials =
                    new CertificateCloudCredentials(
                        windowsAzureSubscription.SubscriptionId,
                        windowsAzureSubscription.Certificate);
            }
            else
            {
                IAccessToken accessToken =
                    windowsAzureSubscription.TokenProvider.GetCachedToken(windowsAzureSubscription, windowsAzureSubscription.ActiveDirectoryUserId);
                subscriptionCloudCredentials =
                    new AccessTokenCredential(
                        windowsAzureSubscription.SubscriptionId,
                        accessToken);
            }

            SiteRecoveryManagementClient siteRecoveryClient =
                new SiteRecoveryManagementClient(
                    asrVaultCreds.CloudServiceName,
                    asrVaultCreds.ResourceName,
                    subscriptionCloudCredentials,
                    this.windowsAzureSubscription.ServiceEndpoint);

            if (null == siteRecoveryClient)
            {
                throw new InvalidOperationException(Properties.Resources.NullRecoveryServicesClient);
            }

            return siteRecoveryClient;
        }
    }
}