#!/bin/bash

echo "================================================"
echo "Elemental Underground - Build Script"
echo "================================================"

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore Cascade.sln

# Build solution
echo "Building solution..."
dotnet build Cascade.sln --configuration Release

# Run tests
echo "Running unit tests..."
dotnet test Cascade.Tests/Cascade.Tests.csproj --configuration Release

echo "================================================"
echo "Build complete!"
echo "To run the game: dotnet run --project Cascade.Game"
echo "================================================"
