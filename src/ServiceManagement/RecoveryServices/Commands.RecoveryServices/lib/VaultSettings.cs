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
    #endregion

    public class VaultSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the resource name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cloud service name.
        /// </summary>
        public string CloudServiceName { get; set; }

        #endregion Properties

        public VaultSettings() { }
        public VaultSettings(string resourceName, string cloudServiceName)
        {
            ResourceName = resourceName;
            CloudServiceName = cloudServiceName;
        }
    }
}