#!/usr/bin/env bash

# Clean up old artifacts
rm -r **/*/bin/
rm -r **/*/obj/

# Restore dependencies
dotnet restore

# Build projects
dotnet build **/project.json

# Run tests
dotnet test ./tests/**/