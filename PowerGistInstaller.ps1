add-type -path ".\GripDev.PowerGist.Addin.dll"

$psISE.CurrentPowerShellTab.VerticalAddOnTools.Add('PowerGist', [GripDev.PowerGist.Addin.PowerGistPanel], $true)