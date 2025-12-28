# Elemental Underground - Fluid Physics Game

A 2D fluid physics game inspired by PixelJunk Shooter's particle-based fluid simulation, built with Stride Engine and clean SOLID architecture.

## Project Structure

```
FluidGame.sln
│
├── FluidGame.Core/                    # Pure C# domain logic (no Stride dependencies)
│   ├── Domain/
│   │   ├── Particles/                 # Particle, Vector2, Color, ParticleType
│   │   └── Physics/                   # SimplePhysicsEngine
│   └── Application/
│       └── Interfaces/                # IPhysicsEngine, IParticleSystem, Bounds
│
├── FluidGame.Infrastructure/          # Stride-specific implementations
│   └── Stride/
│       ├── Systems/                   # ParticlePhysicsSystem, PerformanceMonitorSystem
│       └── Rendering/                 # SimpleParticleRenderer, TypeConverter
│
├── FluidGame.Game/                    # Stride game project
│   ├── FluidGameApp.cs               # Main game class
│   ├── GameBootstrapper.cs           # Dependency injection setup
│   └── Program.cs                     # Entry point
│
└── FluidGame.Tests/                   # Unit tests
    └── Core.Tests/                    # Tests for core domain (no Stride required!)
```

## Architecture Principles

### 1. **Dependency Inversion**
- Core domain has **zero dependencies** on Stride or any framework
- Infrastructure layer adapts Stride to our domain interfaces
- High-level modules depend on abstractions, not concretions

### 2. **Separation of Concerns**
- **Core**: Pure business logic (physics, particles)
- **Infrastructure**: Framework-specific implementations (rendering, systems)
- **Game**: Composition root (bootstrapping, DI)

### 3. **Testability**
- Core domain is 100% unit testable without game engine
- See `FluidGame.Tests/Core.Tests/` for examples

## Current POC Status (Week 1-2)

- ✅ Basic Stride project setup
- ✅ Render 10,000 particles as colored points
- ✅ Implement basic gravity simulation
- ✅ FPS counter and performance monitoring
- ✅ Clean architecture with SOLID principles
- ✅ Unit tests for physics engine

## Building and Running

### Prerequisites
- .NET 8.0 SDK
- Stride Engine 4.2

### Build
```bash
dotnet restore
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run Game
```bash
dotnet run --project FluidGame.Game
```

## Performance Target

- **Current**: 10,000 particles
- **Target**: 32,768 particles at 60 FPS (PixelJunk Shooter specs)

## Controls

- **ESC**: Exit game

## Technical Implementation

### Physics
- Simple Euler integration (will upgrade to Verlet)
- Gravity: 200 pixels/sec²
- Boundary: Wrap-around (720p: 1280x720)

### Rendering
- SpriteBatch-based particle rendering (POC)
- Future: Vertex buffers for better performance

### Particle Properties
- Position (Vector2)
- Velocity (Vector2)
- Color (RGBA)
- Type (Water, Lava, Goo, Ferrofluid - future)
- Mass (future physics)

## Next Steps (Week 3-4)

1. Upgrade to Verlet integration
2. Add particle-particle collision detection
3. Implement spatial hashing/grid
4. Optimize rendering with vertex buffers
5. Add simple terrain/obstacles

## References

- **PixelJunk Shooter GDC Talk**: "Go With The Flow: Fluid Simulation" by Jaymin Kessler
- **Target Specs**: 32,768 particles, 44×28 grid, distance fields
- **Architecture Discussion**: [Claude Conversation](https://claude.ai/share/879b4f59-6218-4f1e-a9ca-2ca718e9c186)

## Why Stride?

Stride was chosen over Unity specifically because:
1. **Clean architecture support**: No MonoBehaviour coupling
2. **Full C# access**: Real dependency injection, no magic
3. **Open source**: Complete control over engine behavior
4. **Performance**: Direct access to rendering pipeline
