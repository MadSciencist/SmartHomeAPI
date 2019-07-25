#!/bin/sh

echo Starting deploy.sh

dotnet publish ./src/SmartHome.API/SmartHome.API.csproj -c Release -r linux-arm

tar -czvf smart_home_api_release_linux_arm.tar.gz ./src/SmartHome.API/bin/Release/netcoreapp2.2/linux-arm/publish/

echo Done deploy.sh
