namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Snow: Sticky, clumpy, fluffy chunks - largest particles
/// </summary>
public class SnowMaterial : MaterialType
{
    public override string Name => "Snow";
    public override Color BaseColor => Color.FromFloat(0.95f, 0.95f, 1.0f); // Pale blue-white

    // Visual properties - CHUNKY large flakes
    public override float ParticleSize => 18.0f;      // Large fluffy chunks
    public override float SizeVariation => 0.4f;      // Lots of size variance

    // Physics - sticky, clumpy, slow-falling
    public override float Mass => 0.5f;           // Very light (floats down slowly)
    public override float Stiffness => 0.3f;      // Very soft/compressible
    public override float Viscosity => 4.0f;      // VERY sticky/viscous
    public override float Restitution => 0.02f;   // No bounce at all
    public override float Friction => 0.9f;       // Super high friction (sticks to everything)
    public override float Temperature => -5.0f;   // Below freezing
    public override float CohesionStrength => 25f; // VERY strong clumping
    public override float RestDensity => 4.0f;    // Loose, fluffy packing
}
