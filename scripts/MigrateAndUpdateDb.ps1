 param ([string]$migrationName = $( Read-Host "Enter migration name" ))

Write-Host 'Migrating app Identity DB schema...'
dotnet ef migrations add $migrationName --project ..\src\SmartHome.Core.DataAccess --context AppDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

Write-Host 'Updating app Identity DB schema...'
dotnet ef database update --project ..\src\SmartHome.Core.DataAccess --context SmartHome.Core.DataAccess.AppDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'
