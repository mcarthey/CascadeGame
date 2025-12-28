namespace FluidGame.Core.Domain.Particles;

/// <summary>
/// Simple color structure - pure domain type with no external dependencies.
/// </summary>
public struct Color
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    // Common colors for convenience
    public static Color White => new(255, 255, 255, 255);
    public static Color Black => new(0, 0, 0, 255);
    public static Color Red => new(255, 0, 0, 255);
    public static Color Green => new(0, 255, 0, 255);
    public static Color Blue => new(0, 0, 255, 255);
    public static Color Yellow => new(255, 255, 0, 255);
    public static Color Cyan => new(0, 255, 255, 255);
    public static Color Magenta => new(255, 0, 255, 255);
    public static Color Transparent => new(0, 0, 0, 0);

    /// <summary>
    /// Creates a color from normalized float values (0.0 to 1.0)
    /// </summary>
    public static Color FromFloat(float r, float g, float b, float a = 1.0f)
    {
        return new Color(
            (byte)(Math.Clamp(r, 0f, 1f) * 255),
            (byte)(Math.Clamp(g, 0f, 1f) * 255),
            (byte)(Math.Clamp(b, 0f, 1f) * 255),
            (byte)(Math.Clamp(a, 0f, 1f) * 255)
        );
    }

    public override string ToString() => $"R:{R} G:{G} B:{B} A:{A}";
}
