namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Lava: Dense, very hot, viscous, glows orange/red
/// </summary>
public class LavaMaterial : MaterialType
{
    public override string Name => "Lava";
    public override Color BaseColor => Color.FromFloat(1.0f, 0.3f, 0.0f); // Orange-red

    public override float Mass => 2.5f;           // Very heavy
    public override float Stiffness => 0.8f;      // Quite stiff but still flows
    public override float Viscosity => 3.5f;      // Very viscous
    public override float Restitution => 0.02f;   // No bounce
    public override float Friction => 0.9f;       // Very high friction
    public override float Temperature => 1200.0f; // Extremely hot
    public override float CohesionStrength => 12f; // Sticky and clumpy
    public override float RestDensity => 10.0f;   // Very dense
}
