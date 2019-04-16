 param ([string]$migrationName = $( Read-Host "Enter migration name" ))

Write-Host 'Migrating app Identity DB schema...'
dotnet ef migrations add $migrationName --project ..\src\SmartHome.API --context AppIdentityDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

Write-Host 'Updating app Identity DB schema...'
dotnet ef database update --project ..\src\SmartHome.API --context SmartHome.API.Persistence.AppIdentityDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'
