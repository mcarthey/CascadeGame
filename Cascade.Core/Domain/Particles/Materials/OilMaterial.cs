namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Oil: Slippery, slides everywhere, forms puddles
/// </summary>
public class OilMaterial : MaterialType
{
    public override string Name => "Oil";
    public override Color BaseColor => Color.FromFloat(0.15f, 0.1f, 0.05f); // Very dark brown/black

    // Visual properties - satisfying droplets
    public override float ParticleSize => 11.0f;      // Medium-chunky droplets
    public override float SizeVariation => 0.25f;     // Moderate variance

    // Physics - light, slippery, spreads out
    public override float Mass => 0.6f;           // Light (floats on water)
    public override float Stiffness => 1.0f;      // Flows moderately
    public override float Viscosity => 1.5f;      // Somewhat viscous (goopy)
    public override float Restitution => 0.05f;   // No bounce
    public override float Friction => 0.05f;      // EXTREMELY slippery
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 6f; // Forms puddles
    public override float RestDensity => 6.0f;    // Medium density
}
