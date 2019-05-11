param ([string]$migrationName = $( Read-Host "Enter migration name" ))

Write-Host 'Migrating app Identity DB schema...'
dotnet ef migrations add $migrationName --project ..\src\SmartHome.Core.DataAccess\SmartHome.Core.DataAccess.csproj --context AppDbContext --verbose --prefix-output
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

Write-Host 'Updating app Identity DB schema...' 
dotnet ef database update --project ..\src\SmartHome.Core.DataAccess\SmartHome.Core.DataAccess.csproj --context SmartHome.Core.DataAccess.AppDbContext --verbose --prefix-output
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'
