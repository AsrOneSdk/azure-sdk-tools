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
    using System.Runtime.Serialization;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// Hash functions which can be used to calculate CIK HMAC.
    /// </summary>
    public enum CikSupportedHashFunctions
    {
        HMACSHA256,

        HMACSHA384,

        HMACSHA512
    }

    /// <summary>
    /// Represents resource / vault credentials.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all contracts together.")]
    public class ResourceCredentials
    {
        /// <summary>
        /// Gets or sets the version of the security configuration version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the value for ACIK
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the resource certificate
        /// </summary>
        public X509Certificate ResourceCertificate { get; set; }

        /// <summary>
        /// Gets or sets the password for the resource certificate.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password for the resource certificate.
        /// </summary>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cloud service name.
        /// </summary>
        public string CloudServiceName { get; set; }
        
        /// <summary>
        /// Gets or sets the value for expiry date of the credential file.
        /// </summary>
        public DateTime NotAfter { get; set; }
    }

    /// <summary>
    /// Error contract returned when some exception occurs in ASR REST API.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all contracts together.")]
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure")]
    public class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        public Error()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        public Error(ServiceError se)
        {
            this.ActivityId = se.ActivityId;
            this.Code = se.Code;
            this.Message = se.Message;
            this.PossibleCauses = se.PossibleCauses;
            this.RecommendedAction = se.RecommendedAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="errorCode">Service generated error code.</param>
        /// <param name="message">Error message.</param>
        /// <param name="possibleCauses">Possible causes of the error.</param>
        /// <param name="recommendedAction">Recommended action to resolve the error.</param>
        /// <param name="activityId">ActivityId in which error occurred.</param>
        public Error(
            string errorCode,
            string message,
            string possibleCauses,
            string recommendedAction,
            string activityId)
        {
            this.Code = errorCode;
            this.Message = message;
            this.PossibleCauses = possibleCauses;
            this.RecommendedAction = recommendedAction;
            this.ActivityId = activityId;
        }

        /// <summary>
        /// Gets or sets error code.
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets possible causes of error.
        /// </summary>
        [DataMember]
        public string PossibleCauses { get; set; }

        /// <summary>
        /// Gets or sets recommended action to resolve error.
        /// </summary>
        [DataMember]
        public string RecommendedAction { get; set; }

        /// <summary>
        /// Gets or sets activity Id.
        /// </summary>
        [DataMember]
        public string ActivityId { get; set; }
    }

    /// <summary>
    /// CIK token details.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Keeping all contracts together.")]
    [DataContract]
    public class CikTokenDetails
    {
        /// <summary>
        /// Gets or sets the timestamp before which the token is not valid.
        /// </summary>
        [DataMember]
        public DateTime NotBeforeTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the timestamp after which the token is not valid.
        /// </summary>
        [DataMember]
        public DateTime NotAfterTimestamp { get; set; }

        /// <summary>
        /// The client request Id for the operation linked with this CIK token.
        /// </summary>
        [DataMember]
        public string ClientRequestId { get; set; }

        /// <summary>
        /// Hash function used to calculate the HMAC.
        /// </summary>
        [DataMember]
        public string HashFunction { get; set; }

        /// <summary>
        /// The HMAC generated using the CIK key.
        /// </summary>
        [DataMember]
        public string Hmac { get; set; }

        /// <summary>
        /// Data contract version.
        /// </summary>
        [DataMember(Name = "Version")]
        public Version Version { get; set; }

        /// <summary>
        /// This property bag is introduced to support addition of any new 
        /// property in data contract without breaking the existing clients.
        /// If any new property needs to be introduced in the contract, 
        /// add a key value pair for it in this dictionary. 
        /// </summary>
        [DataMember]
        public Dictionary<string, object> PropertyBag { get; set; }

        /// <summary>
        /// Converts the object into string.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("NotBeforeTimestamp: " + this.NotBeforeTimestamp);
            sb.AppendLine("NotAfterTimestamp: " + this.NotAfterTimestamp);
            sb.AppendLine("ClientRequestId: " + this.ClientRequestId);
            sb.AppendLine("Hmac: " + this.Hmac);
            return sb.ToString();
        }
    }
}