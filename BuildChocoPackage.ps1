.\packages\NuGet.CommandLine.2.8.3\tools\nuget.exe pack .\powergist.nuspec

#For post build event using this adapted
#"$(TargetDir)..\..\..\packages\NuGet.CommandLine.2.8.3\tools\nuget.exe" pack "$(TargetDir)\..\..\..\Powergist.nuspec"