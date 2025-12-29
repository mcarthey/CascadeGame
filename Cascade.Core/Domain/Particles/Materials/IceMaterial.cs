namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Ice: Hard crystalline chunks that bounce and slide - solid!
/// </summary>
public class IceMaterial : MaterialType
{
    public override string Name => "Ice";
    public override Color BaseColor => Color.FromFloat(0.7f, 0.9f, 1.0f); // Light cyan

    // Visual properties - solid chunks
    public override float ParticleSize => 13.0f;      // Medium-sized crystals
    public override float SizeVariation => 0.35f;     // Varied chunks

    // Physics - solid, bouncy, slides
    public override float Mass => 1.1f;           // Slightly heavier than water
    public override float Stiffness => 5.0f;      // VERY stiff/rigid (solid)
    public override float Viscosity => 0.05f;     // Doesn't flow (solid)
    public override float Restitution => 0.7f;    // VERY bouncy!
    public override float Friction => 0.05f;      // Super slippery (slides forever)
    public override float Temperature => -10.0f;  // Below freezing
    public override float CohesionStrength => 25f; // Strong bonds (solid)
    public override float RestDensity => 8.0f;    // Solid structure
    public override bool IsFluid => false;        // Solid state
}
