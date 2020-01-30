# Fennec.NetCore API Dumper

![Fennec](fennec.png)

.NET Core global CLI tool used to dump API's of .NET assembly/assemblies


## Install

The `fennec.netcore` nuget package is [published to nuget.org](https://www.nuget.org/packages/fennec.netcore/)

You can get the tool by running this command

`$ dotnet tool install -g fennec.netcore`

## Usage

    Fennec.NetCore - .NET Core API dumper 0.4.0

    Dump used API's of .NET assembly/assemblies.

    Usage:  [options] <assembly>

    Arguments:
    assembly                List of assemblies or file pattern

    Options:
    -h|--help               Show help information
    -v|--version            Show version information
    -o|--output[:<FOLDER>]  Output Folder
    -f|--format[:<FORMAT>]  File Format, either JSON or FXT

## Versions

- 0.4.0 second released version for NDC London 2020
- 0.1.0 first released version at Microsoft Build 2019
