#!/usr/bin/env bash

ROOTDIR=$(pwd)

echo "========== Clean up old artifacts =========="

dotnet clean

if [ -d $ROOTDIR/dist ]; then
    rm -r ./dist
fi

echo "=========== Restore dependencies ==========="

dotnet restore

echo "============== Build projects =============="

dotnet build

echo "================ Run tests ================="

dotnet test $ROOTDIR/tests/*/*.csproj

echo "=========== Create nuget package ==========="

dotnet pack $ROOTDIR/src/Genki/Genki.csproj -c Production -o $ROOTDIR/dist