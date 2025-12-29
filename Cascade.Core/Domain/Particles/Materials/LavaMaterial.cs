namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Lava: Super dense, glowing globs that sink and spread slowly
/// </summary>
public class LavaMaterial : MaterialType
{
    public override string Name => "Lava";
    public override Color BaseColor => Color.FromFloat(1.0f, 0.35f, 0.05f); // Glowing orange-red

    // Visual properties - big glowing globs
    public override float ParticleSize => 9.0f;       // Large glowing globs
    public override float SizeVariation => 0.35f;     // Varied blob sizes

    // Physics - HEAVY, slow, sticky
    public override float Mass => 3.5f;           // VERY heavy (sinks fast)
    public override float Stiffness => 0.5f;      // Flows but resists
    public override float Viscosity => 8.0f;      // EXTREMELY viscous (barely flows)
    public override float Restitution => 0.0f;    // Zero bounce (splats)
    public override float Friction => 0.95f;      // Extremely high friction
    public override float Temperature => 1200.0f; // Extremely hot
    public override float CohesionStrength => 18f; // Very sticky and clumpy
    public override float RestDensity => 12.0f;   // Super dense
}
