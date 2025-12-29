using System.Numerics;

namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Defines the physical and visual properties of a particle material type.
/// This is the base class for all material definitions (Snow, Lava, Dirt, etc.)
/// </summary>
public abstract class MaterialType
{
    /// <summary>
    /// Display name of the material
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Base color of particles of this type
    /// </summary>
    public abstract Color BaseColor { get; }

    /// <summary>
    /// Mass multiplier (default 1.0). Heavier materials sink faster.
    /// </summary>
    public virtual float Mass => 1.0f;

    /// <summary>
    /// How much this material resists compression (affects SPH pressure).
    /// Higher = more rigid, Lower = more compressible
    /// </summary>
    public virtual float Stiffness => 1.0f;

    /// <summary>
    /// How sticky/viscous this material is.
    /// Higher = flows slower (honey), Lower = flows faster (water)
    /// </summary>
    public virtual float Viscosity => 1.0f;

    /// <summary>
    /// How bouncy collisions are (0 = sticky, 1 = perfectly elastic)
    /// </summary>
    public virtual float Restitution => 0.1f;

    /// <summary>
    /// Surface friction when sliding (0 = ice, 1 = very rough)
    /// </summary>
    public virtual float Friction => 0.5f;

    /// <summary>
    /// Temperature in arbitrary units. Affects interactions (melting, freezing, etc.)
    /// </summary>
    public virtual float Temperature => 20.0f; // Room temperature

    /// <summary>
    /// How much particles of this type attract each other (cohesion).
    /// Higher = clumps more, Lower = separates more
    /// </summary>
    public virtual float CohesionStrength => 1.0f;

    /// <summary>
    /// Desired spacing between particles of this type.
    /// Used to calculate target density in SPH.
    /// </summary>
    public virtual float RestDensity => 5.0f;

    /// <summary>
    /// Can this material flow like a fluid?
    /// </summary>
    public virtual bool IsFluid => true;

    /// <summary>
    /// Can this material be affected by gravity?
    /// </summary>
    public virtual bool AffectedByGravity => true;

    /// <summary>
    /// Generate a color variant for a specific particle (adds randomness/variation)
    /// </summary>
    public virtual Color GetParticleColor()
    {
        // Add slight color variation
        var random = new Random();
        int variation = 20;

        return new Color(
            (byte)Math.Clamp(BaseColor.R + random.Next(-variation, variation), 0, 255),
            (byte)Math.Clamp(BaseColor.G + random.Next(-variation, variation), 0, 255),
            (byte)Math.Clamp(BaseColor.B + random.Next(-variation, variation), 0, 255),
            BaseColor.A
        );
    }
}
