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
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Xml;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Commands.Utilities.Common;
    using Microsoft.WindowsAzure.Management.SiteRecovery.Models;
    #endregion

    /// <summary>
    /// The base class for all Windows Azure Recovery Services commands
    /// </summary>
    public abstract class RecoveryServicesCmdletBase : AzurePSCmdlet
    {
        /// <summary>
        /// Recovery Services client.
        /// </summary>
        private PSRecoveryServicesClient recoveryServicesClient;

        /// <summary>
        /// Gets or sets a value indicating whether stop processing has been triggered.
        /// </summary>
        internal bool StopProcessingFlag { get; set; }

        /// <summary>
        /// Gets Recovery Services client.
        /// </summary>
        internal PSRecoveryServicesClient RecoveryServicesClient
        {
            get
            {
                if (this.recoveryServicesClient == null)
                {
                    this.recoveryServicesClient = new PSRecoveryServicesClient(CurrentContext.Subscription);
                }

                return this.recoveryServicesClient;
            }
        }

        /// <summary>
        /// Exception handler.
        /// </summary>
        /// <param name="ex">Exception to handle.</param>
        public void HandleException(Exception ex)
        {
            string clientRequestIdMsg = string.Empty;
            if (this.recoveryServicesClient != null)
            {
                clientRequestIdMsg = "ClientRequestId: " + this.recoveryServicesClient.ClientRequestId + "\n";
            }

            CloudException cloudException = ex as CloudException;
            if (cloudException != null)
            {
                Error error = null;
                try
                {
                    using (Stream stream = new MemoryStream())
                    {
                        if (cloudException.ErrorMessage != null)
                        {
                            byte[] data = System.Text.Encoding.UTF8.GetBytes(cloudException.ErrorMessage);
                            stream.Write(data, 0, data.Length);
                            stream.Position = 0;

                            var deserializer = new DataContractSerializer(typeof(Error));
                            error = (Error)deserializer.ReadObject(stream);

                            throw new InvalidOperationException(
                                string.Format(
                                Properties.Resources.CloudExceptionDetails,
                                error.Message,
                                error.PossibleCauses,
                                error.RecommendedAction,
                                error.ClientRequestId));
                        }
                        else
                        {
                            throw new Exception(
                                string.Format(
                                Properties.Resources.InvalidCloudExceptionErrorMessage,
                                clientRequestIdMsg + ex.Message),
                                ex);
                        }
                    }
                }
                catch (XmlException)
                {
                    throw new XmlException(
                        string.Format(
                        Properties.Resources.InvalidCloudExceptionErrorMessage,
                        cloudException.ErrorMessage),
                        cloudException);
                }
                catch (SerializationException)
                {
                    throw new SerializationException(
                        string.Format(
                        Properties.Resources.InvalidCloudExceptionErrorMessage,
                        clientRequestIdMsg + cloudException.ErrorMessage),
                        cloudException);
                }
            }
            else if (ex.Message != null)
            {
                throw new Exception(
                    string.Format(
                    Properties.Resources.InvalidCloudExceptionErrorMessage,
                    clientRequestIdMsg + ex.Message),
                    ex);
            }
        }

        /// <summary>
        /// Waits for the job to complete.
        /// </summary>
        /// <param name="jobId">Id of the job to wait for.</param>
        public void WaitForCompletion(string jobId)
        {
            JobResponse jobResponse = null;
            do
            {
                Thread.Sleep(PSRecoveryServicesClient.TimeToSleepBeforeFetchingJobDetailsAgain);
                jobResponse = this.RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(jobId);
                this.WriteObject("JobState: " + jobResponse.Job.State);
            }
            while (jobResponse.Job.State == JobStatus.Cancelled ||
                            jobResponse.Job.State == JobStatus.Failed ||
                            jobResponse.Job.State == JobStatus.Suspended ||
                            jobResponse.Job.State == JobStatus.Succeeded ||
                        this.StopProcessingFlag);
        }
    }
}