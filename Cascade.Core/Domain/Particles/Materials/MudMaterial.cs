namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Mud: Heavy, very viscous, sticky, flows slowly
/// </summary>
public class MudMaterial : MaterialType
{
    public override string Name => "Mud";
    public override Color BaseColor => Color.FromFloat(0.4f, 0.3f, 0.2f); // Brown

    public override float Mass => 1.4f;           // Heavier than water
    public override float Stiffness => 0.5f;      // Very compressible
    public override float Viscosity => 4.0f;      // Extremely viscous
    public override float Restitution => 0.01f;   // No bounce at all
    public override float Friction => 0.95f;      // Extremely high friction
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 18f; // Very sticky
    public override float RestDensity => 5.0f;    // Can be quite loose
}
