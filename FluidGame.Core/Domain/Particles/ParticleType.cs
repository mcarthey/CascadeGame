namespace FluidGame.Core.Domain.Particles;

/// <summary>
/// Defines the different types of particles/fluids in the simulation.
/// Based on PixelJunk Shooter's fluid types.
/// </summary>
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
    Ferrofluid
}
