using System.Collections.Generic;
using System.Numerics; // Use the standard .NET math library
using Cascade.Core.Domain.Particles;
using Vector2 = System.Numerics.Vector2;

namespace Cascade.Core.Application.Interfaces;

public interface IPhysicsEngine
{
    void Update(IList<Particle> particles, float deltaTime);

    // Ensure this uses System.Numerics.Vector2
    Vector2 Gravity { get; set; }

    Bounds SimulationBounds { get; set; }
}

public struct Bounds
{
    public float MinX { get; set; }
    public float MaxX { get; set; }
    public float MinY { get; set; }
    public float MaxY { get; set; }

    public Bounds(float minX, float maxX, float minY, float maxY)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
    }

    public float Width => MaxX - MinX;
    public float Height => MaxY - MinY;
}
