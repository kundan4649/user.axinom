[CmdletBinding()]
Param(
    # Root folder of Configuration Value Files.
    # Not mandatory because one might want to make use of only environment variables for configuration.
    [Parameter(Mandatory = $False)]
    [string]$appSettingsFile,

    [Parameter(Mandatory = $True)]
    [string]$azureResourceGroup,

    [Parameter(Mandatory = $True)]
    [string]$azureSiteName
)

<#
This script sets app settings in an azure function app from a json file. The script will add any variables that are missing from the server and update any that are changed.
#>

$ErrorActionPreference = "Stop"

# Adds app settings that are missing in azure
function SetAzureAppSettings($configuration) {
    Write-Host "Connecting to azure web app: $azureResourceGroup : $azureSiteName"
    $webApp = Get-AzureRMWebApp -ResourceGroupName $azureResourceGroup -Name $azureSiteName

    $appSettingList = $webApp.SiteConfig.AppSettings
    $appSettings = @{}
    ForEach ($kvp in $appSettingList) {
        $appSettings[$kvp.Name] = $kvp.Value
    }

    $appSettingsChanged = $False
    
    ForEach ($name in $configuration.Keys) {
        $value = $configuration[$name]
        
        if (-Not $appSettings.ContainsKey($name)) {
            Write-Host "Found missing configuration variable. Setting $name to '$value'" -ForegroundColor Green
            $appSettingsChanged = $True
        }
		elseif ($appSettings[$name] -ne $value) {
            Write-Host "Found changed configuration variable. Setting $name to '$value'" -ForegroundColor Green
            $appSettingsChanged = $True
        }
		else {
			Write-Host "Found unchanged configuration variable. $name = '$value'" -ForegroundColor Green
		}

		$appSettings[$name] = $value
    }

    if ($appSettingsChanged -eq $True) {
        Write-Host "Saving app settings: $azureResourceGroup : $azureSiteName"
        Set-AzureRMWebApp -ResourceGroupName $azureResourceGroup -Name $azureSiteName -AppSettings $appSettings
    }
    else {
        Write-Host "No app settings changed"
    } 
}

# Reads configuration values from a json file
function Get-AppSettingsFromFile([string] $appSettingsFile) {
	$configObject = (Get-Content $appSettingsFile) | ConvertFrom-Json
	$hashtable = @{}

	foreach( $property in $configObject.psobject.properties.name )
	{
		$hashtable[$property] = $configObject.$property
	}

	return $hashtable
}

# Write all configuration values to azure web app
if ((Test-Path $appSettingsFile) -eq $False) {
    Write-Error "Configuration file: '$appSettingsFile' not found. Exiting."
}

$configuration = Get-AppSettingsFromFile($appSettingsFile)
SetAzureAppSettings($configuration)

Write-Host "Done."