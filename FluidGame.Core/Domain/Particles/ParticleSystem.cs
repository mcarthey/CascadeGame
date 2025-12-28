using FluidGame.Core.Application.Interfaces;

namespace FluidGame.Core.Domain.Particles;

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

        // Random blue-ish color for water
        var color = new Color(
            (byte)_random.Next(50, 100),
            (byte)_random.Next(100, 200),
            (byte)_random.Next(200, 255),
            255
        );

        return new Particle(position, velocity, color, ParticleType.Water, 1.0f);
    }
}
