﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Storage {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.WindowsAzure.Management.Storage.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} begin processing without ParameterSet..
        /// </summary>
        internal static string BeginProcessingWithoutParameterSetLog {
            get {
                return ResourceManager.GetString("BeginProcessingWithoutParameterSetLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} begin processing with ParameterSet &apos;{1}&apos;.
        /// </summary>
        internal static string BeginProcessingWithParameterSetLog {
            get {
                return ResourceManager.GetString("BeginProcessingWithParameterSetLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blob &apos;{0}&apos; in container &apos;{1}&apos; already exists..
        /// </summary>
        internal static string BlobAlreadyExists {
            get {
                return ResourceManager.GetString("BlobAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blob End Point: {0}..
        /// </summary>
        internal static string BlobEndPointTips {
            get {
                return ResourceManager.GetString("BlobEndPointTips", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find blob name or container name..
        /// </summary>
        internal static string BlobNameNotFound {
            get {
                return ResourceManager.GetString("BlobNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find blob &apos;{0}&apos; in container &apos;{1}&apos;..
        /// </summary>
        internal static string BlobNotFound {
            get {
                return ResourceManager.GetString("BlobNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blob type mismatched, the current blob type of &apos;{0}&apos; is {1}..
        /// </summary>
        internal static string BlobTypeMismatch {
            get {
                return ResourceManager.GetString("BlobTypeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified blob &apos;{0}&apos; is already a snapshot with snapshot time {1}. Can&apos;t use &quot;DeleteSnapshot&quot; option for it..
        /// </summary>
        internal static string CannotDeleteSnapshotForSnapshot {
            get {
                return ResourceManager.GetString("CannotDeleteSnapshotForSnapshot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not send the directory &apos;{0}&apos; to azure. This cmdlet only works with files..
        /// </summary>
        internal static string CannotSendDirectory {
            get {
                return ResourceManager.GetString("CannotSendDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Azure-Storage-PowerShell-{0}.
        /// </summary>
        internal static string ClientRequestIdFormat {
            get {
                return ResourceManager.GetString("ClientRequestIdFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CloudBlobClient is null..
        /// </summary>
        internal static string CloudBlobClientIsNull {
            get {
                return ResourceManager.GetString("CloudBlobClientIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The blob &apos;{0}&apos; in container &apos;{1}&apos; has snapshots. Are you sure to remove blob and its snapshots?.
        /// </summary>
        internal static string ConfirmRemoveBlobWithSnapshot {
            get {
                return ResourceManager.GetString("ConfirmRemoveBlobWithSnapshot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure to remove container &apos;{0}&apos;?.
        /// </summary>
        internal static string ConfirmRemoveContainer {
            get {
                return ResourceManager.GetString("ConfirmRemoveContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm to remove azure container..
        /// </summary>
        internal static string ConfirmRemoveContainerCaption {
            get {
                return ResourceManager.GetString("ConfirmRemoveContainerCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Container &apos;{0}&apos; already exists..
        /// </summary>
        internal static string ContainerAlreadyExists {
            get {
                return ResourceManager.GetString("ContainerAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find the container &apos;{0}&apos;..
        /// </summary>
        internal static string ContainerNotFound {
            get {
                return ResourceManager.GetString("ContainerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current Storage Account not found in subscription &apos;{0}&apos;. Please set it use &quot;Set-AzureSubscription&quot;..
        /// </summary>
        internal static string CurrentStorageAccountNameNotFound {
            get {
                return ResourceManager.GetString("CurrentStorageAccountNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not found current storage account &apos;{0}&apos; on azure, please check whether your storage account exists..
        /// </summary>
        internal static string CurrentStorageAccountNotFoundOnAzure {
            get {
                return ResourceManager.GetString("CurrentStorageAccountNotFoundOnAzure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.blob.core.windows.net/.
        /// </summary>
        internal static string DefaultBlobEndPointFormat {
            get {
                return ResourceManager.GetString("DefaultBlobEndPointFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.queue.core.windows.net/.
        /// </summary>
        internal static string DefaultQueueEndPointFormat {
            get {
                return ResourceManager.GetString("DefaultQueueEndPointFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find your azure storage credential. Please set current storage account using &quot;Set-AzureSubscription&quot; or set the &quot;AZURE_STORAGE_CONNECTION_STRING&quot; environment variable..
        /// </summary>
        internal static string DefaultStorageCredentialsNotFound {
            get {
                return ResourceManager.GetString("DefaultStorageCredentialsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://{0}.table.core.windows.net/.
        /// </summary>
        internal static string DefaultTableEndPointFormat {
            get {
                return ResourceManager.GetString("DefaultTableEndPointFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory &apos;{0}&apos; doesn&apos;t exist..
        /// </summary>
        internal static string DirectoryNotExists {
            get {
                return ResourceManager.GetString("DirectoryNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot download the blob &apos;{0}&apos; in container &apos;{1}&apos; into local file &apos;{2}&apos;. Error: {3}.
        /// </summary>
        internal static string DownloadBlobFailed {
            get {
                return ResourceManager.GetString("DownloadBlobFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Download blob &apos;{0}&apos; successful..
        /// </summary>
        internal static string DownloadBlobSuccessful {
            get {
                return ResourceManager.GetString("DownloadBlobSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} end processing, Used {1} remote calls. Elapsed time {2:0.00} ms. Client operation id: {3}.
        /// </summary>
        internal static string EndProcessingLog {
            get {
                return ResourceManager.GetString("EndProcessingLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AZURE_STORAGE_CONNECTION_STRING.
        /// </summary>
        internal static string EnvConnectionString {
            get {
                return ResourceManager.GetString("EnvConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception cannot be null or empty..
        /// </summary>
        internal static string ExceptionCannotEmpty {
            get {
                return ResourceManager.GetString("ExceptionCannotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File &apos;{0}&apos; already exists..
        /// </summary>
        internal static string FileAlreadyExists {
            get {
                return ResourceManager.GetString("FileAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find the specified file &apos;{0}&apos;..
        /// </summary>
        internal static string FileNotFound {
            get {
                return ResourceManager.GetString("FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Percent : {0}% Speed : {1} bytes/second..
        /// </summary>
        internal static string FileTransmitStatus {
            get {
                return ResourceManager.GetString("FileTransmitStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finish remote call with status code {0} and service request id {1}..
        /// </summary>
        internal static string FinishRemoteCall {
            get {
                return ResourceManager.GetString("FinishRemoteCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get-AzureStorageContainerAcl.
        /// </summary>
        internal static string GetAzureStorageContainerAclCmdletName {
            get {
                return ResourceManager.GetString("GetAzureStorageContainerAclCmdletName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get-AzureStorageContainer.
        /// </summary>
        internal static string GetAzureStorageContainerCmdletName {
            get {
                return ResourceManager.GetString("GetAzureStorageContainerCmdletName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get storage account from environment variable &apos;AZURE_STORAGE_CONNECTION_STRING&apos;..
        /// </summary>
        internal static string GetStorageAccountFromEnvironmentVariable {
            get {
                return ResourceManager.GetString("GetStorageAccountFromEnvironmentVariable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://.
        /// </summary>
        internal static string HTTPPrefix {
            get {
                return ResourceManager.GetString("HTTPPrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://.
        /// </summary>
        internal static string HTTPSPrefix {
            get {
                return ResourceManager.GetString("HTTPSPrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Init service channel from current subscription.
        /// </summary>
        internal static string InitChannelFromSubscription {
            get {
                return ResourceManager.GetString("InitChannelFromSubscription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Init Operation Context with operation id {0}..
        /// </summary>
        internal static string InitOperationContextLog {
            get {
                return ResourceManager.GetString("InitOperationContextLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to invalid parameter combination, please see the command help..
        /// </summary>
        internal static string InvalidAccountParameterCombination {
            get {
                return ResourceManager.GetString("InvalidAccountParameterCombination", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blob name &apos;{0}&apos; is invalid..
        /// </summary>
        internal static string InvalidBlobName {
            get {
                return ResourceManager.GetString("InvalidBlobName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ICloudBlob &quot;{0}&quot; should contain container properties..
        /// </summary>
        internal static string InvalidBlobWithoutContainer {
            get {
                return ResourceManager.GetString("InvalidBlobWithoutContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Container name &apos;{0}&apos; is invalid..
        /// </summary>
        internal static string InvalidContainerName {
            get {
                return ResourceManager.GetString("InvalidContainerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid expiry time &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidExpiryTime {
            get {
                return ResourceManager.GetString("InvalidExpiryTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File name &quot;{0}&quot; is invalid..
        /// </summary>
        internal static string InvalidFileName {
            get {
                return ResourceManager.GetString("InvalidFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Queue name &apos;{0}&apos; is invalid..
        /// </summary>
        internal static string InvalidQueueName {
            get {
                return ResourceManager.GetString("InvalidQueueName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid start time &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidStartTime {
            get {
                return ResourceManager.GetString("InvalidStartTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name &apos;{0}&apos; is invalid..
        /// </summary>
        internal static string InvalidTableName {
            get {
                return ResourceManager.GetString("InvalidTableName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New-Alias.
        /// </summary>
        internal static string NewAlias {
            get {
                return ResourceManager.GetString("NewAlias", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string NewAliasName {
            get {
                return ResourceManager.GetString("NewAliasName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value.
        /// </summary>
        internal static string NewAliasValue {
            get {
                return ResourceManager.GetString("NewAliasValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object &apos;{0}&apos; cannot be null..
        /// </summary>
        internal static string ObjectCannotBeNull {
            get {
                return ResourceManager.GetString("ObjectCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must supply only one permission for container..
        /// </summary>
        internal static string OnlyOnePermissionForContainer {
            get {
                return ResourceManager.GetString("OnlyOnePermissionForContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prepare to upload blob..
        /// </summary>
        internal static string PrepareUploadingBlob {
            get {
                return ResourceManager.GetString("PrepareUploadingBlob", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Queue &apos;{0}&apos; already exists..
        /// </summary>
        internal static string QueueAlreadyExists {
            get {
                return ResourceManager.GetString("QueueAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find queue &apos;{0}&apos;..
        /// </summary>
        internal static string QueueNotFound {
            get {
                return ResourceManager.GetString("QueueNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Download blob &apos;{0}&apos; into &apos;{1}&apos;..
        /// </summary>
        internal static string ReceiveAzureBlobActivity {
            get {
                return ResourceManager.GetString("ReceiveAzureBlobActivity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remove operation of  blob &apos;{0}&apos; in container &apos;{1}&apos; is cancelled..
        /// </summary>
        internal static string RemoveBlobCancelled {
            get {
                return ResourceManager.GetString("RemoveBlobCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removed blob &apos;{0}&apos; in container &apos;{1}&apos; successfully..
        /// </summary>
        internal static string RemoveBlobSuccessfully {
            get {
                return ResourceManager.GetString("RemoveBlobSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remove operation of container &apos;{0}&apos; has been cancelled..
        /// </summary>
        internal static string RemoveContainerCancelled {
            get {
                return ResourceManager.GetString("RemoveContainerCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removed container &apos;{0}&apos; successfully..
        /// </summary>
        internal static string RemoveContainerSuccessfully {
            get {
                return ResourceManager.GetString("RemoveContainerSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remove operation of queue &apos;{0}&apos; has been cancelled..
        /// </summary>
        internal static string RemoveQueueCancelled {
            get {
                return ResourceManager.GetString("RemoveQueueCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removed queue &apos;{0}&apos; successfully..
        /// </summary>
        internal static string RemoveQueueSuccessfully {
            get {
                return ResourceManager.GetString("RemoveQueueSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remove operation of table &apos;{0}&apos; has been cancelled..
        /// </summary>
        internal static string RemoveTableCancelled {
            get {
                return ResourceManager.GetString("RemoveTableCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removed table &apos;{0}&apos; successfully..
        /// </summary>
        internal static string RemoveTableSuccessfully {
            get {
                return ResourceManager.GetString("RemoveTableSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload file &apos;{0}&apos; to blob &apos;{1}&apos; in container &apos;{2}&apos;..
        /// </summary>
        internal static string SendAzureBlobActivity {
            get {
                return ResourceManager.GetString("SendAzureBlobActivity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload local file &apos;{0}&apos; to blob &apos;{1}&apos; cancelled..
        /// </summary>
        internal static string SendAzureBlobCancelled {
            get {
                return ResourceManager.GetString("SendAzureBlobCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start to download blob &apos;{0}&apos; to &apos;{1}&apos;..
        /// </summary>
        internal static string StartDownloadBlob {
            get {
                return ResourceManager.GetString("StartDownloadBlob", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start {0}th remote call, method: {1}, destination: {2}..
        /// </summary>
        internal static string StartRemoteCall {
            get {
                return ResourceManager.GetString("StartRemoteCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start to upload &apos;{0}&apos; to blob &apos;{1}&apos;..
        /// </summary>
        internal static string StartUploadFile {
            get {
                return ResourceManager.GetString("StartUploadFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} stop processing, Use {1} remote calls. Elapsed time {2:0.00} ms. Client operation id: {3}.
        /// </summary>
        internal static string StopProcessingLog {
            get {
                return ResourceManager.GetString("StopProcessingLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Message {0}. HTTP Status Code: {1} - HTTP Error Message: {2}.
        /// </summary>
        internal static string StorageExceptionDetails {
            get {
                return ResourceManager.GetString("StorageExceptionDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table &apos;{0}&apos; already exists..
        /// </summary>
        internal static string TableAlreadyExists {
            get {
                return ResourceManager.GetString("TableAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not find table &apos;{0}&apos;..
        /// </summary>
        internal static string TableNotFound {
            get {
                return ResourceManager.GetString("TableNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown blob..
        /// </summary>
        internal static string UnknownBlob {
            get {
                return ResourceManager.GetString("UnknownBlob", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not upload local file to azure blob. Error: {0}..
        /// </summary>
        internal static string Upload2BlobFailed {
            get {
                return ResourceManager.GetString("Upload2BlobFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload file &apos;{0}&apos; failed.  Error: &apos;{1}&apos;..
        /// </summary>
        internal static string UploadFileFailed {
            get {
                return ResourceManager.GetString("UploadFileFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload file &apos;{0}&apos; successfully..
        /// </summary>
        internal static string UploadFileSuccessfully {
            get {
                return ResourceManager.GetString("UploadFileSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use current storage account &apos;{0}&apos; from subscription &apos;{1}&apos;..
        /// </summary>
        internal static string UseCurrentStorageAccountFromSubscription {
            get {
                return ResourceManager.GetString("UseCurrentStorageAccountFromSubscription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use storage account &apos;{0}&apos; from storage context.
        /// </summary>
        internal static string UseStorageAccountFromContext {
            get {
                return ResourceManager.GetString("UseStorageAccountFromContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} {1}..
        /// </summary>
        internal static string VerboseLogFormat {
            get {
                return ResourceManager.GetString("VerboseLogFormat", resourceCulture);
            }
        }
    }
}
