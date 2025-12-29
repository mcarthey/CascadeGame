using Vector2 = System.Numerics.Vector2;

namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Represents a single particle with position, velocity, and material properties.
/// The MaterialType determines how this particle behaves (mass, viscosity, temperature, etc.)
/// </summary>
public class Particle
{
    /// <summary>
    /// Position in 2D space
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Velocity in pixels per second
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Current density calculated by SPH (used for pressure calculations)
    /// </summary>
    public float Density { get; set; }

    /// <summary>
    /// Visual color of this particle (may vary from material base color)
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Render size in pixels (varies per particle for visual variety)
    /// </summary>
    public float RenderSize { get; set; }

    /// <summary>
    /// Material type that defines physical and visual properties.
    /// This determines how the particle behaves in the simulation.
    /// </summary>
    public MaterialType Material { get; set; }

    /// <summary>
    /// Legacy type enum (kept for compatibility, prefer Material property)
    /// </summary>
    [Obsolete("Use Material property instead")]
    public ParticleType Type { get; set; }

    /// <summary>
    /// Cached mass from material (for performance - avoids repeated lookups)
    /// </summary>
    public float Mass => Material?.Mass ?? 1.0f;

    /// <summary>
    /// Create a particle with a specific material type
    /// </summary>
    public Particle(Vector2 position, Vector2 velocity, MaterialType material)
    {
        Position = position;
        Velocity = velocity;
        Material = material ?? throw new ArgumentNullException(nameof(material));
        Color = material.GetParticleColor();
        RenderSize = material.GetParticleSize();
        Density = 0f;
#pragma warning disable CS0618 // Type or member is obsolete
        Type = ParticleType.Custom;
#pragma warning restore CS0618
    }

    /// <summary>
    /// Legacy constructor (for backward compatibility)
    /// </summary>
    [Obsolete("Use constructor with MaterialType instead")]
    public Particle(Vector2 position, Vector2 velocity, Color color, ParticleType type = ParticleType.Water, float mass = 1.0f)
    {
        Position = position;
        Velocity = velocity;
        Color = color;
#pragma warning disable CS0618 // Type or member is obsolete
        Type = type;
#pragma warning restore CS0618
        Material = GetDefaultMaterialForType(type);
        RenderSize = Material.GetParticleSize();
        Density = 0f;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Particle() : this(Vector2.Zero, Vector2.Zero, new Materials.WaterMaterial())
    {
    }

    /// <summary>
    /// Maps legacy ParticleType enum to MaterialType instances
    /// </summary>
    private static MaterialType GetDefaultMaterialForType(ParticleType type)
    {
        return type switch
        {
            ParticleType.Water => new Materials.WaterMaterial(),
            ParticleType.Lava => new Materials.LavaMaterial(),
            ParticleType.Goo => new Materials.MudMaterial(),
            ParticleType.Ferrofluid => new Materials.OilMaterial(), // Temporary mapping
            _ => new Materials.WaterMaterial()
        };
    }
}
