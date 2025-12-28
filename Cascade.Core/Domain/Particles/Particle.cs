namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Represents a single particle in the fluid simulation.
/// This is a pure domain object with no dependencies on any rendering framework.
/// </summary>
public class Particle
{
    /// <summary>
    /// Position in 2D space (X, Y coordinates)
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Velocity vector (pixels per second)
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Color represented as RGBA values (0-255 for each component)
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Particle type/material identifier (for future fluid type differentiation)
    /// </summary>
    public ParticleType Type { get; set; }

    /// <summary>
    /// Mass of the particle (affects physics calculations)
    /// </summary>
    public float Mass { get; set; }

    public Particle(Vector2 position, Vector2 velocity, Color color, ParticleType type = ParticleType.Water, float mass = 1.0f)
    {
        Position = position;
        Velocity = velocity;
        Color = color;
        Type = type;
        Mass = mass;
    }

    /// <summary>
    /// Creates a particle with default values
    /// </summary>
    public Particle() : this(Vector2.Zero, Vector2.Zero, new Color(255, 255, 255, 255))
    {
    }
}
