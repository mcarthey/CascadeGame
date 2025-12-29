using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using System.Numerics;

namespace Cascade.Core.Domain.Physics
{
    public class SimplePhysicsEngine : IPhysicsEngine
    {
        public Vector2 Gravity { get; set; }
        public Bounds SimulationBounds { get; set; }
        private readonly SpatialGrid _grid;
        private const float InteractionRadius = 15.0f;
        private int _resetCount = 0;

        public SimplePhysicsEngine(Vector2 gravity, Bounds simulationBounds)
        {
            Gravity = gravity;
            SimulationBounds = simulationBounds;
            _grid = new SpatialGrid(InteractionRadius);
        }

        public void Update(IList<Particle> particles, float deltaTime)
        {
            // 1. Stability: Don't allow massive timesteps if the game lags
            float dt = MathF.Min(deltaTime, 0.0166f);

            _grid.Update(particles);

            // 2. Density Pass
            foreach (var p in particles)
            {
                p.Density = 0;
                foreach (var neighbor in _grid.GetNeighbors(p.Position))
                {
                    float dist = Vector2.Distance(p.Position, neighbor.Position);
                    if (dist < InteractionRadius)
                    {
                        float influence = 1.0f - (dist / InteractionRadius);
                        p.Density += influence * influence;
                    }
                }
                if (p.Density < 1.0f) p.Density = 1.0f;
            }

            // 3. Force & Integration Pass
            foreach (var p in particles)
            {
                Vector2 force = Gravity + CalculateSPHForces(p);

                p.Velocity += force * dt;

                // 4. Global Damping: Helps particles settle into stable piles
                // This mimics air resistance and internal friction
                p.Velocity *= 0.992f; // Very slight damping per frame

                // 5. Terminal Velocity: Prevents the "Static" jump effect
                float speedSq = p.Velocity.LengthSquared();
                float maxSpeed = 600f;
                if (speedSq > maxSpeed * maxSpeed)
                {
                    p.Velocity = Vector2.Normalize(p.Velocity) * maxSpeed;
                }

                p.Position += p.Velocity * dt;

                // 6. Bounds & Safety
                if (!float.IsFinite(p.Position.X) || !float.IsFinite(p.Position.Y))
                {
                    _resetCount++;
                    System.Diagnostics.Debug.WriteLine($"[SimplePhysicsEngine] Resetting particle #{_resetCount} due to non-finite position (X={p.Position.X}, Y={p.Position.Y})");
                    ResetParticle(p);
                }
                ApplyBoundaryConstraints(p);
            }
        }

        private Vector2 CalculateSPHForces(Particle p)
        {
            Vector2 pressureForce = Vector2.Zero;
            Vector2 viscosityForce = Vector2.Zero;
            Vector2 cohesionForce = Vector2.Zero;

            // Use material properties for physics behavior
            float targetDensity = p.Material?.RestDensity ?? 6.0f;
            float pressureMultiplier = p.Material?.Stiffness ?? 3f;
            float viscosityStrength = p.Material?.Viscosity ?? 2.5f;
            float cohesionStrength = p.Material?.CohesionStrength ?? 15f;

            foreach (var neighbor in _grid.GetNeighbors(p.Position))
            {
                if (ReferenceEquals(p, neighbor)) continue;

                float dist = Vector2.Distance(p.Position, neighbor.Position);
                if (dist < InteractionRadius && dist > 0.1f)
                {
                    float influence = 1.0f - (dist / InteractionRadius);
                    Vector2 dir = Vector2.Normalize(neighbor.Position - p.Position);

                    // Pressure: Gentle repulsion when too crowded
                    float sharedPressure = (p.Density + neighbor.Density - (2 * targetDensity)) * pressureMultiplier;
                    pressureForce -= dir * sharedPressure * influence;

                    // Viscosity: Makes particles move together (the "syrup" effect)
                    viscosityForce += (neighbor.Velocity - p.Velocity) * influence * viscosityStrength;

                    // Cohesion: Weak attraction to nearby particles (cornstarch clumping)
                    // Attraction is stronger at medium distances, weaker when very close
                    float cohesionInfluence = influence * (1.0f - influence); // Peak at mid-distance
                    cohesionForce += dir * cohesionInfluence * cohesionStrength;
                }
            }

            var combined = pressureForce + viscosityForce + cohesionForce;

            // Defensive: avoid NaN/Infinity and clamp large forces that cause instability
            if (!float.IsFinite(combined.X) || !float.IsFinite(combined.Y) || float.IsNaN(combined.X) || float.IsNaN(combined.Y))
                return Vector2.Zero;

            const float maxForce = 1200f;
            if (combined.LengthSquared() > maxForce * maxForce)
            {
                combined = Vector2.Normalize(combined) * maxForce;
            }

            return combined;
        }

        private void ResetParticle(Particle p)
        {
            // Spawn at a random X at the TOP (MinY)
            var random = new Random();
            p.Position = new Vector2(random.Next((int)SimulationBounds.MinX, (int)SimulationBounds.MaxX), SimulationBounds.MinY + 10);
            p.Velocity = Vector2.Zero;
        }

        private void ApplyBoundaryConstraints(Particle p)
        {
            // Use material properties for boundary interaction
            float bounce = p.Material?.Restitution ?? 0.05f;
            float friction = p.Material?.Friction ?? 0.75f;

            // FLOOR (Bottom of screen)
            if (p.Position.Y > SimulationBounds.MaxY)
            {
                p.Position = new Vector2(p.Position.X, SimulationBounds.MaxY);

                // Kill vertical velocity and apply friction to horizontal
                p.Velocity = new Vector2(p.Velocity.X * friction, 0f);
                return;
            }

            // CEILING (Top of screen)
            if (p.Position.Y < SimulationBounds.MinY)
            {
                p.Position = new Vector2(p.Position.X, SimulationBounds.MinY);
                p.Velocity = new Vector2(p.Velocity.X, p.Velocity.Y * -bounce);
            }

            // LEFT WALL
            if (p.Position.X < SimulationBounds.MinX)
            {
                p.Position = new Vector2(SimulationBounds.MinX, p.Position.Y);
                p.Velocity = new Vector2(p.Velocity.X * -bounce, p.Velocity.Y);
            }
            // RIGHT WALL
            else if (p.Position.X > SimulationBounds.MaxX)
            {
                p.Position = new Vector2(SimulationBounds.MaxX, p.Position.Y);
                p.Velocity = new Vector2(p.Velocity.X * -bounce, p.Velocity.Y);
            }
        }
    }
}