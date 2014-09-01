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

namespace Microsoft.Azure.Commands.RecoveryServices.SiteRecovery
{
    /// <summary>
    /// Parameter Sets used for Azure Site Recovery commands.
    /// </summary>
    internal static class ASRParameterSets
    {
        /// <summary>
        /// When only RP Object is passed to the command.
        /// </summary>
        internal const string ByRPObject = "ByRPObject";

        /// <summary>
        /// When only Object is passed to the command.
        /// </summary>
        internal const string ByObject = "ByObject";

        /// <summary>
        /// When only PE Object is passed to the command.
        /// </summary>
        internal const string ByPEObject = "ByPEObject";

        /// <summary>
        /// When only PC and PE ids are passed to the command.
        /// </summary>
        internal const string ByPCPEId = "ByPCPEId";

        /// <summary>
        /// When only ID is passed to the command.
        /// </summary>
        internal const string ById = "ById";

        /// <summary>
        /// When only RP ID is passed to the command.
        /// </summary>
        internal const string ByRPId = "ByRPId";

        /// <summary>
        /// When only Name is passed to the command.
        /// </summary>
        internal const string ByName = "ByName";

        /// <summary>
        /// When nothing is passed to the command.
        /// </summary>
        internal const string Default = "Default";

        /// <summary>
        /// When group of IDs are passed to the command.
        /// </summary>
        internal const string ByIDs = "ByIDs";

        /// <summary>
        /// When Object and ID are passed to the command.
        /// </summary>
        internal const string ByObjectWithId = "ByObjectWithId";

        /// <summary>
        /// When Object and Name are passed to the command.
        /// </summary>
        internal const string ByObjectWithName = "ByObjectWithName";

        /// <summary>
        /// When group of IDs and ID are passed to the command.
        /// </summary>
        internal const string ByIDsWithId = "ByIDsWithId";

        /// <summary>
        /// When group of IDs and Name are passed to the command.
        /// </summary>
        internal const string ByIDsWithName = "ByIDsWithName";
    }
}