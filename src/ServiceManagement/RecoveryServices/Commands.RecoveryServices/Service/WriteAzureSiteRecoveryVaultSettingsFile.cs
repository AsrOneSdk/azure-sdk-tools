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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Runtime.Serialization;
    using System.Xml;
    using Microsoft.Azure.Portal.RecoveryServices.Models.Common;
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
        /// <summary>
        /// Azure Site Recovery Vault settings file.
        /// </summary>
        private string azureSiteRecoveryVaultSettingsFile = string.Empty;

        /// <summary>
        /// Resource name.
        /// </summary>
        private string resourceName;

        /// <summary>
        /// Cloud Service name.
        /// </summary>
        private string cloudSeriveName;

        /// <summary>
        /// Vault key.
        /// </summary>
        private string vaultKey;

        /// <summary>
        /// Azure Site Recovery Vault settings file path.
        /// </summary>
        private string filePath;

        /// <summary>
        /// Gets or sets Resource name.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ResourceName
        {
            get { return this.resourceName; }
            set { this.resourceName = value; }
        }

        /// <summary>
        /// Gets or sets Cloud service name.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string CloudSeriveName
        {
            get { return this.cloudSeriveName; }
            set { this.cloudSeriveName = value; }
        }

        /// <summary>
        /// Gets or sets Vault key.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string VaultKey
        {
            get { return this.vaultKey; }
            set { this.vaultKey = value; }
        }

        /// <summary>
        /// Gets or sets Azure Site Recovery Vault settings file path.
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Filepath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
        #endregion Parameters

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            this.azureSiteRecoveryVaultSettingsFile =
                this.filePath + "\\RecoveryServicesVaultSettings.VaultCredentials";
            FileStream stream = new FileStream(this.azureSiteRecoveryVaultSettingsFile, FileMode.Create);
            stream.Close();

            ASRVaultCreds asrVaultCreds = new ASRVaultCreds();
            asrVaultCreds.ResourceName = this.resourceName;
            asrVaultCreds.CloudServiceName = this.cloudSeriveName;
            asrVaultCreds.ChannelIntegrityKey = this.vaultKey;

            string tempFilePath;
            var settings = new XmlWriterSettings { Indent = true, CloseOutput = true };

            using (var w = XmlWriter.Create(this.CreateTempFile(out tempFilePath), settings))
            {
                var serializer = new DataContractSerializer(typeof(ASRVaultCreds));
                serializer.WriteObject(w, asrVaultCreds);
            }

            File.Replace(tempFilePath, this.azureSiteRecoveryVaultSettingsFile, null);

            FilePath fp = new FilePath(this.azureSiteRecoveryVaultSettingsFile);
            this.WriteObject(fp);
        }

        /// <summary>
        /// Creates temporary file.
        /// </summary>
        /// <param name="finalFileName">File path name</param>
        /// <returns>File Stream</returns>
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
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "This Cmdlet is temporary.")]
    public class FilePath
    {
        /// <summary>
        /// Azure Site Recovery Vault settings file path.
        /// </summary>
        [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1401:FieldsMustBePrivate",
        Justification = "To write to terminal.")]
        public string AzureSiteRecoveryVaultSettingsFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePath" /> class.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        public FilePath(string filePath)
        {
            this.AzureSiteRecoveryVaultSettingsFile = filePath;
        }
    }
}