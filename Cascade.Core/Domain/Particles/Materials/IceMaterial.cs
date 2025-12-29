namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Ice: Solid, bouncy, slides easily, cold
/// </summary>
public class IceMaterial : MaterialType
{
    public override string Name => "Ice";
    public override Color BaseColor => Color.FromFloat(0.7f, 0.9f, 1.0f); // Light blue

    public override float Mass => 0.9f;           // Slightly lighter than water
    public override float Stiffness => 2.0f;      // Very stiff/rigid
    public override float Viscosity => 0.1f;      // Doesn't flow (solid)
    public override float Restitution => 0.6f;    // Quite bouncy
    public override float Friction => 0.1f;       // Very slippery
    public override float Temperature => -10.0f;  // Below freezing
    public override float CohesionStrength => 20f; // Strong bonds (solid)
    public override float RestDensity => 8.0f;    // Solid structure
    public override bool IsFluid => false;        // Solid state
}
