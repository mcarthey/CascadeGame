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

                // 4. Terminal Velocity: Prevents the "Static" jump effect
                float speedSq = p.Velocity.LengthSquared();
                float maxSpeed = 600f;
                if (speedSq > maxSpeed * maxSpeed)
                {
                    p.Velocity = Vector2.Normalize(p.Velocity) * maxSpeed;
                }

                p.Position += p.Velocity * dt;

                // 5. Bounds & Safety
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

            // Tuning for "Stacking Snow" behavior
            float targetDensity = 5.0f;       // INCREASED: Allows particles to pack tighter
            float pressureMultiplier = 8f;    // Clamped lower to avoid huge forces
            float viscosityStrength = 1.5f;   // INCREASED: Makes them "stick" together like wet snow

            foreach (var neighbor in _grid.GetNeighbors(p.Position))
            {
                if (ReferenceEquals(p, neighbor)) continue;

                float dist = Vector2.Distance(p.Position, neighbor.Position);
                if (dist < InteractionRadius && dist > 0.1f)
                {
                    float influence = 1.0f - (dist / InteractionRadius);
                    Vector2 dir = Vector2.Normalize(neighbor.Position - p.Position);

                    // If density is below target, this force becomes attractive or neutral, 
                    // preventing the sudden explosion when they touch.
                    float sharedPressure = (p.Density + neighbor.Density - (2 * targetDensity)) * pressureMultiplier;
                    pressureForce -= dir * sharedPressure * influence;

                    viscosityForce += (neighbor.Velocity - p.Velocity) * influence * viscosityStrength;
                }
            }

            var combined = pressureForce + viscosityForce;

            // Defensive: avoid NaN/Infinity and clamp large forces that cause instability
            if (!float.IsFinite(combined.X) || !float.IsFinite(combined.Y) || float.IsNaN(combined.X) || float.IsNaN(combined.Y))
                return Vector2.Zero;

            const float maxForce = 1000f;
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
            float bounce = 0.1f;    // Low bounce for snow
            float friction = 0.85f; // Friction to damp horizontal motion so they settle

            // FLOOR (Bottom of screen)
            if (p.Position.Y > SimulationBounds.MaxY)
            {
                p.Position = new Vector2(p.Position.X, SimulationBounds.MaxY);

                // Zero vertical velocity and heavily damp horizontal to make them stick
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