# Developer Guide

This guide covers everything you need to develop, test, and extend Cascade.

## Prerequisites

### Required
- **.NET 10.0 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Windows** - Required for Stride Engine

### Recommended IDE
- **Visual Studio 2022** with Game Development workload
- **JetBrains Rider** - Excellent C# support
- **VS Code** with C# Dev Kit extension

## Project Structure

```
CascadeGame/
├── Cascade.Core/                 # Pure C# domain (no framework dependencies)
│   ├── Domain/
│   │   ├── Particles/            # Particle, Vector2, Color, ParticleType
│   │   └── Physics/              # SimplePhysicsEngine, Bounds
│   └── Application/
│       └── Interfaces/           # IPhysicsEngine, IParticleSystem
│
├── Cascade.Infrastructure/       # Stride Engine adapters
│   └── Stride/
│       ├── Systems/              # ParticlePhysicsSystem, PerformanceMonitorSystem
│       └── Rendering/            # ParticleSceneRenderer, TypeConverter
│
├── Cascade.Game/                 # Composition root & entry point
│   ├── Program.cs                # Main()
│   ├── CascadeApp.cs             # Stride Game class
│   └── GameBootstrapper.cs       # Dependency injection setup
│
└── Cascade.Tests/                # Unit tests (no Stride required)
    └── Core.Tests/               # Tests for domain logic
```

## Technology Stack

| Component | Technology | Version |
|-----------|------------|---------|
| Runtime | .NET | 10.0 |
| Game Engine | Stride | 4.3.0 |
| Toolkit | Stride.CommunityToolkit | 1.0.0-preview |
| Testing | xUnit | 2.6.6 |
| Assertions | FluentAssertions | 6.12.0 |

## Build & Run

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Run unit tests
dotnet test

# Run the game
dotnet run --project Cascade.Game
```

## Development Workflow

### Adding a New Feature

1. **Start in Core** - Write domain logic first (pure C#)
2. **Test it** - Add unit tests in Cascade.Tests
3. **Adapt it** - Create Stride adapter in Infrastructure if needed
4. **Wire it up** - Register in GameBootstrapper

### Example: Adding a Wind Force

```csharp
// 1. Define interface in Core/Application/Interfaces
public interface IWindForce
{
    Vector2 GetWindVelocity(Vector2 position);
}

// 2. Implement in Core/Domain/Physics
public class SimpleWind : IWindForce
{
    public Vector2 Direction { get; set; } = new(50, 0);

    public Vector2 GetWindVelocity(Vector2 position) => Direction;
}

// 3. Test in Cascade.Tests
[Fact]
public void Wind_ShouldReturnConfiguredDirection()
{
    var wind = new SimpleWind { Direction = new Vector2(100, 0) };
    var velocity = wind.GetWindVelocity(Vector2.Zero);
    velocity.X.Should().Be(100);
}

// 4. Integrate in SimplePhysicsEngine.Update()
// 5. Register in GameBootstrapper if needed
```

## Key Classes

### Cascade.Core

| Class | Purpose |
|-------|---------|
| `Particle` | Entity with position, velocity, color, type, mass |
| `Vector2` | Pure math type (framework-independent) |
| `Color` | RGBA color (framework-independent) |
| `SimplePhysicsEngine` | Euler integration physics |
| `ParticleSystem` | Manages particle collection |

### Cascade.Infrastructure

| Class | Purpose |
|-------|---------|
| `ParticlePhysicsSystem` | Bridges Stride GameSystem to IPhysicsEngine |
| `ParticleSceneRenderer` | Custom SceneRenderer for particle drawing |
| `PerformanceMonitorSystem` | FPS and particle count display |
| `TypeConverter` | Converts domain types to Stride types |

### Cascade.Game

| Class | Purpose |
|-------|---------|
| `CascadeApp` | Main Stride Game class |
| `GameBootstrapper` | Dependency injection and system setup |

## Rendering Pipeline

The game uses Stride's graphics compositor with a custom scene renderer:

1. `CascadeApp.BeginRun()` calls `SetupBase2D()` from Community Toolkit
2. `GameBootstrapper` adds `ParticleSceneRenderer` to the compositor
3. `ParticleSceneRenderer.DrawCore()` uses SpriteBatch to render particles

```
SetupBase2D() → Creates GraphicsCompositor + Camera
     ↓
GameBootstrapper → Adds ParticleSceneRenderer to compositor
     ↓
DrawCore() → SpriteBatch renders each particle
```

## Testing

### Run All Tests
```bash
dotnet test
```

### Run with Verbose Output
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Test Coverage
Tests are in `Cascade.Tests/Core.Tests/`:
- `PhysicsEngineTests.cs` - Gravity, boundaries, particle updates
- `ParticleSystemTests.cs` - Initialization, add/clear operations
- `Vector2Tests.cs` - Math operations

**Important**: Tests run without Stride runtime. They test pure domain logic only.

## Troubleshooting

### Build Errors

**"Stride packages not found"**
```bash
# Packages are restored from NuGet automatically
dotnet restore --force
```

**"Target framework not supported"**
Ensure .NET 10.0 SDK is installed:
```bash
dotnet --list-sdks
# Should show 10.0.x
```

### Runtime Errors

**Black screen**
- Ensure `SetupBase2D()` is called before `GameBootstrapper.Bootstrap()`
- Check that `ParticleSceneRenderer` is added to the graphics compositor

**"Graphics device not available"**
- Update GPU drivers
- Ensure DirectX 11+ support

**Low FPS**
- Profile with Visual Studio or `dotnet-trace`
- Check particle count (default: 10,000)
- Consider spatial optimization for collision detection

### Test Failures

**"Stride runtime required"**
This indicates an architecture violation:
- Tests should only reference `Cascade.Core`
- Remove any Stride references from test project
- Move framework-dependent code to Infrastructure

## Architecture Validation

Verify the architecture remains clean:

```bash
# Core should have NO Stride dependencies
grep -r "using Stride" Cascade.Core/
# Should return nothing

# Tests should only reference Core
grep -r "Cascade.Infrastructure" Cascade.Tests/
# Should return nothing
```

## Performance Targets

| Metric | Current | Target |
|--------|---------|--------|
| Particles | 10,000 | 32,768 |
| Frame Rate | 60 FPS | 60 FPS |
| Update Time | <16ms | <16ms |
| Memory | <100MB | <200MB |

## Roadmap

### Phase 1: Foundation (Complete)
- [x] Clean architecture setup
- [x] Particle system with gravity
- [x] Basic rendering pipeline
- [x] Unit tests

### Phase 2: Physics Improvements
- [ ] Verlet integration (stability)
- [ ] Particle-particle collision
- [ ] Spatial hashing optimization

### Phase 3: Fluid Types
- [ ] Water, lava, oil behaviors
- [ ] Temperature simulation
- [ ] State changes (freeze, melt)

### Phase 4: Gameplay
- [ ] Player controls
- [ ] Terrain collision
- [ ] Level design tools

## Resources

- **Stride Documentation**: https://doc.stride3d.net/
- **Stride Community Toolkit**: https://stride3d.github.io/stride-community-toolkit/
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- **PixelJunk Shooter GDC Talk**: https://www.gdcvault.com/play/1012447/Go-With-the-Flow-Fluid
