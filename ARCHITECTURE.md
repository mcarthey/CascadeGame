# Architecture Documentation

## Overview

This project follows **Clean Architecture** principles (also known as Hexagonal Architecture or Ports & Adapters). The goal is to keep business logic independent of frameworks, making the system testable, maintainable, and flexible.

## Dependency Flow

```
┌─────────────────────────────────────────────────────────┐
│                    FluidGame.Game                        │
│              (Composition Root / Entry Point)            │
│                                                          │
│  - Program.cs (Main entry point)                        │
│  - FluidGameApp.cs (Stride Game class)                  │
│  - GameBootstrapper.cs (DI container setup)             │
└────────────────┬────────────────────────────────────────┘
                 │ depends on ↓
                 │
     ┌───────────┴──────────┬─────────────────────────────┐
     │                      │                             │
     ↓                      ↓                             ↓
┌─────────────┐    ┌─────────────────────┐    ┌──────────────────┐
│  Core       │    │  Infrastructure     │    │   Tests          │
│             │←───│                     │    │                  │
│  Pure C#    │    │  Stride-specific    │    │  Pure C# tests   │
│  No deps    │    │  implementations    │    │  No Stride deps  │
└─────────────┘    └─────────────────────┘    └──────────────────┘
```

## Layer Descriptions

### 1. FluidGame.Core (Domain Layer)

**Purpose**: Contains pure business logic with zero external dependencies.

**Contents**:
- `Domain/Particles/` - Particle entity, Vector2, Color, ParticleType
- `Domain/Physics/` - SimplePhysicsEngine implementation
- `Application/Interfaces/` - Contracts (IPhysicsEngine, IParticleSystem)

**Rules**:
- ✅ Can reference: .NET standard library only
- ❌ Cannot reference: Stride, Unity, MonoGame, or any framework
- ✅ Should be: 100% unit testable without game engine
- ✅ Purpose: Business logic that outlives any framework

**Example**:
```csharp
// ✅ Good - Pure domain logic
public class Particle
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
}

// ❌ Bad - Framework dependency
using Stride.Core.Mathematics;
public class Particle
{
    public Vector3 Position { get; set; } // Stride type!
}
```

### 2. FluidGame.Infrastructure (Adapter Layer)

**Purpose**: Adapts our domain to work with Stride Engine.

**Contents**:
- `Stride/Systems/` - ParticlePhysicsSystem, PerformanceMonitorSystem
- `Stride/Rendering/` - SimpleParticleRenderer, TypeConverter
- `Stride/Components/` - (Future: Stride EntityComponents)

**Rules**:
- ✅ Can reference: FluidGame.Core, Stride packages
- ✅ Should: Implement interfaces from Core
- ✅ Should: Convert between domain types and Stride types
- ❌ Should not: Contain business logic

**Example**:
```csharp
// ✅ Good - Adapter pattern
public static class TypeConverter
{
    public static Stride.Core.Mathematics.Vector2 ToStrideVector2(Domain.Vector2 v)
    {
        return new Stride.Core.Mathematics.Vector2(v.X, v.Y);
    }
}

// ✅ Good - Stride system wrapping domain logic
public class ParticlePhysicsSystem : GameSystem
{
    private readonly IPhysicsEngine _physicsEngine; // Domain interface!

    public override void Update(GameTime gameTime)
    {
        _physicsEngine.Update(particles, deltaTime);
    }
}
```

### 3. FluidGame.Game (Composition Root)

**Purpose**: Wires everything together and starts the application.

**Contents**:
- `Program.cs` - Application entry point
- `FluidGameApp.cs` - Stride Game subclass
- `GameBootstrapper.cs` - Dependency injection setup

**Rules**:
- ✅ Can reference: Everything (Core, Infrastructure, Stride)
- ✅ Should: Create and wire up dependencies
- ❌ Should not: Contain business logic
- ❌ Should not: Contain rendering code

**Example**:
```csharp
// ✅ Good - Dependency injection
var particleSystem = new ParticleSystem();
var physicsEngine = new SimplePhysicsEngine(gravity, bounds);
services.AddService<IPhysicsEngine>(physicsEngine);

var physicsSystem = new ParticlePhysicsSystem(services, physicsEngine, particleSystem);
gameSystems.Add(physicsSystem);
```

