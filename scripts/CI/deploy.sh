#!/bin/sh

echo Starting deploy.sh
output_file="smart_home_api_release_linux_arm.tgz"

dotnet publish ./src/SmartHome.API/SmartHome.API.csproj -c Release -r linux-arm

tar -czvf $output_file -C ./src/SmartHome.API/bin/Release/netcoreapp2.2/linux-arm/publish .

echo Done deploy.sh
