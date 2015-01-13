add-type -path "C:\Users\lagrippe\Documents\Visual Studio 14\Projects\GripDev.PowerGist\GripDev.PowerGist.Addin\bin\Debug\GripDev.PowerGist.Addin.dll"

$psISE.CurrentPowerShellTab.VerticalAddOnTools.Add('PowerGist', [GripDev.PowerGist.Addin.UserControl1], $true)