$migrationsDirectory = Join-Path -Path $PSScriptRoot -ChildPath "..\src\SmartHome.Core.DataAccess"

If(Test-Path -Path $migrationsDirectory)
{
    Write-Host 'Cleaning migrations'
    Remove-Item $migrationsDirectory -Recurse
    Write-Host 'Migrations cleaned'
}
else 
{
    Write-Host 'No migrations'
}