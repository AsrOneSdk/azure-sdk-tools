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

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using System.Net;
    using Management.Network;
    using Management.Network.Models;
    using Model;
    using Properties;
    using Utilities.Common;

    [Cmdlet(VerbsCommon.Get, "AzureVNetSite"), OutputType(typeof(IEnumerable<VirtualNetworkSiteContext>))]
    public class GetAzureVNetSiteCommand : ServiceManagementBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The virtual network name.")]
        [ValidateNotNullOrEmpty]
        public string VNetName
        {
            get;
            set;
        }

        public IEnumerable<VirtualNetworkSiteContext> GetVirtualNetworkSiteProcess()
        {
            IEnumerable<VirtualNetworkSiteContext> result = null;

            InvokeInOperationContext(() =>
            {
                try
                {
                    WriteVerboseWithTimestamp(string.Format(Resources.AzureVNetSiteBeginOperation, CommandRuntime.ToString()));
                    var response = this.NetworkClient.Networks.List();
                    var sites = response.VirtualNetworkSites;

                    if (!string.IsNullOrEmpty(this.VNetName))
                    {
                        sites = sites.Where(s => string.Equals(s.Name, this.VNetName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if (sites.Count() == 0)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.VirtualNetworkNameNotFound, this.VNetName), "VNetName");
                        }
                    }

                    var operation = GetOperationNewSM(response.RequestId);
                    WriteVerboseWithTimestamp(string.Format(Resources.AzureVNetSiteCompletedOperation, CommandRuntime.ToString()));
                    result = sites.Select(site => ContextFactory<NetworkListResponse.VirtualNetworkSite, VirtualNetworkSiteContext>(site, operation));
                }
                catch (CloudException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.NotFound && !IsVerbose())
                    {
                        result = null;
                    }
                    else
                    {
                        this.WriteExceptionDetails(ex);
                    }
                }
            });

            return result;
        }

        protected override void OnProcessRecord()
        {
            ServiceManagementProfile.Initialize();
            var virtualNetworkSites = this.GetVirtualNetworkSiteProcess();
            if (virtualNetworkSites != null)
            {
                WriteObject(virtualNetworkSites, true);
            }
        }
    }
}
