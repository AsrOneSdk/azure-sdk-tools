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
    using System.Security.Cryptography.X509Certificates;
    #endregion

    public class ResourceCredentials
    {
        #region Properties

        /// <summary>
        /// Gets or sets the version of the security configuration version.
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Gets or sets the value for ACIK
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// Gets or sets the resource certificate
        /// </summary>
        public X509Certificate resourceCertificate { get; set; }

        /// <summary>
        /// Gets or sets the password for the resource certificate.
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Gets or sets the password for the resource certificate.
        /// </summary>
        public string certificateThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource name.
        /// </summary>
        public string resourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cloud service name.
        /// </summary>
        public string cloudServiceName { get; set; }
        
        /// <summary>
        /// Gets or sets the value for expiry date of the credential file.
        /// </summary>
        public DateTime notAfter { get; set; }

        #endregion Properties
    }
}