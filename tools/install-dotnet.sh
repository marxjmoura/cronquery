#!/bin/bash

set -e

version=$1
tar_gz="https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$version/dotnet-sdk-$version-linux-x64.tar.gz"

curl -SL -o dotnet.tar.gz $tar_gz
sudo mkdir -p /usr/share/dotnet
sudo ln -sfn /usr/share/dotnet/dotnet /usr/bin/dotnet
