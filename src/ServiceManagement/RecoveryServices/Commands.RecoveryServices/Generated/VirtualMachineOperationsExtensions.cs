// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.SiteRecovery;
using Microsoft.Azure.Management.SiteRecovery.Models;

namespace Microsoft.WindowsAzure
{
    public static partial class VirtualMachineOperationsExtensions
    {
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Aync calls.
        /// </returns>
        public static JobResponse DisableProtection(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IVirtualMachineOperations)s).DisableProtectionAsync(serverId, protectedContainerId, virtualMachineId);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Aync calls.
        /// </returns>
        public static Task<JobResponse> DisableProtectionAsync(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return operations.DisableProtectionAsync(serverId, protectedContainerId, virtualMachineId, CancellationToken.None);
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Aync calls.
        /// </returns>
        public static JobResponse EnableProtection(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IVirtualMachineOperations)s).EnableProtectionAsync(serverId, protectedContainerId, virtualMachineId);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Aync calls.
        /// </returns>
        public static Task<JobResponse> EnableProtectionAsync(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return operations.EnableProtectionAsync(serverId, protectedContainerId, virtualMachineId, CancellationToken.None);
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Vm object.
        /// </returns>
        public static VirtualMachineResponse Get(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IVirtualMachineOperations)s).GetAsync(serverId, protectedContainerId, virtualMachineId);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <param name='virtualMachineId'>
        /// Required. VM ID.
        /// </param>
        /// <returns>
        /// The response model for the Vm object.
        /// </returns>
        public static Task<VirtualMachineResponse> GetAsync(this IVirtualMachineOperations operations, string serverId, string protectedContainerId, string virtualMachineId)
        {
            return operations.GetAsync(serverId, protectedContainerId, virtualMachineId, CancellationToken.None);
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <returns>
        /// The response model for the list Vm operation.
        /// </returns>
        public static VirtualMachineListResponse List(this IVirtualMachineOperations operations, string serverId, string protectedContainerId)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IVirtualMachineOperations)s).ListAsync(serverId, protectedContainerId);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Get the list of all Vms under the cloud.  (see
        /// http://msdn.microsoft.com/en-us/library/windowsazure/XXXXX.aspx
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.SiteRecovery.IVirtualMachineOperations.
        /// </param>
        /// <param name='serverId'>
        /// Required. Server ID.
        /// </param>
        /// <param name='protectedContainerId'>
        /// Required. Protected Container ID.
        /// </param>
        /// <returns>
        /// The response model for the list Vm operation.
        /// </returns>
        public static Task<VirtualMachineListResponse> ListAsync(this IVirtualMachineOperations operations, string serverId, string protectedContainerId)
        {
            return operations.ListAsync(serverId, protectedContainerId, CancellationToken.None);
        }
    }
}
