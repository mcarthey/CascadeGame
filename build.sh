#!/bin/bash

echo "================================================"
echo "Elemental Underground - Build Script"
echo "================================================"

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore FluidGame.sln

# Build solution
echo "Building solution..."
dotnet build FluidGame.sln --configuration Release

# Run tests
echo "Running unit tests..."
dotnet test FluidGame.Tests/FluidGame.Tests.csproj --configuration Release

echo "================================================"
echo "Build complete!"
echo "To run the game: dotnet run --project FluidGame.Game"
echo "================================================"
