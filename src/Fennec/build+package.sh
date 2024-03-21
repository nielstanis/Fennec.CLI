#!/bin/bash
rm -rf Packages
dotnet build
dotnet pack -p:PackageVersion=0.7.0

#remove old version and add new package to local repo.
dotnet tool uninstall --global fennec
rm -rf ../../Packages/fennec
nuget add ./Packages/Fennec.0.7.0.nupkg -s ../../Packages

#clear all local nuget cache & install global tool.
dotnet nuget locals all --clear
dotnet tool install --global fennec
