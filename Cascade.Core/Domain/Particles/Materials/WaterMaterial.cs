namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Water: Fluid, medium density, flows easily
/// </summary>
public class WaterMaterial : MaterialType
{
    public override string Name => "Water";
    public override Color BaseColor => Color.FromFloat(0.2f, 0.4f, 0.9f); // Blue

    public override float Mass => 1.0f;           // Standard reference mass
    public override float Stiffness => 1.0f;      // Moderate stiffness
    public override float Viscosity => 0.5f;      // Low viscosity (flows easily)
    public override float Restitution => 0.1f;    // Little bounce
    public override float Friction => 0.3f;       // Low friction
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 5f; // Moderate cohesion
    public override float RestDensity => 8.0f;    // Tight packing for water
}
