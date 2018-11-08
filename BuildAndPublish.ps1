Write-Host "Ensure the CSPROJ file(s) have been updated first!"
Write-Host "Press enter to continue..."

Read-Host

Push-Location .\src\CBot.Bot

Write-Host "Removing previous nuget packages"
Remove-Item .\bin\Release\*.nupkg > $null

Write-Host "Building CBot"
dotnet pack --configuration Release
#msbuild /t:pack /p:Configuration=Release
#nuget pack CBot.Bot.csproj -IncludeReferencedProjects -Prop Configuration=Release #Works but would need spec file

$nugetPackage = Get-ChildItem .\bin\Release\*.nupkg | Select-Object -First 1

Write-Host "Publishing package:$nugetPackage"
nuget push $nugetPackage -Source https://api.nuget.org/v3/index.json

Pop-Location