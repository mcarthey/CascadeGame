namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Water: Fluid, flows fast and freely - small splashy droplets
/// </summary>
public class WaterMaterial : MaterialType
{
    public override string Name => "Water";
    public override Color BaseColor => Color.FromFloat(0.2f, 0.5f, 1.0f); // Bright blue

    // Visual properties - small droplets
    public override float ParticleSize => 6.0f;       // Small droplets
    public override float SizeVariation => 0.3f;      // Moderate variance

    // Physics - flows freely, bouncy
    public override float Mass => 1.0f;           // Standard reference mass
    public override float Stiffness => 2.0f;      // Resists compression (splashy)
    public override float Viscosity => 0.3f;      // VERY low viscosity (flows freely)
    public override float Restitution => 0.2f;    // Bouncy splashes
    public override float Friction => 0.2f;       // Very low friction
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 3f; // Low cohesion (spreads out)
    public override float RestDensity => 8.0f;    // Tight packing for water
}
