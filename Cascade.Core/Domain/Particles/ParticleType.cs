namespace Cascade.Core.Domain.Particles;

/// <summary>
/// Defines the different types of particles/fluids in the simulation.
/// Based on PixelJunk Shooter's fluid types.
/// NOTE: This enum is deprecated. Use MaterialType classes instead.
/// </summary>
[Obsolete("Use MaterialType classes (WaterMaterial, LavaMaterial, etc.) instead of ParticleType enum")]
public enum ParticleType
{
    /// <summary>
    /// Standard water particles (blue)
    /// </summary>
    Water,

    /// <summary>
    /// Oil/lava particles (orange/red) - future implementation
    /// </summary>
    Lava,

    /// <summary>
    /// Viscous liquid (green) - future implementation
    /// </summary>
    Goo,

    /// <summary>
    /// Magnetic ferrofluid (black) - future implementation
    /// </summary>
    Ferrofluid,

    /// <summary>
    /// Custom material using the new MaterialType system
    /// </summary>
    Custom
}
