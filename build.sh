#!/usr/bin/env bash

# Clean up old artifacts
dotnet clean

# Restore dependencies
dotnet restore

# Build projects
dotnet build

# Run tests
dotnet test ./tests/*/*.csproj