namespace FluidGame.Core.Domain.Particles;

/// <summary>
/// Simple 2D vector structure - pure domain type with no external dependencies.
/// Keeps the core domain completely independent of any rendering framework.
/// </summary>
public struct Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);
    public static Vector2 UnitX => new(1, 0);
    public static Vector2 UnitY => new(0, 1);

    public float Length => MathF.Sqrt(X * X + Y * Y);
    public float LengthSquared => X * X + Y * Y;

    public Vector2 Normalized()
    {
        float length = Length;
        return length > 0 ? new Vector2(X / length, Y / length) : Zero;
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 v, float scalar) => new(v.X * scalar, v.Y * scalar);
    public static Vector2 operator *(float scalar, Vector2 v) => new(v.X * scalar, v.Y * scalar);
    public static Vector2 operator /(Vector2 v, float scalar) => new(v.X / scalar, v.Y / scalar);

    public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
    public static float Distance(Vector2 a, Vector2 b) => (a - b).Length;
    public static float DistanceSquared(Vector2 a, Vector2 b) => (a - b).LengthSquared;

    // Equality operators for tests and comparisons
    public static bool operator ==(Vector2 left, Vector2 right) =>
        left.X == right.X && left.Y == right.Y;

    public static bool operator !=(Vector2 left, Vector2 right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        obj is Vector2 other && this == other;

    public override int GetHashCode() =>
        HashCode.Combine(X, Y);

    public override string ToString() => $"({X}, {Y})";
}
