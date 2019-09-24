#!/bin/bash
rm -rf Packages
dotnet build
dotnet pack -p:PackageVersion=0.1.0

#remove old version and add new package to local repo.
dotnet tool uninstall --global fennec.netcore
rm -rf ../../Packages/fennec.netcore
nuget add ./Packages/Fennec.NetCore.0.1.0.nupkg -s ../../Packages

#clear all local nuget cache & install global tool.
dotnet nuget locals all --clear
dotnet tool install --global fennec.netcore