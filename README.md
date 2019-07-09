# SmartHomeAPI
An open-source home automation system server.

### Build status:
Master:
[![Build Status](https://travis-ci.org/MadSciencist/SmartHomeAPI.svg?branch=master)](https://travis-ci.org/MadSciencist/SmartHomeAPI)

Develop:
[![Build Status](https://travis-ci.org/MadSciencist/SmartHomeAPI.svg?branch=develop)](https://travis-ci.org/MadSciencist/SmartHomeAPI)

Metrics:
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=MadSciencist_SmartHomeAPI&metric=ncloc)](https://sonarcloud.io/dashboard?id=MadSciencist_SmartHomeAPI)


### Dev setup
prerequisites:
  * MySQL (easiest way is to use XAMMP or docker)
  
1) Configure MySQL to work with following settings:
      port=3306
      userid=root
      password=<none>
2) Rebuild solution
3) Run scripts/RenewDb1.ps1 (for the first time, during development use MigrateAndUpdate.ps1)
4) Run src/SmartHome.API (from VS or by using dotnet run)
