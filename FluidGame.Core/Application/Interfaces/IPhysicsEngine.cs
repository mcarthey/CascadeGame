using FluidGame.Core.Domain.Particles;

namespace FluidGame.Core.Application.Interfaces;

/// <summary>
/// Interface for physics simulation engine.
/// Follows Dependency Inversion Principle - high-level modules depend on this abstraction.
/// </summary>
public interface IPhysicsEngine
{
    /// <summary>
    /// Updates all particles based on physics simulation
    /// </summary>
    /// <param name="particles">Collection of particles to update</param>
    /// <param name="deltaTime">Time elapsed since last update (in seconds)</param>
    void Update(IList<Particle> particles, float deltaTime);

    /// <summary>
    /// Gravity force applied to particles (pixels per second squared)
    /// </summary>
    Vector2 Gravity { get; set; }

    /// <summary>
    /// Simulation bounds (particles wrap or bounce at these limits)
    /// </summary>
    Bounds SimulationBounds { get; set; }
}

/// <summary>
/// Defines the rectangular bounds of the simulation space
/// </summary>
public struct Bounds
{
    public float MinX { get; set; }
    public float MaxX { get; set; }
    public float MinY { get; set; }
    public float MaxY { get; set; }

    public Bounds(float minX, float maxX, float minY, float maxY)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
    }

    public float Width => MaxX - MinX;
    public float Height => MaxY - MinY;
}
