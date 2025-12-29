using System.Numerics;
using Cascade.Core.Domain.Particles;

namespace Cascade.Core.Domain.Physics;

public class SpatialGrid
{
    private readonly int _bucketCount;
    private readonly List<Particle>[] _buckets;
    private readonly float _cellSize;

    public SpatialGrid(float cellSize, int expectedParticleCount = 2000)
    {
        _cellSize = cellSize;
        // We use more buckets than particles to reduce collisions
        _bucketCount = expectedParticleCount * 2;
        _buckets = new List<Particle>[_bucketCount];

        for (var i = 0; i < _bucketCount; i++)
        {
            _buckets[i] = new List<Particle>(32);
        }
    }

    public IEnumerable<Particle> GetNeighbors(Vector2 position)
    {
        var cellX = (int)MathF.Floor(position.X / _cellSize);
        var cellY = (int)MathF.Floor(position.Y / _cellSize);

        // Check the 9-cell neighborhood
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var index = GetHash(cellX + x, cellY + y);
                var bucket = _buckets[index];
                for (var i = 0; i < bucket.Count; i++)
                {
                    yield return bucket[i];
                }
            }
        }
    }

    public void Update(IList<Particle> particles)
    {
        // Clear all buckets
        for (var i = 0; i < _bucketCount; i++)
        {
            _buckets[i].Clear();
        }

        foreach (var p in particles)
        {
            // Safety check to prevent index out of bounds on explosion
            if (!float.IsFinite(p.Position.X) || !float.IsFinite(p.Position.Y))
            {
                continue;
            }

            var index = GetBucketIndex(p.Position);
            _buckets[index].Add(p);
        }
    }

    private int GetBucketIndex(Vector2 pos)
    {
        return GetHash((int)MathF.Floor(pos.X / _cellSize), (int)MathF.Floor(pos.Y / _cellSize));
    }

    private int GetHash(int x, int y)
    {
        // Spatial Hash function: maps 2D coordinates to a 1D array index
        // Using large primes to distribute particles evenly
        var h = (uint)((x * 73856093) ^ (y * 19349663));
        return (int)(h % (uint)_bucketCount);
    }
}
