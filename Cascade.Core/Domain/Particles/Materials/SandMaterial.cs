namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Sand: Tiny grains that pour and scatter, minimal clumping
/// </summary>
public class SandMaterial : MaterialType
{
    public override string Name => "Sand";
    public override Color BaseColor => Color.FromFloat(0.9f, 0.8f, 0.5f); // Tan/beige

    // Visual properties - visible grains
    public override float ParticleSize => 7.0f;       // Visible grains
    public override float SizeVariation => 0.25f;     // Uniform size

    // Physics - pours freely, no stickiness
    public override float Mass => 1.8f;           // Heavy (sinks quickly)
    public override float Stiffness => 0.2f;      // Very compressible
    public override float Viscosity => 0.1f;      // Pours very freely
    public override float Restitution => 0.15f;   // Some bounce
    public override float Friction => 0.85f;      // High friction (settles into piles)
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 1f; // Almost no cohesion (doesn't clump)
    public override float RestDensity => 3.0f;    // Very loose packing
    public override bool IsFluid => false;        // Granular
}
