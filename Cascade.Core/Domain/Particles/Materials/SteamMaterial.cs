namespace Cascade.Core.Domain.Particles.Materials;

/// <summary>
/// Steam: Very light, rises (negative gravity), dissipates over time
/// </summary>
public class SteamMaterial : MaterialType
{
    public override string Name => "Steam";
    public override Color BaseColor => Color.FromFloat(0.9f, 0.9f, 0.95f, 0.6f); // Translucent white

    public override float Mass => 0.1f;           // Very light
    public override float Stiffness => 0.1f;      // Almost no stiffness (gas)
    public override float Viscosity => 0.1f;      // Flows very freely
    public override float Restitution => 0.3f;    // Some bounce
    public override float Friction => 0.05f;      // Almost no friction
    public override float Temperature => 100.0f;  // Hot
    public override float CohesionStrength => 0.5f; // Almost no cohesion
    public override float RestDensity => 1.0f;    // Very loose
    public override bool AffectedByGravity => true; // But should rise (handled specially)
}
