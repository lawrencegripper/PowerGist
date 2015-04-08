# PowerGist

![status badge](https://ci.appveyor.com/api/projects/status/github/lawrencegripper/PowerGist)

An addin for the Powershell ISE to allow quick access, update and creation of Gists

#Install 

Get Chocolatey (awesome command line installer for windows)

Open powershell as administrator and type:

C:\> choco install powergist

See this blog post for more details http://wp.me/p1He68-au

*Warning – This was a quick project and should be considered alpha quality. Feel free to submit fixes and enhancements in a pull request*

#Debugging 

1. Enable first chance exceptions. 
2. Under debug for the project set to start external application "C:\Windows\System32\WindowsPowerShell\v1.0\powershell_ise.exe"
3. Set commandline arguments to "-noprofile -file c:\your\path\to\repository\PowerGistInstallerDebug.ps1"
