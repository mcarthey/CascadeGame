# Setup Guide - Elemental Underground

This guide will help you get the project running on your machine.

## Prerequisites

### Required

1. **.NET 8.0 SDK or later**
   - Download: https://dotnet.microsoft.com/download
   - Verify installation: `dotnet --version`

2. **Stride Engine 4.2**
   - Download Stride Launcher: https://stride3d.net/download/
   - Install Stride 4.2.0 or later through the launcher

### Recommended IDE

- **Visual Studio 2022** (Windows) with Game Development workload
- **JetBrains Rider** (Cross-platform) - excellent for Stride development
- **VS Code** with C# extension (lightweight option)

## Quick Start

### 1. Clone and Restore

```bash
git clone <repository-url>
cd FluidGame
dotnet restore
```

### 2. Build

```bash
# Build everything
dotnet build FluidGame.sln --configuration Debug

# Or use the build script
chmod +x build.sh
./build.sh
```

### 3. Run Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

Expected output: All tests should pass ✅

### 4. Run the Game

```bash
dotnet run --project FluidGame.Game
```

**Expected Result:**
- Window opens showing 10,000 colored particles (blue-ish water)
- Particles fall due to gravity
- FPS counter in top-left corner shows ~60 FPS
- Particles wrap around screen edges

## Troubleshooting

### "Stride packages not found"

If you see Stride package errors during restore:

1. Open Stride Launcher
2. Ensure Stride 4.2.x is installed
3. Note the installation path
4. Add Stride's NuGet feed to your config:

```bash
dotnet nuget add source "C:\Program Files\Stride\Stride-4.2\bin\packages" --name "Stride Local"
```

Replace the path with your actual Stride installation directory.

### "Graphics device not available"

This usually means Stride can't initialize graphics. Ensure:
- Your GPU drivers are up to date
- You have DirectX 11+ (Windows) or Vulkan support
- Try running in Administrator mode (Windows)

### "Font not loaded" / Blank FPS counter

The performance monitor will work even without fonts - it just won't display text. To fix:

1. In Stride Editor, create a SpriteFont asset named "StrideDefaultFont"
2. Or modify `PerformanceMonitorSystem.cs` to handle missing fonts gracefully (already implemented)

### Tests fail with "Stride runtime required"

This is **critical** - if tests require Stride runtime, our architecture is broken!

Tests in `FluidGame.Tests/Core.Tests/` should run **without** Stride. They test pure domain logic.

If you see this error:
1. Check that test project only references `FluidGame.Core`
2. Ensure no Stride packages in test project `.csproj`
3. Domain code should have ZERO Stride dependencies

## Project Structure Verification

Run this to verify structure:

```bash
tree -I 'bin|obj|.git'
```

Expected:
```
.
├── FluidGame.Core/              ← Pure C#, no Stride!
├── FluidGame.Infrastructure/    ← Stride implementations
├── FluidGame.Game/              ← Game entry point
├── FluidGame.Tests/             ← Pure C# tests
└── FluidGame.sln
```

## Development Workflow

### Adding a New Feature

1. **Start with domain logic** in `FluidGame.Core/`
2. **Write tests first** in `FluidGame.Tests/Core.Tests/`
3. **Implement in Core** (pure C#)
4. **Create Stride adapter** in `FluidGame.Infrastructure/`
5. **Wire up in bootstrapper** (`GameBootstrapper.cs`)

### Example: Adding Wind Force

```csharp
// 1. Add to Core/Domain/Physics
public interface IForce
{
    Vector2 Calculate(Particle particle);
}

// 2. Test in Tests/Core.Tests
[Fact]
public void Wind_ShouldPushParticles_Horizontally() { }

// 3. Implement
public class WindForce : IForce
{
    public Vector2 Calculate(Particle p) => new Vector2(50, 0);
}

// 4. Integrate in Infrastructure (if needed)
// 5. Register in GameBootstrapper
```

## Performance Benchmarks

After running, verify these targets:

| Metric | POC Target | Final Target |
|--------|-----------|--------------|
| Particles | 10,000 | 32,768 |
| FPS | 60 | 60 |
| Update Time | <16ms | <16ms |
| Memory | <100MB | <200MB |

Check the FPS counter in-game to verify performance.

## Next Steps

Once you have the POC running:

1. Read `README.md` for architecture details
2. Explore `FluidGame.Core/` to understand domain model
3. Check out the tests to see TDD in action
4. Review Week 3-4 roadmap in README
5. Start implementing Verlet integration (Week 3)

## Getting Help

- **Architecture questions**: See linked Claude conversation in README
- **Stride-specific**: https://doc.stride3d.net/
- **Build issues**: Check `.csproj` files for correct package versions
- **Performance**: Profile with `dotnet-trace` or Visual Studio profiler

## Validation Checklist

Before you start developing:

- [ ] `dotnet restore` completes without errors
- [ ] `dotnet build` succeeds for all projects
- [ ] `dotnet test` shows all tests passing
- [ ] Game window opens and shows particles
- [ ] FPS counter visible and showing ~60 FPS
- [ ] Particles fall due to gravity
- [ ] No Stride dependencies in `FluidGame.Core.csproj`
- [ ] Tests run without game engine (pure C#)

If all boxes are checked, you're ready to develop! 🚀
