Write-Host "Clearing BIN and OBJ files";

$path = $PSScriptRoot;

Get-ChildItem ${path} -include bin,obj,packages -Recurse | foreach ($_) {
    "Cleaning: " + $_.fullname
    remove-item $_.fullname -Force -Recurse -ErrorAction SilentlyContinue -Verbose
}
