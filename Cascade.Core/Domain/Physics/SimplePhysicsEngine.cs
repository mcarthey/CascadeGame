using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;

namespace Cascade.Core.Domain.Physics;

/// <summary>
/// Simple physics engine implementing basic Newtonian mechanics.
/// This is a starting point - will evolve to Verlet integration and SPH (Smoothed Particle Hydrodynamics).
/// </summary>
public class SimplePhysicsEngine : IPhysicsEngine
{
    public Vector2 Gravity { get; set; }
    public Bounds SimulationBounds { get; set; }

    public SimplePhysicsEngine(Vector2 gravity, Bounds simulationBounds)
    {
        Gravity = gravity;
        SimulationBounds = simulationBounds;
    }

    /// <summary>
    /// Updates particle physics using simple Euler integration.
    /// Future: Upgrade to Verlet integration for better stability.
    /// </summary>
    public void Update(IList<Particle> particles, float deltaTime)
    {
        foreach (var particle in particles)
        {
            UpdateParticle(particle, deltaTime);
        }
    }

    private void UpdateParticle(Particle particle, float deltaTime)
    {
        // Apply gravity force: F = ma, a = F/m
        // For now, we ignore mass (assume m=1) for simplicity
        Vector2 acceleration = Gravity;

        // Update velocity: v = v0 + a*dt
        particle.Velocity += acceleration * deltaTime;

        // Update position: p = p0 + v*dt
        particle.Position += particle.Velocity * deltaTime;

        // Apply boundary constraints (wrap around for now)
        ApplyBoundaryConstraints(particle);
    }

    private void ApplyBoundaryConstraints(Particle particle)
    {
        // Wrap around horizontally
        if (particle.Position.X < SimulationBounds.MinX)
            particle.Position = new Vector2(SimulationBounds.MaxX, particle.Position.Y);
        else if (particle.Position.X > SimulationBounds.MaxX)
            particle.Position = new Vector2(SimulationBounds.MinX, particle.Position.Y);

        // Wrap around vertically
        if (particle.Position.Y < SimulationBounds.MinY)
            particle.Position = new Vector2(particle.Position.X, SimulationBounds.MaxY);
        else if (particle.Position.Y > SimulationBounds.MaxY)
            particle.Position = new Vector2(particle.Position.X, SimulationBounds.MinY);
    }
}
