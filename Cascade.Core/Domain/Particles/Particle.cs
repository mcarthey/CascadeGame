using Vector2 = System.Numerics.Vector2;
namespace Cascade.Core.Domain.Particles;

public class Particle
{
    // Now standard .NET Vector2
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    // Density is used by the SPH physics engine
    public float Density { get; set; }

    public Color Color { get; set; }
    public ParticleType Type { get; set; }
    public float Mass { get; set; }

    public Particle(Vector2 position, Vector2 velocity, Color color, ParticleType type = ParticleType.Water, float mass = 1.0f)
    {
        Position = position;
        Velocity = velocity;
        Color = color;
        Type = type;
        Mass = mass;
        Density = 0f; // Initialize
    }

    public Particle() : this(Vector2.Zero, Vector2.Zero, new Color(255, 255, 255, 255))
    {
    }
}
