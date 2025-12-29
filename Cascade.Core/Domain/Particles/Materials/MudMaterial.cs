namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Mud: Thick sludge that oozes and sticks - very slow
/// </summary>
public class MudMaterial : MaterialType
{
    public override string Name => "Mud";
    public override Color BaseColor => Color.FromFloat(0.4f, 0.3f, 0.2f); // Brown

    // Visual properties - thick chunky globs
    public override float ParticleSize => 12.0f;      // Thick chunky globs
    public override float SizeVariation => 0.3f;      // Varied globs

    // Physics - heavy, VERY slow, VERY sticky
    public override float Mass => 2.0f;           // Heavy
    public override float Stiffness => 0.4f;      // Soft/compressible
    public override float Viscosity => 10.0f;     // RIDICULOUSLY viscous (barely moves)
    public override float Restitution => 0.0f;    // Zero bounce (splats)
    public override float Friction => 0.98f;      // Extreme friction
    public override float Temperature => 20.0f;   // Room temperature
    public override float CohesionStrength => 20f; // Super sticky
    public override float RestDensity => 5.0f;    // Can be quite loose
}
