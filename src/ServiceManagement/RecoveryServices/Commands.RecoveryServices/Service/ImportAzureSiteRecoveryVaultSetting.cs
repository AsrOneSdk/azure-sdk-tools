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
    using System.Management.Automation;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using System.Linq;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    // using Microsoft.WindowsAzure.Commands.Utilities.Profile;
    #endregion

    /// <summary>
    ///
    /// </summary>
    [Cmdlet(VerbsData.Import, "AzureSiteRecoveryVaultSetting")]
    public class ImportAzureSiteRecoveryVaultSetting : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// Path to the Azure site Recovery Vault Settings file. This file can be downloaded from 
        /// Azure site recovery Vault portal and stored locally.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "AzureSiteRecovery vault settings file path", ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string AzureSiteRecoveryVaultSettingsFile
        {
            get { return this.azureSiteRecoveryVaultSettingsFile; }
            set { this.azureSiteRecoveryVaultSettingsFile = value; }
        }
        private string azureSiteRecoveryVaultSettingsFile;
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            WriteObject("Vault settings path file: " + azureSiteRecoveryVaultSettingsFile);

            ResourceCredentials resourceCredentials = null;
            if (File.Exists(azureSiteRecoveryVaultSettingsFile))
            {
                try
                {
                    var serializer = new DataContractSerializer(typeof(ResourceCredentials));
                    using (var s = new FileStream(azureSiteRecoveryVaultSettingsFile, FileMode.Open,
                        FileAccess.Read, FileShare.Read))
                    {
                        resourceCredentials = (ResourceCredentials)serializer.ReadObject(s);
                    }
                }
                catch (XmlException xmlException)
                {
                    // WriteObject("XML is malformed or file is empty. Treat as no profile.");
                    WriteError(new ErrorRecord(xmlException, "XML is malformed or file is empty.", ErrorCategory.InvalidData, null));
                }
            }
            else
            {
                WriteObject("File doesn't exist");
                return;
            }

            if (string.IsNullOrEmpty(resourceCredentials.resourceName))
            {
                WriteObject("Resource Name is either null or empty");
                return;
            }

            if (string.IsNullOrEmpty(resourceCredentials.cloudServiceName))
            {
                WriteObject("Cloud Service Name is either null or empty");
                return;
            }

            WriteObject("Resource Name: " + resourceCredentials.resourceName);
            WriteObject("Cloud Service Name: " + resourceCredentials.cloudServiceName);
            this.ImportAzureSiteRecoveryVaultSettings(resourceCredentials);
        }

        public void ImportAzureSiteRecoveryVaultSettings(ResourceCredentials resourceCredentials)
        {
            WindowsAzureSubscription subscription = Profile.Subscriptions.FirstOrDefault(s => s.IsDefault);

            subscription.AzureSiteRecoveryResourceName = resourceCredentials.resourceName;
            subscription.AzureSiteRecoveryCloudServiceName = resourceCredentials.cloudServiceName;
            Profile.UpdateSubscription(subscription);
        }
    }
}