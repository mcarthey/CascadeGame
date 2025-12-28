using Cascade.Core.Domain.Particles;

namespace Cascade.Core.Application.Interfaces;

/// <summary>
/// Interface for managing the collection of particles.
/// Provides abstraction over particle storage and access.
/// </summary>
public interface IParticleSystem
{
    /// <summary>
    /// All particles in the system
    /// </summary>
    IReadOnlyList<Particle> Particles { get; }

    /// <summary>
    /// Number of particles in the system
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds a particle to the system
    /// </summary>
    void AddParticle(Particle particle);

    /// <summary>
    /// Clears all particles
    /// </summary>
    void Clear();

    /// <summary>
    /// Initializes the system with a specific number of particles
    /// </summary>
    void Initialize(int particleCount, Bounds bounds);
}
