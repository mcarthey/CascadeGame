namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Steam: Wispy clouds that rise upward - lightest material
/// </summary>
public class SteamMaterial : MaterialType
{
    public override string Name => "Steam";
    public override Color BaseColor => Color.FromFloat(0.95f, 0.95f, 1.0f, 0.7f); // Translucent white

    // Visual properties - small wispy puffs
    public override float ParticleSize => 9.0f;       // Large puffs of steam
    public override float SizeVariation => 0.5f;      // Very varied (wispy)

    // Physics - VERY light, rises up, disperses
    public override float Mass => 0.05f;          // Extremely light (rises fast!)
    public override float Stiffness => 0.05f;     // Almost no stiffness (gas)
    public override float Viscosity => 0.05f;     // Flows extremely freely
    public override float Restitution => 0.4f;    // Bouncy (gas)
    public override float Friction => 0.01f;      // No friction
    public override float Temperature => 100.0f;  // Hot
    public override float CohesionStrength => 0.2f; // Almost no cohesion (disperses)
    public override float RestDensity => 0.5f;    // Super loose
    public override bool AffectedByGravity => true; // But should rise (handled specially)
}