### 4. FluidGame.Tests (Test Layer)

**Purpose**: Validates domain logic without requiring game engine.

**Contents**:
- `Core.Tests/` - Unit tests for Core domain

**Rules**:
- ✅ Can reference: FluidGame.Core only (not Infrastructure!)
- ✅ Should: Run without Stride runtime
- ✅ Should: Be fast (no I/O, no graphics)
- ✅ Framework: xUnit + FluentAssertions

**Example**:
```csharp
// ✅ Good - Pure domain test
[Fact]
public void Gravity_ShouldAccelerateParticle()
{
    var engine = new SimplePhysicsEngine(new Vector2(0, 100), bounds);
    var particle = new Particle(Vector2.Zero, Vector2.Zero, Color.Blue);

    engine.Update(new[] { particle }, 0.1f);

    particle.Velocity.Y.Should().Be(10f);
}
```

## Design Patterns Used

### 1. Dependency Inversion Principle (DIP)

High-level modules don't depend on low-level modules. Both depend on abstractions.

```
┌─────────────────────┐
│  GameBootstrapper   │  ← High-level
│  (Game layer)       │
└──────────┬──────────┘
           │ depends on
           ↓
┌─────────────────────┐
│  IPhysicsEngine     │  ← Abstraction (Interface)
│  (Core/Application) │
└──────────┬──────────┘
           ↑ implements
           │
┌──────────────────────┐
│ SimplePhysicsEngine  │  ← Low-level
│ (Core/Domain)        │
└──────────────────────┘
```

### 2. Adapter Pattern

`TypeConverter` adapts domain types to Stride types:

```csharp
Domain.Vector2 → TypeConverter.ToStrideVector2() → Stride.Mathematics.Vector2
```

### 3. Service Locator / Dependency Injection

Using Stride's `IServiceRegistry`:

```csharp
services.AddService<IPhysicsEngine>(physicsEngine);
var engine = services.GetService<IPhysicsEngine>();
```

### 4. Strategy Pattern

`IPhysicsEngine` allows swapping implementations:

```csharp
IPhysicsEngine engine;

// POC: Simple Euler integration
engine = new SimplePhysicsEngine(...);

// Week 3: Upgrade to Verlet
engine = new VerletPhysicsEngine(...);

// Later: Add SPH
engine = new SPHPhysicsEngine(...);
```

## Data Flow

### Game Loop Flow

```
┌──────────────────────────────────────────────────────────┐
│  1. Game.Update() called by Stride (60 FPS)              │
└────────────────────────┬─────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────┐
│  2. ParticlePhysicsSystem.Update()                       │
│     - Gets deltaTime from GameTime                       │
│     - Calls IPhysicsEngine.Update(particles, deltaTime)  │
└────────────────────────┬─────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────┐
│  3. SimplePhysicsEngine.Update() [PURE DOMAIN LOGIC]     │
│     - Apply gravity: velocity += gravity * dt            │
│     - Update position: position += velocity * dt         │
│     - Apply boundary constraints (wrap-around)           │
└────────────────────────┬─────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────┐
│  4. Game.Draw() called by Stride                         │
└────────────────────────┬─────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────┐
│  5. SimpleParticleRenderer.Draw()                        │
│     - Converts domain particles to Stride vertices       │
│     - Uses TypeConverter for type translation            │
│     - Renders with SpriteBatch                           │
└────────────────────────┬─────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────┐
│  6. PerformanceMonitorSystem.Draw()                      │
│     - Displays FPS counter                               │
│     - Shows particle count                               │
└──────────────────────────────────────────────────────────┘
```

## Key Interfaces

### IPhysicsEngine

```csharp
public interface IPhysicsEngine
{
    void Update(IList<Particle> particles, float deltaTime);
    Vector2 Gravity { get; set; }
    Bounds SimulationBounds { get; set; }
}
```

**Purpose**: Encapsulates physics simulation logic.

**Implementations**:
- `SimplePhysicsEngine` - Basic Euler integration (Week 1-2)
- *Future*: `VerletPhysicsEngine` - Stable integration (Week 3-4)
- *Future*: `SPHPhysicsEngine` - Fluid dynamics (Week 5+)

