namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Sand: Granular, flows like fluid but settles into stable piles
/// </summary>
public class SandMaterial : MaterialType
{
    public override string Name => "Sand";
    public override Color BaseColor => Color.FromFloat(0.9f, 0.8f, 0.5f); // Tan/beige

    public override float Mass => 1.5f;           // Fairly heavy
    public override float Stiffness => 0.4f;      // Very compressible
    public override float Viscosity => 0.3f;      // Flows easily
    public override float Restitution => 0.2f;    // Some bounce
    public override float Friction => 0.8f;       // High friction
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 2f; // Very little cohesion
    public override float RestDensity => 4.0f;    // Loose packing
}
