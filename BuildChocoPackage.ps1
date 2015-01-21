.\packages\NuGet.CommandLine.2.8.3\tools\nuget.exe pack .\PowerGist.Install.1.0.0.nuspec

#For post build event using this adapted
#"$(TargetDir)..\..\..\packages\NuGet.CommandLine.2.8.3\tools\nuget.exe" pack "$(TargetDir)\..\..\..\Powergist.nuspec"