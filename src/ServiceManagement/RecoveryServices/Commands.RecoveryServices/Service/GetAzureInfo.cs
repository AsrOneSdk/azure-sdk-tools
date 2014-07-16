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

/*
 * This is a first & sample cmdlet which writes some basic Azure account & Subscription information.
 */ 

namespace Microsoft.Azure.Commands.RecoveryServices
{
    #region Using directives
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    #endregion

    [Cmdlet(VerbsCommon.Get, "AzureInfo"), OutputType(typeof(string))]
    public class GetAzureInfo : RecoveryServicesCmdletBase
    {
        public override void ExecuteCmdlet()
        {
            IEnumerable<WindowsAzureSubscription> subscriptions = Profile.Subscriptions.Where(s => s.ActiveDirectoryUserId != null);
            var sortedSubscriptions = from s in subscriptions
                                      orderby s.ActiveDirectoryUserId ascending
                                      group s by s.ActiveDirectoryUserId into g
                                      select new
                                      {
                                          AzureAccountName = g.Key,
                                      };

            WriteObject(sortedSubscriptions.First());
            WriteObject("Subscription Name: " + CurrentSubscription.SubscriptionName);
            WriteObject("Subscription ID: " + CurrentSubscription.SubscriptionId);
            // WriteObject("Storage Account Name: " + CurrentSubscription.CurrentStorageAccountName);
            WriteObject("Service End point: " + CurrentSubscription.ServiceEndpoint);
            WriteObject("Gallery End point: " + CurrentSubscription.GalleryEndpoint);
            WriteObject("Resource Manager End point: " + CurrentSubscription.ResourceManagerEndpoint);
        }
    }
}