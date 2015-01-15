$path = Split-Path -Parent $MyInvocation.MyCommand.Definition
$iseProfile = "$pshome\Microsoft.PowerShellISE_profile.ps1"
$line1 = "Add-Type -Path '$path\GripDev.PowerGist.Addin.dll'“
$line2 = "`$psISE.CurrentPowerShellTab.VerticalAddOnTools.Add('PowerGist', [GripDev.PowerGist.Addin.PowerGistPanel], `$true) "
Add-Content $iseProfile $line1 -Force
Add-Content $iseProfile $line2 -Force