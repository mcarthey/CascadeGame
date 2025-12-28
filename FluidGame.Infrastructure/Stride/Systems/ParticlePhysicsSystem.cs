using FluidGame.Core.Application.Interfaces;
using FluidGame.Core.Domain.Particles;
using Stride.Core;
using Stride.Engine;
using Stride.Games;

namespace FluidGame.Infrastructure.Stride.Systems;

/// <summary>
/// Stride GameSystem that updates particle physics each frame.
/// Adapts our core physics engine to work within Stride's game loop.
/// </summary>
public class ParticlePhysicsSystem : GameSystem
{
    private readonly IPhysicsEngine _physicsEngine;
    private readonly IParticleSystem _particleSystem;

    public ParticlePhysicsSystem(IServiceRegistry services, IPhysicsEngine physicsEngine, IParticleSystem particleSystem)
        : base(services)
    {
        _physicsEngine = physicsEngine ?? throw new ArgumentNullException(nameof(physicsEngine));
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));

        // This system should update during the main game update phase
        Enabled = true;
        UpdateOrder = -1000; // Run before rendering
    }

    public override void Update(GameTime gameTime)
    {
        if (!Enabled) return;

        // Convert Stride's GameTime to deltaTime in seconds
        float deltaTime = (float)gameTime.Elapsed.TotalSeconds;

        // Get particles as mutable list for physics update
        var particles = _particleSystem.Particles as IList<Particle>
                       ?? _particleSystem.Particles.ToList();

        // Update physics
        _physicsEngine.Update(particles, deltaTime);
    }
}
