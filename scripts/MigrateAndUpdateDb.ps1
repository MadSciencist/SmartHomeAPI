param ([string]$migrationName = $( Read-Host "Enter migration name" ))

Write-Host 'Migrating app Identity DB schema...'
dotnet ef migrations add $migrationName --project ..\src\SmartHome.Core.Data\SmartHome.Core.Data.csproj --context EntityFrameworkContext --verbose --prefix-output
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

Write-Host 'Updating app Identity DB schema...' 
dotnet ef database update --project ..\src\SmartHome.Core.Data\SmartHome.Core.Data.csproj --context SmartHome.Core.Data.EntityFrameworkContext --verbose --prefix-output
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'
