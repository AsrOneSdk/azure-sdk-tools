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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Runtime.Serialization;
    using System.Xml;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    #endregion

    /// <summary>
    /// Creates a vault settings file.
    /// </summary>
    [Cmdlet(VerbsCommunications.Write, "AzureSiteRecoveryVaultSettingFile")]
    [OutputType(typeof(FilePath))]
    public class WriteAzureSiteRecoveryVaultSettingsFile : RecoveryServicesCmdletBase
    {
        #region Parameters
        private string azureSiteRecoveryVaultSettingsFile = string.Empty;
        private string resourceName;
        private string cloudSeriveName;
        private string vaultKey;
        private string filePath;

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ResourceName
        {
            get { return this.resourceName; }
            set { this.resourceName = value; }
        }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string CloudSeriveName
        {
            get { return this.cloudSeriveName; }
            set { this.cloudSeriveName = value; }
        }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string VaultKey
        {
            get { return this.vaultKey; }
            set { this.vaultKey = value; }
        }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Filepath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
        #endregion Parameters

        public override void ExecuteCmdlet()
        {
            this.azureSiteRecoveryVaultSettingsFile =
                this.filePath + "\\RecoveryServicesVaultSettings.vaultsettings";
            FileStream stream = new FileStream(this.azureSiteRecoveryVaultSettingsFile, FileMode.Create);
            stream.Close();

            ResourceCredentials resourceCredentials = new ResourceCredentials();
            resourceCredentials.ResourceName = this.resourceName;
            resourceCredentials.CloudServiceName = this.cloudSeriveName;
            resourceCredentials.Key = this.vaultKey;

            string tempFilePath;
            var settings = new XmlWriterSettings { Indent = true, CloseOutput = true };

            using (var w = XmlWriter.Create(this.CreateTempFile(out tempFilePath), settings))
            {
                var serializer = new DataContractSerializer(typeof(ResourceCredentials));
                serializer.WriteObject(w, resourceCredentials);
            }

            File.Replace(tempFilePath, this.azureSiteRecoveryVaultSettingsFile, null);

            FilePath fp = new FilePath(this.azureSiteRecoveryVaultSettingsFile);
            this.WriteObject(fp);
        }

        private FileStream CreateTempFile(out string finalFileName)
        {
            do
            {
                try
                {
                    string fileName =
                        this.azureSiteRecoveryVaultSettingsFile + "." + Guid.NewGuid().ToString();
                    var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                    finalFileName = fileName;
                    return stream;
                }
                catch (IOException)
                {
                    // If we got this, the file already existed. Try again.
                }
            }
            while (true);
        }
    }

    /// <summary>
    /// Represent FILE path.
    /// </summary>
    public class FilePath
    {
        public string AzureSiteRecoveryVaultSettingsFile;

        public FilePath(string filePath)
        {
            this.AzureSiteRecoveryVaultSettingsFile = filePath;
        }
    }
}