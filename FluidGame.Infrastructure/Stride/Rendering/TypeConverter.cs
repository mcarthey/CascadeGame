using Stride.Core.Mathematics;
using DomainVector2 = FluidGame.Core.Domain.Particles.Vector2;
using DomainColor = FluidGame.Core.Domain.Particles.Color;

namespace FluidGame.Infrastructure.Stride.Rendering;

/// <summary>
/// Converts between our pure domain types and Stride's framework types.
/// This is the adapter layer that keeps our core domain clean.
/// </summary>
public static class TypeConverter
{
    public static Vector3 ToStrideVector3(DomainVector2 vector)
    {
        return new Vector3(vector.X, vector.Y, 0f);
    }

    public static Vector2 ToStrideVector2(DomainVector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }

    public static DomainVector2 ToDomainVector2(Vector2 vector)
    {
        return new DomainVector2(vector.X, vector.Y);
    }

    public static Color ToStrideColor(DomainColor color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }

    public static DomainColor ToDomainColor(Color color)
    {
        return new DomainColor(color.R, color.G, color.B, color.A);
    }

    public static Core.Application.Interfaces.Bounds ToDomainBounds(float width, float height)
    {
        return new Core.Application.Interfaces.Bounds(0, width, 0, height);
    }
}
