﻿[CmdletBinding()]
param
(
    # Directory or file to do token replacement within.
    [Parameter(Mandatory = $true)]
    [string] $path,

    # Filenames (or patterns) to use for selecting files to replace tokens within, if the Path parameter is a directory.
    # Optional if the Path parameter already specifies a file.
    [Parameter(Mandatory = $false)]
    [string] $filenames,

    # Whether the search for files is recursive (defaults to no). Ignored if the Path parameter specifies a file.
    [Parameter(Mandatory = $false)]
    [switch] $recursive = $false,

    # Allows overriding the token start string, for custom file formats where the default is problematic.
    [Parameter(Mandatory = $false)]
    [string] $tokenStart = "__",

    # Allows overriding the token end string, for custom file formats where the default is problematic. Regex syntax.
    [Parameter(Mandatory = $false)]
    [string] $tokenEnd = "__",

    # A list of secret-values variables ("key=value","key=value") to include in replacement. Secret build
    # process variables are not available for replacement without explicitly providing them here. Regex syntax.
	# Variables names here must use underscored instead of dot, even if in content files the dot is used.
    [Parameter(Mandatory = $false)]
    [Alias("variables")]
    [string[]] $secrets
)

# Forked from https://github.com/TotalALM/VSTS-Tasks/tree/master/Tasks/Tokenization and heavily modified.

$ErrorActionPreference = "Stop"

. "$PSScriptRoot\Functions.ps1"

# Assemble all the secrets into a hashtable for easy use later.
$secretValues = @{}

if ($secrets -and $secrets.Length -gt 0) {
    Write-Host "$($secrets.Length) secret variables provided on command line."

    foreach ($s in $secrets) {
        $pair = @($s -split "=", 2)

        if ($pair.Length -ne 2) {
            Write-Error "A secret value parameter was not formatted as key=value."
        }
        else {
            $secretValues[$pair[0]] = $pair[1]

            Write-Host "Obtained secret value with name $($pair[0])"
        }
    }
}

# Prepare for string processing.
$patterns = @()
$regex = $TokenStart + "[A-Za-z0-9._]+" + $TokenEnd
$matches = @()

Write-Host "Regex: $regex"

function ProcessFile($file) {
    $fileFullName = $file.FullName

    Write-Host "Found file: $fileFullName"

    $fileEncoding = Get-FileEncoding($fileFullName)

    Write-Host "Detected file encoding: $fileEncoding"

    $newlines = Get-NewlineCharacters($fileFullName)

    $matches = select-string -Path $fileFullName -Pattern $regex -AllMatches | % { $_.Matches } | % { $_.Value }

    if ($matches.Count -eq 0) {
        Write-Host "No placeholders in the file."
        return
    }

    $fileContent = Get-Content $fileFullName

    foreach ($match in $matches) {
        $matchedItem = $match
        $matchedItem = $matchedItem.TrimStart($TokenStart)
        $matchedItem = $matchedItem.TrimEnd($TokenEnd)
        $matchedItem = $matchedItem -replace "\.", "_"

        Write-Host "Found token $matchedItem" -ForegroundColor Green

        if (Test-Path Env:$matchedItem) {
            $matchValue = (Get-ChildItem Env:$matchedItem).Value

            Write-Host "Found matching variable. Value: $matchValue" -ForegroundColor Green
        }
        elseif ($secretValues.ContainsKey($matchedItem)) {
            $matchValue = $secretValues[$matchedItem]

            Write-Host "Found matching secret variable." -ForegroundColor Green
        }
        else {
            $matchValue = ""

            Write-Host "Found no matching variable. Replaced with empty string." -ForegroundColor Green
        }

        $fileContent = $fileContent | Foreach-Object { $_ -replace $match, $matchValue }
    }

    $newContentAsString = [string]::Join($newlines, $fileContent)

    # If file is UTF-8 and has no BOM, make sure there is also no BOM in the newly created file.
    if ($fileEncoding -eq "UTF8") {
        if ($PSVersionTable.PSVersion.Major -le 5) {
            $bytes = Get-Content $fileFullName -Encoding Byte -TotalCount 3
        }
        else {
            $bytes = Get-Content $fileFullName -AsByteStream -TotalCount 3
        }

        if ($bytes[0] -eq 0xef -and $bytes[1] -eq 0xbb -and $bytes[2] -eq 0xbf) {
            Write-Verbose "Writing UTF8 file with BOM."
            Set-Content $fileFullName -Force -Encoding $fileEncoding -Value $newContentAsString -NoNewline
        }
        else {
            Write-Verbose "Writing UTF8 file without BOM."
            [IO.File]::WriteAllText($fileFullName, $newContentAsString)
        }
    }
    else {
        Write-Verbose "Writing $fileEncoding file."
        Set-Content $fileFullName -Force -Encoding $fileEncoding -Value $newContentAsString -NoNewline
    }
}

Write-Host "Target path: $path"
Write-Host "Recursive: $recursive"

if (Test-Path -PathType Leaf $path) {
    Write-Host "Target is a file. Will proceed directly to perform token replacement on it."

    Get-Item -Path $path | ForEach-Object { ProcessFile $_ }
}
elseif (Test-Path -PathType Container $path) {
    Write-Host "Target is a directory. Will replace tokens in contents."

    foreach ($target in $filenames.Split(",;".ToCharArray())) {
        Write-Host "Targeted filename or pattern: $target"

        if ($recursive) {
            Get-ChildItem -Path $path -File -Filter $target -Recurse | ForEach-Object { ProcessFile $_ }
        }
        else {
            Get-ChildItem -Path $path -File -Filter $target | ForEach-Object { ProcessFile $_ }
        }
    }
}
else {
    Write-Error "Target path points to item that does not exist."
}