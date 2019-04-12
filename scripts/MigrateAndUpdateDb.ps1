 param ([string]$migrationName = $( Read-Host "Enter migration name" ))

Write-Host 'Migrating app Identity DB schema...'
dotnet ef migrations add $migrationName --project ..\SmartHome.API --context AppIdentityDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

#Write-Host 'Migrating app DB schema...'
#dotnet ef migrations add Initial2 --project ..\SmartHome.API --context AppDbContext --verbose
#Write-Host '*****************'
#Write-Host 'DONE'
#Write-Host '*****************'

Write-Host 'Updating app Identity DB schema...'
dotnet ef database update --project ..\SmartHome.API --context SmartHome.API.Persistence.Identity.AppIdentityDbContext --verbose
Write-Host '*****************'
Write-Host 'DONE'
Write-Host '*****************'

#Write-Host 'Updating app DB schema...'
#dotnet ef database update --project ..\SmartHome.API\ --context SmartHome.API.Persistence.App.AppDbContext --verbose
#Write-Host '*****************'
#Write-Host 'DONE'
#Write-Host '*****************'