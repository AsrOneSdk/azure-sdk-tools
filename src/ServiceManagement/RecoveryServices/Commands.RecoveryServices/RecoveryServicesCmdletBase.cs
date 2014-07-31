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
    #endregion

    /// <summary>
    /// The base class for all Windows Azure Recovery Service Cmdlets
    /// </summary>
    /// 
    /// abstract ?
    public abstract class RecoveryServicesCmdletBase : CmdletWithSubscriptionBase
    {
        private PSRecoveryServicesClient recoveryServicesClient;
        internal PSRecoveryServicesClient RecoveryServicesClient
        {
            get
            {
                if (recoveryServicesClient == null)
                {
                    recoveryServicesClient = new PSRecoveryServicesClient(CurrentSubscription);
                }
                return recoveryServicesClient;
            }
        }
    }
}