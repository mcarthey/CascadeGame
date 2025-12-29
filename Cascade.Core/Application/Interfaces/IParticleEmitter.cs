using System.Numerics;
using Cascade.Core.Domain.Particles;

namespace Cascade.Core.Application.Interfaces;

/// <summary>
/// Interface for emitting particles into the simulation.
/// Used for creating "spray gun" mechanics and dynamic particle spawning.
/// </summary>
public interface IParticleEmitter
{
    /// <summary>
    /// Emits a single particle at the specified position with the given material.
    /// </summary>
    void EmitParticle(Vector2 position, MaterialType material);

    /// <summary>
    /// Emits multiple particles in a spray pattern at the specified position.
    /// </summary>
    /// <param name="position">The center point of the spray</param>
    /// <param name="material">The material type to emit</param>
    /// <param name="count">Number of particles to emit</param>
    /// <param name="spreadAngle">The cone angle of the spray in radians (default: 30 degrees)</param>
    /// <param name="velocity">The initial velocity magnitude of emitted particles</param>
    void EmitSpray(Vector2 position, MaterialType material, int count = 5, float spreadAngle = 0.52f, float velocity = 100f);
}
