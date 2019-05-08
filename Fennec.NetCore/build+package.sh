#!/bin/bash
rm -rf Packages
dotnet build
dotnet pack -p:PackageVersion=0.1.0