### IParticleSystem

```csharp
public interface IParticleSystem
{
    IReadOnlyList<Particle> Particles { get; }
    int Count { get; }
    void Initialize(int particleCount, Bounds bounds);
    void AddParticle(Particle particle);
    void Clear();
}
```

**Purpose**: Manages particle collection and lifecycle.

**Implementation**:
- `ParticleSystem` - Simple list-based storage
- *Future*: `SpatialHashParticleSystem` - Grid-based for collision detection

## Benefits of This Architecture

### ✅ Testability

```csharp
// Test runs in milliseconds, no graphics needed
[Fact]
public void Gravity_Works()
{
    var engine = new SimplePhysicsEngine(...);
    var particle = new Particle(...);
    engine.Update(new[] { particle }, 0.1f);
    particle.Velocity.Y.Should().Be(10f);
}
```

### ✅ Framework Independence

Want to switch from Stride to MonoGame? Just rewrite the Infrastructure layer. Core stays the same!

```
Core (unchanged) + Infrastructure.MonoGame (new) = MonoGame version
Core (unchanged) + Infrastructure.Godot (new) = Godot version
```

### ✅ Evolution-Friendly

Upgrade physics without touching rendering:

```csharp
// Week 1-2: Simple physics
services.AddService<IPhysicsEngine>(new SimplePhysicsEngine(...));

// Week 3: Just swap the implementation!
services.AddService<IPhysicsEngine>(new VerletPhysicsEngine(...));
```

### ✅ Clear Responsibilities

- **Core**: "What should happen?" (business rules)
- **Infrastructure**: "How to do it with Stride?" (technical details)
- **Game**: "Wire it all together" (composition)

## Anti-Patterns to Avoid

### ❌ DON'T: Put Stride types in Core

```csharp
// BAD!
using Stride.Core.Mathematics;

public class Particle
{
    public Vector3 Position { get; set; } // Stride dependency!
}
```

### ❌ DON'T: Put business logic in Infrastructure

```csharp
// BAD!
public class ParticlePhysicsSystem : GameSystem
{
    public override void Update(GameTime gameTime)
    {
        // Business logic here is wrong!
        particle.Velocity += gravity * deltaTime;
        particle.Position += particle.Velocity * deltaTime;
    }
}

// GOOD!
public class ParticlePhysicsSystem : GameSystem
{
    private readonly IPhysicsEngine _engine; // Delegate to Core!

    public override void Update(GameTime gameTime)
    {
        _engine.Update(particles, deltaTime);
    }
}
```

### ❌ DON'T: Reference Infrastructure from Tests

```csharp
// BAD!
using FluidGame.Infrastructure.Stride.Systems;

[Fact]
public void TestPhysics()
{
    var system = new ParticlePhysicsSystem(...); // Framework dependency!
}

// GOOD!
using FluidGame.Core.Domain.Physics;

[Fact]
public void TestPhysics()
{
    var engine = new SimplePhysicsEngine(...); // Pure C#!
}
```

## Future Enhancements

This architecture supports future additions cleanly:

1. **Week 3-4**: Verlet integration
   - Add `VerletPhysicsEngine : IPhysicsEngine`
   - Swap in GameBootstrapper

2. **Week 5-6**: Spatial hashing
   - Add `SpatialHashParticleSystem : IParticleSystem`
   - Swap in GameBootstrapper

3. **Week 7-8**: SPH fluid dynamics
   - Add `ISPHSolver` interface
   - Implement `SPHPhysicsEngine : IPhysicsEngine`
   - All existing code continues to work!

## Validation

To verify architecture is correct:

```bash
# Core should have ZERO Stride references
grep -r "using Stride" FluidGame.Core/
# Should return nothing!

# Tests should run without Stride
dotnet test --logger "console;verbosity=detailed"
# All tests should pass in < 1 second
```

## Resources

- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- **Dependency Inversion**: https://en.wikipedia.org/wiki/Dependency_inversion_principle
- **Hexagonal Architecture**: https://alistair.cockburn.us/hexagonal-architecture/
