#!/bin/sh

echo Starting install.sh

LATEST_RELEASE=$(curl -L -H 'Accept: application/json' https://github.com/MadSciencist/SmartHomeAPI/releases/latest)
echo "latest release: $LATEST_RELEASE"

LATEST_VERSION=$(echo $LATEST_RELEASE | sed -e 's/.*"tag_name":"\([^"]*\)".*/\1/')
echo "Latest version: $LATEST_VERSION"

ARTIFACT_URL="https://github.com/MadSciencist/SmartHomeAPI/releases/download/$LATEST_VERSION/smart_home_api_release_linux_arm.tar.gz"
echo "Artifact URL: $ARTIFACT_URL"

wget $ARTIFACT_URL
