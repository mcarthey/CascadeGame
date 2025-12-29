# Cascade

A 2D physics-based action game featuring dynamic fluid simulation and underground exploration.

## About

**Cascade** is an experimental game that combines the visceral fluid physics of PixelJunk Shooter with exploration-focused gameplay. Navigate through underground caverns filled with different types of liquids, each with unique properties and behaviors.

The game uses a particle-based fluid simulation system where thousands of individual particles interact physically, creating emergent and unpredictable gameplay moments.

## Core Concept

You explore procedurally-generated underground environments where fluids cascade, pool, and interact:

- **Water** - Flows naturally, can extinguish fire and cool lava
- **Lava** - Burns obstacles, solidifies when cooled
- **Oil** - Highly flammable, lighter than water
- **Magnetic Ferrofluid** - Responds to electromagnetic fields

Each fluid type has distinct physical properties (density, viscosity, temperature) that create unique interaction opportunities.

## Current Status

Early development. The foundation is in place:

- ✅ **SPH (Smoothed Particle Hydrodynamics)** physics simulation
- ✅ **8 material types** with distinct physical properties (Snow, Water, Lava, Oil, Mud, Sand, Steam, Ice)
- ✅ **Material spray system** for interactive testing ("flamethrower" style emitter)
- ✅ **Spatial grid optimization** for efficient neighbor queries
- ✅ **Clean architecture** allowing rapid iteration
- ✅ **Comprehensive test coverage** (unit tests for all systems)
- 🚧 **Terrain collision** (in progress)
- 🚧 **Terrain generation** (planned)
- 🚧 **Player controls** (planned)

## Technology

Built with:
- **Stride Engine** (C# game engine)
- **Particle-based physics** inspired by PixelJunk Shooter's GDC presentation
- **Clean architecture** - testable, maintainable, framework-independent core

## Building & Running

### Prerequisites
- .NET 10.0 SDK
- Stride Engine 4.3+ (installed automatically via NuGet)

### Quick Start

```bash
# Clone and build
git clone <repository-url>
cd CascadeGame
dotnet restore
dotnet build

# Run tests (should complete in < 1 second)
dotnet test

# Run the game
dotnet run --project Cascade.Game
```

**Controls:**
- **ESC** - Exit game
- **1-8** - Select material type (1=Snow, 2=Water, 3=Lava, 4=Oil, 5=Mud, 6=Sand, 7=Steam, 8=Ice)
- **TAB** - Cycle through materials
- **Left Mouse** - Spray burst (10 particles)
- **Right Mouse (Hold)** - Continuous spray stream

## Development Roadmap

### Phase 1: Foundation (✅ Complete)
- [x] Particle system architecture
- [x] SPH physics simulation (density, pressure, viscosity, cohesion)
- [x] Rendering pipeline
- [x] Spatial optimization (grid-based hashing)
- [x] Material type system with 8 distinct materials
- [ ] Verlet integration upgrade

### Phase 2: Fluids (In Progress)
- [x] Multiple fluid types (snow, water, lava, oil, mud, sand, steam, ice)
- [x] Material-specific physics properties
- [x] Interactive material emitter system
- [ ] Fluid-fluid interactions (mixing, separation)
- [ ] Temperature simulation & transfer
- [ ] State changes (water ↔ ice ↔ steam, lava → rock)

### Phase 3: Gameplay
- [ ] Player character & controls
- [ ] Terrain collision
- [ ] Basic level design
- [ ] Tools (drill, suction, etc.)

### Phase 4: Polish
- [ ] Procedural terrain generation
- [ ] Particle effects & polish
- [ ] Audio system
- [ ] Save/load system

## Architecture

The game follows clean architecture principles:

```
Cascade/
├── Cascade.Core/              # Pure C# - framework-independent
│   ├── Domain/                # Particles, physics, game logic
│   └── Application/           # Interfaces & use cases
│
├── Cascade.Infrastructure/    # Stride-specific implementations
│   └── Stride/
│       ├── Systems/           # Game loop integration
│       └── Rendering/         # Graphics pipeline
│
├── Cascade.Game/              # Composition root
│   └── Bootstrapper/          # Dependency injection
│
└── Cascade.Tests/             # Unit tests (no engine required)
```

**Key principle**: Core game logic has zero dependencies on Stride, making it:
- ✅ Unit testable without graphics
- ✅ Portable to other engines
- ✅ Fast to iterate on

## Technical Details

### Particle System
- **Particle count**: 10,000 (current) → 32,768 (target)
- **Integration**: Euler (upgrading to Verlet)
- **Spatial structure**: Grid-based hashing (planned)
- **Collision**: Broad-phase optimization with distance fields

### Performance Targets
- **60 FPS** with 32,768 particles
- **< 16ms** frame time
- **< 200MB** memory footprint

## Inspiration

**PixelJunk Shooter** (Q-Games, 2009)
- Particle-based fluid simulation
- Emergent gameplay from physical interactions
- Multiple fluid types with distinct properties

GDC Presentation: ["Go With The Flow: Fluid Simulation" by Jaymin Kessler](https://www.gdcvault.com/play/1012447/Go-With-the-Flow-Fluid)

## Documentation

- **[DEVELOPER.md](DEVELOPER.md)** - Developer guide & troubleshooting
- **[PHYSICS.md](PHYSICS.md)** - SPH physics implementation details

## Contributing

This is currently a solo development project and learning experiment. Contributions, suggestions, and feedback are welcome!

## License

TBD

---

**Why "Cascade"?**

The name reflects both the cascading nature of fluids flowing through underground caverns and the cascading interactions between different game systems - where simple rules create complex, emergent behaviors.
