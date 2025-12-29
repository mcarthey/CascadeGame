using Stride.Core.Mathematics;
using System.Numerics; // The new Core Vector2
using DomainColor = Cascade.Core.Domain.Particles.Color;
using CoreVector2 = System.Numerics.Vector2; // Alias to prevent name collision
using StrideVector2 = Stride.Core.Mathematics.Vector2;
using StrideVector3 = Stride.Core.Mathematics.Vector3;

namespace Cascade.Infrastructure.Stride.Rendering;

/// <summary>
/// Converts between our pure domain types (System.Numerics) and Stride's framework types.
/// This is the adapter layer that keeps our core domain clean.
/// </summary>
public static class TypeConverter
{
    public static StrideVector3 ToStrideVector3(CoreVector2 vector)
    {
        return new StrideVector3(vector.X, vector.Y, 0f);
    }

    public static StrideVector2 ToStrideVector2(CoreVector2 vector)
    {
        return new StrideVector2(vector.X, vector.Y);
    }

    public static CoreVector2 ToDomainVector2(StrideVector2 vector)
    {
        return new CoreVector2(vector.X, vector.Y);
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
        // Assuming 0,0 is bottom-left for your simulation space
        return new Core.Application.Interfaces.Bounds(0, width, 0, height);
    }
}
