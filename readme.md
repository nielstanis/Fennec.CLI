# Fennec.NetCore API Dumper

![Fennec](fennec.png)

.NET Core global CLI tool used to dump API's of .NET assembly/assemblies


## Install

The `fennec.netcore` nuget package is [published to nuget.org](https://www.nuget.org/packages/fennec.netcore/)

You can get the tool by running this command

`$ dotnet tool install -g fennec.netcore`

## Usage

    Usage: fennec.netcore -o=[dir] [assembly...|filepattern...]

    dir:
        Output folder to write content to (optional)
    assembly:
        Single assembly name
    filepattern:
        file pattern that matches a set of files..

## Versions

- 0.1.0 first released version at Microsoft Build 2019
