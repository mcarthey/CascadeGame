namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Oil: Light, floats on water, low friction, flammable
/// </summary>
public class OilMaterial : MaterialType
{
    public override string Name => "Oil";
    public override Color BaseColor => Color.FromFloat(0.3f, 0.2f, 0.1f); // Dark brown

    public override float Mass => 0.7f;           // Lighter than water
    public override float Stiffness => 0.8f;      // Flows moderately
    public override float Viscosity => 1.2f;      // Somewhat viscous
    public override float Restitution => 0.05f;   // Little bounce
    public override float Friction => 0.2f;       // Very slippery
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 8f; // Moderate cohesion
    public override float RestDensity => 7.0f;    // Medium density
}
