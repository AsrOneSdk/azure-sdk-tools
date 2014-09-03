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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Subscriptions;
using Microsoft.Azure.Subscriptions.Models;
using Microsoft.WindowsAzure.Commands.Common.Interfaces;
using Microsoft.WindowsAzure.Commands.Common.Models;
using Microsoft.WindowsAzure.Commands.Common.Properties;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Microsoft.WindowsAzure.Commands.Common
{
    /// <summary>
    /// Convenience client for azure profile and subscriptions.
    /// </summary>
    public class ProfileClient
    {
        public static IDataStore DataStore { get; set; }

        public AzureProfile Profile { get; private set; }

        public Action<string> WarningLog;

        public Action<string> DebugLog;

        private void WriteDebugMessage(string message)
        {
            if (DebugLog != null)
            {
                DebugLog(message);
            }
        }

        private void WriteWarningMessage(string message)
        {
            if (WarningLog != null)
            {
                WarningLog(message);
            }
        }

        private static void UpgradeProfile()
        {
            string oldProfileFilePath = System.IO.Path.Combine(AzurePowerShell.ProfileDirectory, AzurePowerShell.OldProfileFile);
            string newProfileFilePath = System.IO.Path.Combine(AzurePowerShell.ProfileDirectory, AzurePowerShell.ProfileFile);
            if (DataStore.FileExists(oldProfileFilePath))
            {
                string oldProfilePath = System.IO.Path.Combine(AzurePowerShell.ProfileDirectory,
                    AzurePowerShell.OldProfileFile);
                AzureProfile oldProfile = new AzureProfile(DataStore, oldProfilePath);

                if (DataStore.FileExists(newProfileFilePath))
                {
                    // Merge profile files
                    AzureProfile newProfile = new AzureProfile(DataStore, newProfileFilePath);
                    foreach (var environment in newProfile.Environments.Values)
                    {
                        oldProfile.Environments[environment.Name] = environment;
                    }
                    foreach (var subscription in newProfile.Subscriptions.Values)
                    {
                        oldProfile.Subscriptions[subscription.Id] = subscription;
                    }
                    DataStore.DeleteFile(newProfileFilePath);
                }

                // Save the profile to the disk
                oldProfile.Save();
                
                // Rename WindowsAzureProfile.xml to WindowsAzureProfile.json
                DataStore.RenameFile(oldProfilePath, newProfileFilePath);
            }
        }

        static ProfileClient()
        {
            DataStore = new DiskDataStore();
        }

        public ProfileClient()
            : this(System.IO.Path.Combine(AzurePowerShell.ProfileDirectory, AzurePowerShell.ProfileFile))
        {

        }

        public ProfileClient(string profilePath)
        {
            ProfileClient.UpgradeProfile();

            Profile = new AzureProfile(DataStore, profilePath);

            WarningLog = (s) => Debug.WriteLine(s);
        }

        #region Account management

        public AzureAccount AddAccount(UserCredentials credentials, AzureEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            credentials.ShowDialog = ShowDialog.Always;

            var subscriptionsFromDisk = Profile.Subscriptions.Values.Where(s => s.Environment == environment.Name);
            var subscriptionsFromServer = ListSubscriptionsFromServer(ref credentials, environment).ToList();
            var mergedSubscriptions = MergeSubscriptions(subscriptionsFromDisk.ToList(), subscriptionsFromServer);
            
            // Update back Profile.Subscriptions
            // Update AzureAccount
            foreach (var subscription in mergedSubscriptions)
            {
                Profile.Subscriptions[subscription.Id] = subscription;
            }

            // If credentials.UserName is null the login failed
            if (credentials.UserName != null)
            {
                AzureAccount account = new AzureAccount
                {
                    Id = credentials.UserName,
                    Type = AzureAccount.AccountType.User,
                };

                // Set account subscriptions from the profile
                account.SetSubscriptions(
                    Profile.Subscriptions.Values.Where(
                        s => s.Account == credentials.UserName)
                        .ToList());

                // Set account tenants
                account.SetOrAppendProperty(AzureAccount.Property.Tenants,
                    subscriptionsFromServer.SelectMany(s => s.GetPropertyAsArray(AzureSubscription.Property.Tenants))
                    .Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray());

                foreach (var subscription in subscriptionsFromServer)
                {
                    // Set account subscriptions from the server
                    if (!account.HasSubscription(subscription.Id))
                    {
                        account.SetOrAppendProperty(AzureAccount.Property.Subscriptions, subscription.Id.ToString());
                    }

                    // Set default account to credentials.UserName
                    Profile.Subscriptions[subscription.Id].Account = credentials.UserName;
                }

                // Add the account to the profile
                Profile.Accounts[account.Id] = account;

                if (Profile.DefaultSubscription == null)
                {
                    var firstSubscription = Profile.Subscriptions.Values.FirstOrDefault();
                    if (firstSubscription != null)
                    {
                        SetSubscriptionAsDefault(firstSubscription.Name, firstSubscription.Account);
                    }
                }

                return account;
            }
            else
            {
                return null;
            }
        }

        public AzureAccount GetAccountOrNull(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                throw new ArgumentNullException("accountName");
            }

            if (Profile.Accounts.ContainsKey(accountName))
            {
                return Profile.Accounts[accountName];
            }
            else
            {
                return null;
            }
        }

        public AzureAccount GetAccount(string accountName)
        {
            var account = GetAccountOrNull(accountName);
            
            if (account == null)
            {
                throw new ArgumentException(string.Format("Account with name '{0}' does not exist.", accountName), "accountName");
            }

            return account;
        }

        public IEnumerable<AzureAccount> ListAccounts(string accountName)
        {
            List<AzureAccount> accounts = new List<AzureAccount>();
            
            if (!string.IsNullOrEmpty(accountName))
            {
                if (Profile.Accounts.ContainsKey(accountName))
                {
                    accounts.Add(Profile.Accounts[accountName]);
                }
            }
            else
            {
                accounts = Profile.Accounts.Values.ToList();
            }

            return Profile.Accounts.Values;
        }

        public AzureAccount RemoveAccount(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentNullException("User name needs to be specified.", "userName");
            }

            if (!Profile.Accounts.ContainsKey(accountId))
            {
                throw new ArgumentException("User name is not valid.", "userName");
            }

            AzureAccount account = Profile.Accounts[accountId];
            Profile.Accounts.Remove(account.Id);

            foreach (AzureSubscription subscription in account.GetSubscriptions(Profile))
            {
                if (subscription.Account == accountId)
                {
                    AzureAccount defaultAccount = GetDefaultAccount(subscription.Id);

                    // There's no default account to use, remove the subscription.
                    if (defaultAccount == null)
                    {
                        // Warn the user if the removed subscription is the default one.
                        if (subscription.IsPropertySet(AzureSubscription.Property.Default))
                        {
                            WriteWarningMessage(Resources.RemoveDefaultSubscription);
                        }

                        // Warn the user if the removed subscription is the current one.
                        if (subscription.Equals(AzureSession.CurrentContext.Subscription))
                        {
                            WriteWarningMessage(Resources.RemoveCurrentSubscription);
                        }

                        Profile.Subscriptions.Remove(subscription.Id);
                    }
                }
            }

            return account;
        }

        private AzureAccount GetDefaultAccount(Guid subscriptionId)
        {
            List<AzureAccount> accounts = ListSubscriptionAccounts(subscriptionId);
            AzureAccount account = accounts.FirstOrDefault(a => a.Type != AzureAccount.AccountType.Certificate);

            if (account != null)
            {
                // Found a non-certificate account.
                return account;
            }

            // Use certificate account if its there.
            account = accounts.FirstOrDefault();

            return account;
        }

        #endregion

        #region Subscripton management

        public void AddOrSetSubscriptions(IEnumerable<AzureSubscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                AddOrSetSubscription(subscription);
            }
        }

        public AzureSubscription AddOrSetSubscription(AzureSubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("Subscription needs to be specified.", "subscription");
            }
            if (subscription.Environment == null)
            {
                throw new ArgumentNullException("Environment needs to be specified.", "subscription.Environment");
            }
            // Validate environment
            GetEnvironmentOrDefault(subscription.Environment);

            if (Profile.Subscriptions.ContainsKey(subscription.Id))
            {
                Profile.Subscriptions[subscription.Id] = MergeSubscriptionProperties(Profile.Subscriptions[subscription.Id], subscription);
            }
            else
            {
                Profile.Subscriptions[subscription.Id] = subscription;
            }
            return Profile.Subscriptions[subscription.Id];
        }

        public AzureSubscription RemoveSubscription(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Subscription name needs to be specified.", "name");
            }

            var subscription = Profile.Subscriptions.Values.FirstOrDefault(s => s.Name == name);

            if (subscription == null)
            {
                throw new ArgumentException(string.Format(Resources.SubscriptionNameNotFoundMessage, name), "name");
            }
            else
            {
                return RemoveSubscription(subscription.Id);
            }
        }

        public AzureSubscription RemoveSubscription(Guid id)
        {
            if (!Profile.Subscriptions.ContainsKey(id))
            {
                throw new ArgumentException(Resources.SubscriptionIdNotFoundMessage, "name");
            }

            var subscription = Profile.Subscriptions[id];

            if (subscription.IsPropertySet(AzureSubscription.Property.Default))
            {
                WriteWarningMessage(Resources.RemoveDefaultSubscription);
            }

            // Warn the user if the removed subscription is the current one.
            if (AzureSession.CurrentContext.Subscription != null && subscription.Id == AzureSession.CurrentContext.Subscription.Id)
            {
                WriteWarningMessage(Resources.RemoveCurrentSubscription);
                AzureSession.SetCurrentContext(null, null, null);
            }

            Profile.Subscriptions.Remove(id);

            // Remove this subscription from its associated AzureAccounts
            List<AzureAccount> accounts = ListSubscriptionAccounts(id);

            foreach (AzureAccount account in accounts)
            {
                account.RemoveSubscription(id);
                if (!account.IsPropertySet(AzureAccount.Property.Subscriptions))
                {
                    Profile.Accounts.Remove(account.Id);
                }
            }

            return subscription;
        }

        public List<AzureSubscription> RefreshSubscriptions(AzureEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            var subscriptionsFromDisk = Profile.Subscriptions.Values.Where(s => s.Environment == environment.Name);
            var subscriptionsFromServer = ListSubscriptionsFromServerForAllAccounts(environment);
            var mergedSubscriptions = MergeSubscriptions(subscriptionsFromDisk.ToList(), subscriptionsFromServer.ToList());

            // Update back Profile.Subscriptions
            foreach (var subscription in mergedSubscriptions)
            {
                Profile.Subscriptions[subscription.Id] = subscription;
            }

            return Profile.Subscriptions.Values.ToList();
        }

        public AzureSubscription GetSubscriptionById(Guid id)
        {
            if (Profile.Subscriptions.ContainsKey(id))
            {
                return Profile.Subscriptions[id];
            }
            else
            {
                throw new ArgumentException(Resources.SubscriptionIdNotFoundMessage, "id");
            }
        }

        public AzureSubscription SetSubscriptionAsCurrent(string name, string accountName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", string.Format(Resources.InvalidSubscription, name));
            }

            AzureSubscription currentSubscription = null;
            var subscription = Profile.Subscriptions.Values.FirstOrDefault(s => s.Name == name);

            if (subscription == null)
            {
                throw new ArgumentException(string.Format(Resources.InvalidSubscription, name), "name");
            }
            else
            {
                currentSubscription = new AzureSubscription { Id = subscription.Id };
                currentSubscription = MergeSubscriptionProperties(subscription, currentSubscription);
                var environment = GetEnvironmentOrDefault(subscription.Environment);
                accountName = string.IsNullOrEmpty(accountName) ? subscription.Account : accountName;
                var account = GetAccount(accountName);
                currentSubscription.Account = account.Id;
                AzureSession.SetCurrentContext(currentSubscription, environment, account);
            }

            return currentSubscription;
        }

        public AzureSubscription SetSubscriptionAsDefault(string name, string accountName)
        {
            AzureSubscription subscription = SetSubscriptionAsCurrent(name, accountName);

            if (subscription != null)
            {
                Profile.DefaultSubscription = subscription;
            }

            return subscription;
        }

        public void ClearDefaultSubscription()
        {
            Profile.DefaultSubscription = null;
        }

        public void ImportCertificate(X509Certificate2 certificate)
        {
            DataStore.AddCertificate(certificate);
        }

        public List<AzureAccount> ListSubscriptionAccounts(Guid subscriptionId)
        {
            return Profile.Accounts.Where(a => a.Value.HasSubscription(subscriptionId))
                .Select(a => a.Value).ToList();
        }

        public List<AzureSubscription> ImportPublishSettings(string filePath, string environmentName)
        {
            var azureEnvironment = GetEnvironmentOrDefault(environmentName);
            var subscriptions = ListSubscriptionsFromPublishSettingsFile(filePath, azureEnvironment);
            if (subscriptions.Any())
            {
                var thumbprint = subscriptions.First().Account;
                Profile.Accounts[thumbprint] = new AzureAccount
                {
                    Id = thumbprint,
                    Type = AzureAccount.AccountType.Certificate
                };
                Profile.Accounts[thumbprint].SetSubscriptions(subscriptions);

                foreach (var subscription in subscriptions)
                {
                    subscription.Properties[AzureSubscription.Property.SupportedModes] =
                        AzureModule.AzureServiceManagement.ToString();

                    AddOrSetSubscription(subscription);
                }
            }
            return subscriptions;
        }

        private List<AzureSubscription> ListSubscriptionsFromPublishSettingsFile(string filePath, AzureEnvironment environment)
        {
            if (string.IsNullOrEmpty(filePath) || !DataStore.FileExists(filePath))
            {
                throw new ArgumentException("File path is not valid.", "filePath");
            }
            return PublishSettingsImporter.ImportAzureSubscription(DataStore.ReadFileAsStream(filePath), environment.Name).ToList();
        }

        private IEnumerable<AzureSubscription> ListSubscriptionsFromServerForAllAccounts(AzureEnvironment environment)
        {
            // Get all AD accounts and iterate
            var principalNames = Profile.Accounts.Keys;

            List<AzureSubscription> subscriptions = new List<AzureSubscription>();

            foreach (var principal in principalNames)
            {
                UserCredentials credentials = new UserCredentials
                {
                    UserName = principal,
                    ShowDialog = ShowDialog.Never
                };

                subscriptions.AddRange(ListSubscriptionsFromServer(ref credentials, environment));
            }

            if (subscriptions.Any())
            {
                return subscriptions;
            }
            else
            {
                return new AzureSubscription[0];
            }
        }

        private IEnumerable<AzureSubscription> ListSubscriptionsFromServer(ref UserCredentials credentials, AzureEnvironment environment)
        {
            try
            {
                IAccessToken commonTenantToken = AzureSession.AuthenticationFactory.Authenticate(environment,
                    ref credentials);

                credentials.ShowDialog = ShowDialog.Never;

                List<AzureSubscription> mergedSubscriptions = MergeSubscriptions(
                    ListServiceManagementSubscriptions(environment, commonTenantToken, ref credentials).ToList(),
                    ListResourceManagerSubscriptions(environment, commonTenantToken, ref credentials).ToList());

                // Set user ID
                foreach (var subscription in mergedSubscriptions)
                {
                    subscription.Environment = environment.Name;
                    subscription.Account = credentials.UserName;
                }

                if (mergedSubscriptions.Any())
                {
                    return mergedSubscriptions;
                }
                else
                {
                    return new AzureSubscription[0];
                }
            }
            catch (AadAuthenticationException aadEx)
            {
                WriteOrThrowAadExceptionMessage(aadEx);
                return new AzureSubscription[0];
            }
        }


        private List<AzureSubscription> MergeSubscriptions(List<AzureSubscription> subscriptionsList1,
            List<AzureSubscription> subscriptionsList2)
        {
            if (subscriptionsList1 == null)
            {
                subscriptionsList1 = new List<AzureSubscription>();
            }
            if (subscriptionsList2 == null)
            {
                subscriptionsList2 = new List<AzureSubscription>();
            }

            Dictionary<Guid, AzureSubscription> mergedSubscriptions = new Dictionary<Guid, AzureSubscription>();
            foreach (var subscription in subscriptionsList1.Concat(subscriptionsList2))
            {
                if (mergedSubscriptions.ContainsKey(subscription.Id))
                {
                    mergedSubscriptions[subscription.Id] = MergeSubscriptionProperties(mergedSubscriptions[subscription.Id],
                        subscription);
                }
                else
                {
                    mergedSubscriptions[subscription.Id] = subscription;
                }
            }
            return mergedSubscriptions.Values.ToList();
        }

        private AzureSubscription MergeSubscriptionProperties(AzureSubscription subscription1, AzureSubscription subscription2)
        {
            if (subscription1 == null || subscription2 == null)
            {
                throw new ArgumentNullException("subscription1");
            }
            if (subscription1.Id != subscription2.Id)
            {
                throw new ArgumentException("Subscription Ids do not match.");
            }
            AzureSubscription mergedSubscription = new AzureSubscription
            {
                Id = subscription1.Id,
                Name = subscription1.Name,
                Environment = subscription1.Environment,
                Account = subscription1.Account ?? subscription2.Account
            };

            // Merge all properties
            foreach (AzureSubscription.Property property in Enum.GetValues(typeof(AzureSubscription.Property)))
            {
                string propertyValue = subscription1.GetProperty(property) ?? subscription2.GetProperty(property);
                if (propertyValue != null)
                {
                    mergedSubscription.Properties[property] = propertyValue;
                }
            }

            // Merge RegisteredResourceProviders
            var registeredProviders = subscription1.GetPropertyAsArray(AzureSubscription.Property.RegisteredResourceProviders)
                    .Union(subscription2.GetPropertyAsArray(AzureSubscription.Property.RegisteredResourceProviders), StringComparer.CurrentCultureIgnoreCase);

            mergedSubscription.SetProperty(AzureSubscription.Property.RegisteredResourceProviders, registeredProviders.ToArray());

            // Merge SupportedMode
            var supportedModes = subscription1.GetPropertyAsArray(AzureSubscription.Property.SupportedModes)
                    .Union(subscription2.GetPropertyAsArray(AzureSubscription.Property.SupportedModes), StringComparer.CurrentCultureIgnoreCase);

            mergedSubscription.SetProperty(AzureSubscription.Property.SupportedModes, supportedModes.ToArray());

            // Merge Tenants
            var tenants = subscription1.GetPropertyAsArray(AzureSubscription.Property.Tenants)
                    .Union(subscription2.GetPropertyAsArray(AzureSubscription.Property.Tenants), StringComparer.CurrentCultureIgnoreCase);

            mergedSubscription.SetProperty(AzureSubscription.Property.Tenants, tenants.ToArray());

            return mergedSubscription;
        }

        private AzureEnvironment MergeEnvironmentProperties(AzureEnvironment environment1, AzureEnvironment environment2)
        {
            if (environment1 == null || environment2 == null)
            {
                throw new ArgumentNullException("environment1");
            }
            if (environment1.Name != environment2.Name)
            {
                throw new ArgumentException("Subscription Ids do not match.");
            }
            AzureEnvironment mergedEnvironment = new AzureEnvironment
            {
                Name = environment1.Name
            };

            // Merge all properties
            foreach (AzureEnvironment.Endpoint property in Enum.GetValues(typeof(AzureEnvironment.Endpoint)))
            {
                string propertyValue = environment1.GetEndpoint(property) ?? environment2.GetEndpoint(property);
                if (propertyValue != null)
                {
                    mergedEnvironment.Endpoints[property] = propertyValue;
                }
            }

            return mergedEnvironment;
        }

        private IEnumerable<AzureSubscription> ListResourceManagerSubscriptions(AzureEnvironment environment, IAccessToken commonTenantToken, ref UserCredentials credentials)
        {
            List<AzureSubscription> result = new List<AzureSubscription>();

            try
            {
                TenantListResult tenants;
                using (var subscriptionClient = AzureSession.ClientFactory.CreateCustomClient<Azure.Subscriptions.SubscriptionClient>(
                    new TokenCloudCredentials(commonTenantToken.AccessToken),
                    environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager)))
                {
                    tenants = subscriptionClient.Tenants.List();
                }

                // Go over each tenant and get all subscriptions for tenant
                foreach (var tenant in tenants.TenantIds)
                {
                    IAccessToken tenantToken = null;
                    // Generate tenant specific token to query list of subscriptions
                    try
                    {
                        tenantToken = AzureSession.AuthenticationFactory.Authenticate(environment, tenant.TenantId, ref credentials);
                    }
                    catch (AadAuthenticationException ex)
                    {
                        var adex = ex.InnerException as AdalException;
                        WarningLog(string.Format(Resources.AddAccountNonInteractiveGuestOrFpo, tenant.TenantId));
                        continue;
                    }

                    if (tenantToken == null) continue;

                    using (var subscriptionClient = AzureSession.ClientFactory.CreateCustomClient<Azure.Subscriptions.SubscriptionClient>(
                            new TokenCloudCredentials(tenantToken.AccessToken),
                            environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager)))
                    {
                        var subscriptionListResult = subscriptionClient.Subscriptions.List();
                        foreach (var subscription in subscriptionListResult.Subscriptions)
                        {
                            AzureSubscription psSubscription = new AzureSubscription
                            {
                                Id = new Guid(subscription.SubscriptionId),
                                Name = subscription.DisplayName,
                                Environment = environment.Name
                            };
                            psSubscription.SetProperty(AzureSubscription.Property.SupportedModes, AzureModule.AzureResourceManager.ToString());
                            psSubscription.SetProperty(AzureSubscription.Property.Tenants, tenant.TenantId);
                            if (commonTenantToken.LoginType == LoginType.LiveId)
                            {
                                AzureSession.SubscriptionTokenCache[Tuple.Create(psSubscription.Id, psSubscription.Account)] = tenantToken;
                            }
                            else
                            {
                                AzureSession.SubscriptionTokenCache[Tuple.Create(psSubscription.Id, psSubscription.Account)] = commonTenantToken;
                            }

                            result.Add(psSubscription);
                        }
                    }
                }
            }
            catch (AadAuthenticationException aadEx)
            {
                WriteOrThrowAadExceptionMessage(aadEx);
            }

            return result;
        }

        private IEnumerable<AzureSubscription> ListServiceManagementSubscriptions(AzureEnvironment environment, IAccessToken commonTenantToken, ref UserCredentials credentials)
        {
            List<AzureSubscription> result = new List<AzureSubscription>();

            try
            {
                using (var subscriptionClient = AzureSession.ClientFactory
                    .CreateCustomClient<WindowsAzure.Subscriptions.SubscriptionClient>(
                        new TokenCloudCredentials(commonTenantToken.AccessToken),
                        environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ServiceManagement)))
                {
                    var subscriptionListResult = subscriptionClient.Subscriptions.List();
                    foreach (var subscription in subscriptionListResult.Subscriptions)
                    {
                        AzureSubscription psSubscription = new AzureSubscription
                        {
                            Id = new Guid(subscription.SubscriptionId),
                            Name = subscription.SubscriptionName,
                            Environment = environment.Name
                        };
                        psSubscription.Properties[AzureSubscription.Property.SupportedModes] = AzureModule.AzureServiceManagement.ToString();
                        psSubscription.SetProperty(AzureSubscription.Property.Tenants, subscription.ActiveDirectoryTenantId);
                        
                        if (commonTenantToken.LoginType == LoginType.LiveId)
                        {
                            AzureSession.SubscriptionTokenCache[Tuple.Create(psSubscription.Id, psSubscription.Account)] =
                                AzureSession.AuthenticationFactory.Authenticate(environment,
                                    subscription.ActiveDirectoryTenantId, ref credentials);
                        }
                        else
                        {
                            AzureSession.SubscriptionTokenCache[Tuple.Create(psSubscription.Id, psSubscription.Account)] = commonTenantToken;
                        }

                        result.Add(psSubscription);
                    }
                }
            }
            catch (AadAuthenticationException aadEx)
            {
                WriteOrThrowAadExceptionMessage(aadEx);
            }

            return result;
        }

        private void WriteOrThrowAadExceptionMessage(AadAuthenticationException aadEx)
        {
            if (aadEx is AadAuthenticationFailedWithoutPopupException)
            {
                WriteDebugMessage(aadEx.Message);
            }
            else if (aadEx is AadAuthenticationCanceledException)
            {
                WriteWarningMessage(aadEx.Message);
            }
            else
            {
                throw aadEx;
            }
        }

        #endregion

        #region Environment management

        public AzureEnvironment GetEnvironmentOrDefault(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return AzureSession.CurrentContext.Environment;
            }
            else if (AzureSession.CurrentContext.Environment != null && AzureSession.CurrentContext.Environment.Name == name)
            {
                return AzureSession.CurrentContext.Environment;
            }
            else if (Profile.Environments.ContainsKey(name))
            {
                return Profile.Environments[name];
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.EnvironmentNotFound, name));
            }
        }

        public List<AzureEnvironment> ListEnvironments(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Profile.Environments.Values.ToList();
            }
            else if (Profile.Environments.ContainsKey(name))
            {
                return new[] { Profile.Environments[name] }.ToList();
            }
            else
            {
                return new AzureEnvironment[0].ToList();
            }
        }

        public AzureEnvironment RemoveEnvironment(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Environment name needs to be specified.", "name");
            }
            if (AzureEnvironment.PublicEnvironments.ContainsKey(name))
            {
                throw new ArgumentException(Resources.RemovingDefaultEnvironmentsNotSupported, "name");
            }
            
            if (Profile.Environments.ContainsKey(name))
            {
                var environment = Profile.Environments[name];
                var subscriptions = Profile.Subscriptions.Values.Where(s => s.Environment == name).ToArray();
                foreach (var subscription in subscriptions)
                {
                    RemoveSubscription(subscription.Id);
                }
                Profile.Environments.Remove(name);
                return environment;
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.EnvironmentNotFound, name), "name");
            }
        }

        public AzureEnvironment AddOrSetEnvironment(AzureEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment needs to be specified.", "environment");
            }

            if (AzureEnvironment.PublicEnvironments.ContainsKey(environment.Name))
            {
                throw new ArgumentException(Resources.ChangingDefaultEnvironmentNotSupported, "environment");
            }

            if (Profile.Environments.ContainsKey(environment.Name))
            {
                Profile.Environments[environment.Name] = 
                    MergeEnvironmentProperties(Profile.Environments[environment.Name], environment);
            }
            else
            {
                Profile.Environments[environment.Name] = environment;
            }

            return Profile.Environments[environment.Name];
        }
        #endregion
    }
}