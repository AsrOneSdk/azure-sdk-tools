# ----------------------------------------------------------------------------------
#
# Copyright Microsoft Corporation
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------------

########################## Recovery Services End to End Scenario Tests #############################

<#
.SYNOPSIS
Recovery Services End to End
#>
function Test-RecoveryServicesEndToEnd
{
	Import-AzureSiteRecoveryVaultSettingsFile 'c:\Users\sriramvu\Desktop\rijethma-vault_Thursday, September 4, 2014.vaultcredentials'
	# $vaultSettings = Get-AzureSiteRecoveryVaultSettings
	# $servers = Get-AzureSiteRecoveryServer
	$protectionContainers = Get-AzureSiteRecoveryProtectionContainer
	Assert-True { $protectionContainers.Count -gt 0 }
	Assert-NotNull($protectionContainers)
	foreach($protectionContainer in $protectionContainers)
	{
		Assert-NotNull($protectionContainer.Name)
	}
	# Assert.True($protectionContainers.All(protectedContainer => !string.IsNullOrEmpty(protectedContainer.Name)), "Protection Container name can't be null or empty");
	# Assert.True($protectionContainers.All(protectedContainer => !string.IsNullOrEmpty(protectedContainer.ID)), "Protection Container Id can't be null or empty");
	# Assert-AreEqual 'sriramvuVault1' $vaultSettings1.ResourceName
	# Assert-AreEqual "" $vaultSettings.ResourceName
}