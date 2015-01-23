$packageName = 'powergist'

$path = Split-Path -Parent $MyInvocation.MyCommand.Definition

$pathLib = "$path\..\lib\"

$iseInstances = get-process | ?{$_.ProcessName -like "powershell_ise"}

if ($iseInstances.Length -gt 0)
{
    Write-Host "ISE Running, recomend stopping before install"
}


$addinDll = "$pathLib\GripDev.PowerGist.Addin.dll"
$depDll = "$pathLib\GistsApi.dll"
$iseProfile = "$pshome\Microsoft.PowerShellISE_profile.ps1"

Copy-Item $addinDll $pshome -Force 
Copy-Item $depDll $pshome -Force 

$line1 = "Add-Type -Path '$pshome\GripDev.PowerGist.Addin.dll'"
$line2 = "Add-Type -Path '$pshome\GistsApi.dll'"
$line3 = "`$psISE.CurrentPowerShellTab.VerticalAddOnTools.Add('PowerGist', [GripDev.PowerGist.Addin.PowerGistPanel], `$true)"

$iseProfileExists = Test-Path $iseProfile

if ($iseProfileExists)
{
	$containsText = Select-String -Path $iseProfile -Pattern "PowerGist" `
					| Select-Object -First 1 -ExpandProperty Matches `
					| Select-Object -ExpandProperty Success
}
else
{
	New-Item $iseProfile -type file
	$containsTest = $false
}

if ($containsText)
{
	Write-Host "*****************************************************"
	Write-Host "ISE Profile configuration already setup, manually check the following is present in: " + $iseProfile
	Write-Host $line1
	Write-Host $line2
	Write-Host $line3
	Write-Host "*****************************************************"
}
else
{
	Add-Content $iseProfile $line1 -Force
	Add-Content $iseProfile $line2 -Force
	Add-Content $iseProfile $line3 -Force
}

Write-ChocolateySuccess $packageName