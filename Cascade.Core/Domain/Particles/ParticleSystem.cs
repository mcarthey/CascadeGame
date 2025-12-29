using System.Numerics;
using Cascade.Core.Application.Interfaces;

namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Manages the collection of particles in the simulation.
/// Provides efficient storage and access to particle data.
/// </summary>
public class ParticleSystem : IParticleSystem
{
    private readonly List<Particle> _particles;
    private readonly Random _random;

    public IReadOnlyList<Particle> Particles => _particles;
    public int Count => _particles.Count;

    public ParticleSystem()
    {
        _particles = new List<Particle>();
        _random = new Random();
    }

    public void AddParticle(Particle particle)
    {
        _particles.Add(particle);
    }

    public void Clear()
    {
        _particles.Clear();
    }

    /// <summary>
    /// Initializes the system with randomly positioned particles
    /// </summary>
    public void Initialize(int particleCount, Bounds bounds)
    {
        Clear();

        for (int i = 0; i < particleCount; i++)
        {
            var particle = CreateRandomParticle(bounds);
            _particles.Add(particle);
        }
    }

    private Particle CreateRandomParticle(Bounds bounds)
    {
        // Random position within bounds
        var position = new Vector2(
            _random.NextSingle() * bounds.Width + bounds.MinX,
            _random.NextSingle() * bounds.Height + bounds.MinY
        );

        // Small random initial velocity
        var velocity = new Vector2(
            (_random.NextSingle() - 0.5f) * 20f,
            (_random.NextSingle() - 0.5f) * 20f
        );

        // Use SnowMaterial by default for the "cornstarch" clumpy behavior
        var material = new Materials.SnowMaterial();

        return new Particle(position, velocity, material);
    }
}
