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

using System;
using System.Net;
using System.Net.Security;
using Microsoft.WindowsAzure;
using Microsoft.Azure.Utilities.HttpRecorder;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Management.RecoveryServices;
using Microsoft.WindowsAzure.Management.SiteRecovery;
using Microsoft.WindowsAzure.Testing;

namespace Microsoft.Azure.Commands.RecoveryServices.Test.ScenarioTests
{
    public abstract class RecoveryServicesTestsBase
    {
        private RDFETestEnvironmentFactory rdfeTestFactory;
        private EnvironmentSetupHelper helper;
        private string resourceName;
        private string cloudService;
        private string vaultKey;

        public SiteRecoveryManagementClient SiteRecoveryMgmtClient { get; private set; }
        public RecoveryServicesManagementClient RecoveryServicesMgmtClient { get; private set; }

        protected RecoveryServicesTestsBase()
        {
            resourceName = Environment.GetEnvironmentVariable("RESOURCE_NAME");
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception("Please set RESOURCE_NAME environment variable before running the tests");
            }

            cloudService = Environment.GetEnvironmentVariable("CLOUD_SERVICE_NAME");
            if (string.IsNullOrEmpty(cloudService))
            {
                throw new Exception("Please set CLOUD_SERVICE_NAME environment variable before running the tests");
            }

            vaultKey = Environment.GetEnvironmentVariable("CHANNEL_INTEGRITY_KEY");
            if (string.IsNullOrEmpty(vaultKey))
            {
                throw new Exception("Please set CHANNEL_INTEGRITY_KEY environment variable before running the tests");
            }

            helper = new EnvironmentSetupHelper();
        }

        protected void SetupManagementClients()
        {
            RecoveryServicesMgmtClient = GetRecoveryServicesManagementClient();
            SiteRecoveryMgmtClient = GetSiteRecoveryManagementClient();

            helper.SetupManagementClients(RecoveryServicesMgmtClient, SiteRecoveryMgmtClient);
            // helper.SetupManagementClients(recoveryServicesManagementClient);
        }

        protected void RunPowerShellTest(params string[] scripts)
        {
            using (UndoContext context = UndoContext.Current)
            {
                context.Start(TestUtilities.GetCallingClass(2), TestUtilities.GetCurrentMethodName(2));

                this.rdfeTestFactory = new RDFETestEnvironmentFactory();

                SetupManagementClients();

                helper.SetupEnvironment(AzureModule.AzureServiceManagement);
                helper.SetupModules(AzureModule.AzureServiceManagement,
                    "ScenarioTests\\" + this.GetType().Name + ".ps1");

                helper.RunPowerShellTest(scripts);
            }
        }

        private RecoveryServicesManagementClient GetRecoveryServicesManagementClient()
        {
            return TestBase.GetServiceClient<RecoveryServicesManagementClient>(this.rdfeTestFactory);
        }

        private SiteRecoveryManagementClient GetSiteRecoveryManagementClient()
        {
            // return TestBase.GetServiceClient<SiteRecoveryManagementClient>(new RDFETestEnvironmentFactory());
            TestEnvironment environment = this.rdfeTestFactory.GetTestEnvironment();

            if (ServicePointManager.ServerCertificateValidationCallback == null)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    IgnoreCertificateErrorHandler;
            }

            return new SiteRecoveryManagementClient(
                cloudService,
                resourceName,
                (SubscriptionCloudCredentials)environment.Credentials,
                environment.BaseUri).WithHandler(HttpMockServer.CreateInstance());
        }

        private static bool IgnoreCertificateErrorHandler
           (object sender,
           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}