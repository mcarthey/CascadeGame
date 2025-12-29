namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Snow: Sticky, clumpy, relatively light, low temperature
/// </summary>
public class SnowMaterial : MaterialType
{
    public override string Name => "Snow";
    public override Color BaseColor => Color.FromFloat(0.95f, 0.95f, 1.0f); // Pale blue-white

    public override float Mass => 0.8f;           // Lighter than water
    public override float Stiffness => 0.6f;      // Quite compressible
    public override float Viscosity => 2.5f;      // Very sticky/viscous
    public override float Restitution => 0.05f;   // Almost no bounce
    public override float Friction => 0.75f;      // High friction
    public override float Temperature => -5.0f;   // Below freezing
    public override float CohesionStrength => 15f; // Strong clumping
    public override float RestDensity => 6.0f;    // Dense packing
}
