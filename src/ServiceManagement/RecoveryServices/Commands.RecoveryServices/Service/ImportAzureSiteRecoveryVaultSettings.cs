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
    using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Runtime.Serialization;
    using System.Xml;
    #endregion

    [Cmdlet(VerbsData.Import, "AzureSiteRecoveryVaultSettingsFile")]
    [OutputType(typeof(ASRVaultSettings))]
    public class ImportAzureSiteRecoveryVaultSettingsFile : RecoveryServicesCmdletBase
    {
        #region Parameters
        /// <summary>
        /// Path to the Azure site Recovery Vault Settings file. This file can be downloaded from 
        /// Azure site recovery Vault portal and stored locally.
        /// </summary>
        [Parameter(
            Position = 0, 
            Mandatory = true, 
            HelpMessage = "AzureSiteRecovery vault settings file path", 
            ValueFromPipelineByPropertyName = true)]
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
            WriteVerbose("Vault Settings File path: " + azureSiteRecoveryVaultSettingsFile);

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
                    throw new XmlException(
                        string.Format(Properties.Resources.InvalidXml, xmlException));
                }
                catch (SerializationException serializationException)
                {
                    throw new SerializationException(
                        string.Format(Properties.Resources.InvalidXml, serializationException));
                }
            }
            else
            {
                throw new FileNotFoundException(
                    Properties.Resources.VaultSettingFileNotFound, 
                    azureSiteRecoveryVaultSettingsFile);
            }

            // Validate required parameters taken from the Vault settings file.
            if (string.IsNullOrEmpty(resourceCredentials.ResourceName))
            {
                throw new ArgumentException(
                    Properties.Resources.ResourceNameNullOrEmpty, 
                    resourceCredentials.ResourceName);
            }

            if (string.IsNullOrEmpty(resourceCredentials.CloudServiceName))
            {
                throw new ArgumentException(
                    Properties.Resources.CloudServiceNameNullOrEmpty,
                    resourceCredentials.CloudServiceName);
            }
            try
            {
                RecoveryServicesClient.ValidateVaultSettings(
                    resourceCredentials.ResourceName, 
                    resourceCredentials.CloudServiceName);

                this.ImportAzureSiteRecoveryVaultSettings(resourceCredentials);
                WriteObject(new ASRVaultSettings(
                    resourceCredentials.ResourceName, 
                    resourceCredentials.CloudServiceName));
            }
            catch (CloudException cloudException)
            {
                RecoveryServicesClient.ThrowCloudExceptionDetails(cloudException);
            }
        }

        public void ImportAzureSiteRecoveryVaultSettings(ResourceCredentials resourceCredentials)
        {
            Object updateVaultSettingsOneAtATime = new Object();
            lock (updateVaultSettingsOneAtATime)
            {
                PSRecoveryServicesClient.resourceCredentials.ResourceName =
                    resourceCredentials.ResourceName;
                PSRecoveryServicesClient.resourceCredentials.CloudServiceName =
                    resourceCredentials.CloudServiceName;
                PSRecoveryServicesClient.resourceCredentials.Key =
                    resourceCredentials.Key;
            }
        }
    }
}