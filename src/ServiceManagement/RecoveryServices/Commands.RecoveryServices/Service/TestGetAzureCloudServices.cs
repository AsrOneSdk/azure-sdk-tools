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
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using Microsoft.Azure.Management.SiteRecovery;
    using Microsoft.Azure.Management.SiteRecovery.Models;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.WindowsAzure;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Threading.Tasks;
    #endregion

    [Cmdlet(VerbsDiagnostic.Test, "GetAzureCloudServices"), OutputType(typeof(string))]
    public class TestGetAzureCloudServices : CmdletWithSubscriptionBase
    {
        private const string CertSubjectName = "RdfeTestClientCert1";
        // private const string BaseUrl = "https://localhost:8443/RdfeProxy.svc/";
        private const string BaseUrl = "https://rajesh6:8443/RdfeProxy.svc";
        private const string SubscriptionId = "c7ae6b4b-c02f-4055-9341-4b7f50aebe2c"; // auxportal one

        public override void ExecuteCmdlet()
        {
            CloudServiceListResponse services = GetCloudServicesAsyncInt(BaseUrl, SubscriptionId, CertSubjectName);
            WriteObject(services.CloudServices.ToList());
            
        }

        private CloudServiceListResponse GetCloudServicesAsyncInt(
            string baseUrl,
            string subscriptionId,
            string certSubjectname
            )
        {
            // sanjkuma: Temp hack to disable cert validation.
            /*ServicePointManager.ServerCertificateValidationCallback =
                delegate
                {
                    return true;
                };*/

            X509Certificate2 _x509Certificate = GetCertBySubject(certSubjectname);

            CurrentSubscription = new WindowsAzureSubscription
            {
                SubscriptionId = subscriptionId,
                Certificate = _x509Certificate,
                ServiceEndpoint = new Uri(baseUrl)
            };

       
            ServicePointManager.ServerCertificateValidationCallback =
               delegate
               {
                   return true;
               };

            var client = CurrentSubscription.CreateClient<RecoveryServicesManagementClient>();

            //var client = new RecoveryServicesManagementClient(
            //    new CertificateCloudCredentials(subscriptionId, _x509Certificate),
            //    new System.Uri(baseUrl));

            /* var client = new SiteRecoveryManagementClient(
                new CertificateCloudCredentials(subscriptionId, _x509Certificate),
                new System.Uri(baseUrl));
             */

            CloudServiceListResponse retList = new CloudServiceListResponse();

            // Task<CloudServiceListResponse> task = client.CloudServices.ListAsync();
            // task.Wait();
            //if (task.Wait(60000))
            //{
                 // retList = task.Result;
            //}

            retList = client.CloudServices.List();

            return retList;
        }

        /// <summary>
        /// Get Cert from cert store based on the given subject.
        /// </summary>
        /// <param name="subject">Subject name.</param>
        private X509Certificate2 GetCertBySubject(string subject)
        {
            X509Certificate2 certificate = null;

            X509Store machineCertificateStore = new X509Store(
                StoreName.My,
                StoreLocation.LocalMachine);

            machineCertificateStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = machineCertificateStore.Certificates.Find(
                X509FindType.FindBySubjectName,
                subject,
                true);

            if (certs.Count == 1)
            {
                certificate = certs[0];
            }

            machineCertificateStore.Close();

            return certificate;
        }
    }
}