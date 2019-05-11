$migrationsDirectory = Join-Path -Path $PSScriptRoot -ChildPath "..\src\SmartHome.Core.DataAccess\Migrations"
Write-Host $migrationsDirectory

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

function Invoke-MySQL {
Param(
  [Parameter(
  Mandatory = $true,
  ParameterSetName = '',
  ValueFromPipeline = $true)]
  [string]$Query
  )

$ConnectionString = "server=localhost;port=3306;userid=root;password=;SslMode=none;CharSet=utf8"
$libPath = Join-Path -Path $PSScriptRoot -ChildPath "..\lib\MySql.Data.dll"

Try {
  [void][System.Reflection.Assembly]::LoadFrom($libPath)
  $Connection = New-Object MySql.Data.MySqlClient.MySqlConnection
  $Connection.ConnectionString = $ConnectionString
  $Connection.Open()

  $Command = New-Object MySql.Data.MySqlClient.MySqlCommand($Query, $Connection)
  $DataAdapter = New-Object MySql.Data.MySqlClient.MySqlDataAdapter($Command)
  $DataSet = New-Object System.Data.DataSet
  $RecordCount = $dataAdapter.Fill($dataSet, "data")
  $DataSet.Tables[0]
  }

Catch {
  throw "ERROR : Unable to run query : $query `n$Error[0]"
 }

Finally {
  $Connection.Close()
  }
 }

Write-Host 'Executing DB Drop'
Invoke-MySQL -Query "DROP DATABASE IF EXISTS `smarthomedb`;"
Write-Host 'DB Drop executed sucesfully'

Write-Host "Running initial migration"
$addInitial = Join-Path -Path $PSScriptRoot -ChildPath "MigrateAndUpdateDb.ps1 -migrationName 'init'"
Invoke-Expression $addInitial

