# Quick Start - Elemental Underground

Get up and running in 5 minutes!

## Prerequisites Check

```bash
# Check .NET version (need 8.0+)
dotnet --version

# Check Stride installation
# Stride should be installed via Stride Launcher
# Download from: https://stride3d.net/download/
```

## Build and Run

```bash
# 1. Restore packages
dotnet restore

# 2. Build solution
dotnet build

# 3. Run tests (should complete in < 1 second)
dotnet test

# 4. Run the game
dotnet run --project FluidGame.Game
```

## What You Should See

When the game runs:

```
┌────────────────────────────────────────┐
│  FPS: 60                               │
│  Particles: 10,000                     │
│  Target: 60 FPS                        │
│                                        │
│                                        │
│         ··  ·  ·   ·  ··              │
│        ·  ·  ·  ·  · ·  ·             │
│       ·   ·  ·   · ·  ·  ·            │
│         ·  ·  ·  ·  ·   ·             │
│       (10,000 blue particles falling)  │
│                                        │
│                                        │
└────────────────────────────────────────┘
```

- **Blue particles**: Water simulation
- **Falling motion**: Gravity at work
- **Wrapping**: Particles reappear at top when they fall off bottom
- **60 FPS**: Smooth animation

## Controls

- **ESC**: Exit game

## Troubleshooting

### "Stride packages not found"

Add Stride's package source:

```bash
# Windows
dotnet nuget add source "C:\Program Files\Stride\Stride-4.2\bin\packages" --name Stride

# Adjust path to your Stride installation
```

### "Tests fail"

This is critical! Tests should run without Stride runtime.

```bash
# Make sure you're in the repo root
cd /path/to/CascadeGame

# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed"
```

If tests fail, check:
1. .NET 8.0 SDK is installed
2. FluidGame.Tests.csproj doesn't reference Stride
3. FluidGame.Core.csproj has no Stride dependencies

### "Game doesn't start"

1. Check Stride is installed
2. Update graphics drivers
3. Try running as administrator (Windows)

## Next Steps

### Week 3-4: Upgrade Physics

```bash
# Create a new branch for Verlet integration
git checkout -b feature/verlet-integration

# Implement VerletPhysicsEngine in FluidGame.Core/Domain/Physics/
# Test it thoroughly
# Swap it in GameBootstrapper.cs
```

### Week 5-6: Add Spatial Hashing

```bash
# Implement grid-based collision detection
# Target: 20,000+ particles at 60 FPS
```

## Project Structure

```
FluidGame/
├── FluidGame.Core/              ← Start here: pure domain logic
│   ├── Domain/Particles/       ← Particle, Vector2, Color
│   ├── Domain/Physics/         ← SimplePhysicsEngine
│   └── Application/Interfaces/ ← IPhysicsEngine, IParticleSystem
│
├── FluidGame.Infrastructure/    ← Stride adapters
│   └── Stride/
│       ├── Systems/            ← Game loop integration
│       └── Rendering/          ← Drawing particles
│
├── FluidGame.Game/              ← Entry point & DI setup
│   ├── Program.cs              ← Main()
│   ├── FluidGameApp.cs         ← Stride Game class
│   └── GameBootstrapper.cs     ← Wire up dependencies
│
└── FluidGame.Tests/             ← Unit tests (no Stride!)
    └── Core.Tests/             ← Test domain logic
```

## Development Tips

### Adding a New Feature

1. **Start in Core**: Write domain logic first
2. **Test it**: Add unit tests (should run without game engine)
3. **Adapt it**: Create Stride adapter in Infrastructure
4. **Wire it up**: Register in GameBootstrapper

### Example: Adding Wind

```csharp
// 1. Core: Define the interface
public interface IWindForce
{
    Vector2 GetWindVelocity(Vector2 position);
}

// 2. Core: Implement
public class SimpleWind : IWindForce
{
    public Vector2 GetWindVelocity(Vector2 pos) => new(50, 0);
}

// 3. Test: Verify behavior
[Fact]
public void Wind_ShouldPushParticlesRight()
{
    var wind = new SimpleWind();
    var force = wind.GetWindVelocity(Vector2.Zero);
    force.X.Should().Be(50);
}

// 4. Integrate: Update physics engine to apply wind
// 5. Bootstrap: Register in GameBootstrapper
```

## Key Files to Read

1. **README.md** - Project overview
2. **ARCHITECTURE.md** - Why we built it this way
3. **SETUP.md** - Detailed setup instructions
4. **IMPLEMENTATION_SUMMARY.md** - What was built

## Validation Commands

```bash
# Architecture validation
grep -r "using Stride" FluidGame.Core/
# Should return: nothing ✅

# Test validation
dotnet test
# Should pass in < 1 second ✅

# Build validation
dotnet build --configuration Release
# Should succeed with 0 errors ✅
```

## Performance Profiling

```bash
# Run with diagnostics
dotnet run --project FluidGame.Game

# Watch the FPS counter in-game
# Target: 60 FPS with 10,000 particles

# For detailed profiling:
# - Use Visual Studio Profiler
# - Use dotnet-trace
# - Monitor Task Manager / Activity Monitor
```

## Help & Resources

- **Stride Docs**: https://doc.stride3d.net/
- **Clean Architecture**: See ARCHITECTURE.md
- **GDC Talk**: PixelJunk Shooter fluid simulation (referenced in README)

---

Happy coding! 🚀

If you hit any issues, check SETUP.md for detailed troubleshooting.
