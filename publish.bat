nuget sources Remove -Name Repository -Source http://
nuget sources Add -Name Repository -Source http:// -username "" -password ""
nuget setapikey : -Source Repository
nuget pack application.nuspec
nuget push demo.1.0.0.nupkg -Source Repository