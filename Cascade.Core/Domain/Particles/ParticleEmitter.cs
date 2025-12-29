using System.Numerics;
using Cascade.Core.Application.Interfaces;

namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Emits particles into the simulation with various spray patterns.
/// Implements the "spray gun" mechanic for material emission.
/// </summary>
public class ParticleEmitter : IParticleEmitter
{
    private readonly IParticleSystem _particleSystem;
    private readonly Random _random;

    public ParticleEmitter(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
        _random = new Random();
    }

    /// <summary>
    /// Emits a single particle at the specified position.
    /// </summary>
    public void EmitParticle(Vector2 position, MaterialType material)
    {
        var velocity = new Vector2(
            (_random.NextSingle() - 0.5f) * 50f,
            (_random.NextSingle() - 0.5f) * 50f
        );

        var particle = new Particle(position, velocity, material);
        _particleSystem.AddParticle(particle);
    }

    /// <summary>
    /// Emits multiple particles in a spray pattern.
    /// Creates a cone-shaped spray similar to a flamethrower.
    /// </summary>
    public void EmitSpray(Vector2 position, MaterialType material, int count = 5, float spreadAngle = 0.52f, float velocity = 100f)
    {
        for (int i = 0; i < count; i++)
        {
            // Random angle within the spread cone
            float angle = (float)(_random.NextDouble() * spreadAngle - spreadAngle / 2);

            // Random velocity magnitude with some variation
            float speed = velocity + (_random.NextSingle() - 0.5f) * velocity * 0.3f;

            // Calculate velocity vector from angle
            var particleVelocity = new Vector2(
                MathF.Cos(angle) * speed,
                MathF.Sin(angle) * speed
            );

            // Add slight positional variance for more natural spray
            var spawnPosition = position + new Vector2(
                (_random.NextSingle() - 0.5f) * 10f,
                (_random.NextSingle() - 0.5f) * 10f
            );

            var particle = new Particle(spawnPosition, particleVelocity, material);
            _particleSystem.AddParticle(particle);
        }
    }
}
