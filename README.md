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

- ✅ **10,000 particle simulation** running at 60 FPS
- ✅ **Physics engine** with gravity and particle dynamics
- ✅ **Clean architecture** allowing rapid iteration
- 🚧 **Collision detection** (in progress)
- 🚧 **Terrain generation** (planned)
- 🚧 **Player controls** (planned)

## Technology

Built with:
- **Stride Engine** (C# game engine)
- **Particle-based physics** inspired by PixelJunk Shooter's GDC presentation
- **Clean architecture** - testable, maintainable, framework-independent core

## Building & Running

### Prerequisites
- .NET 8.0 SDK
- Stride Engine 4.2+

### Quick Start

```bash
# Clone and build
git clone <repository-url>
cd CascadeGame
dotnet restore
dotnet build

# Run
dotnet run --project Cascade.Game
```

**Controls:**
- ESC - Exit

## Development Roadmap

### Phase 1: Foundation (Current)
- [x] Particle system architecture
- [x] Basic physics simulation
- [x] Rendering pipeline
- [ ] Spatial optimization (grid-based)
- [ ] Verlet integration

### Phase 2: Fluids
- [ ] Multiple fluid types (water, lava, oil)
- [ ] Fluid-fluid interactions
- [ ] Temperature simulation
- [ ] State changes (water ↔ ice, lava ↔ rock)

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

- **[QUICKSTART.md](QUICKSTART.md)** - Get running in 5 minutes
- **[SETUP.md](SETUP.md)** - Detailed setup & troubleshooting
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Design principles & patterns
- **[IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)** - Technical details

## Contributing

This is currently a solo development project and learning experiment. Contributions, suggestions, and feedback are welcome!

## License

TBD

---

**Why "Cascade"?**

The name reflects both the cascading nature of fluids flowing through underground caverns and the cascading interactions between different game systems - where simple rules create complex, emergent behaviors.
