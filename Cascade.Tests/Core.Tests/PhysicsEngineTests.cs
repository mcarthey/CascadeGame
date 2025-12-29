using System.Numerics;
using FluentAssertions;
using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Cascade.Core.Domain.Physics;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Unit tests for the physics engine.
/// Notice: Zero dependencies on Stride - pure domain logic testing!
/// </summary>
public class PhysicsEngineTests
{
    [Fact]
    public void Gravity_ShouldIncreaseParticleVelocity_OverTime()
    {
        // Arrange
        var gravity = new Vector2(0, 100f); // 100 pixels/sec² downward
        var bounds = new Bounds(0, 1000, 0, 1000);
        var engine = new SimplePhysicsEngine(gravity, bounds);

        var particle = new Particle(
            position: new Vector2(500, 500),
            velocity: Vector2.Zero,
            material: new WaterMaterial()
        );

        var particles = new List<Particle> { particle };
        float deltaTime = 0.1f; // 100ms

        // Act
        engine.Update(particles, deltaTime);

        // Assert
        particle.Velocity.Y.Should().Be(10f, "gravity should accelerate particle downward");
        particle.Position.Y.Should().Be(501f, "particle should move down by velocity * deltaTime");
    }

    [Fact]
    public void Gravity_ShouldContinuouslyAccelerate_Particle()
    {
        // Arrange
        var gravity = new Vector2(0, 100f);
        var bounds = new Bounds(0, 1000, 0, 1000);
        var engine = new SimplePhysicsEngine(gravity, bounds);

        var particle = new Particle(
            position: new Vector2(500, 100),
            velocity: Vector2.Zero,
            material: new WaterMaterial()
        );

        var particles = new List<Particle> { particle };
        float deltaTime = 0.1f;

        // Act - simulate 3 frames
        engine.Update(particles, deltaTime);
        var velocityAfterFrame1 = particle.Velocity.Y;

        engine.Update(particles, deltaTime);
        var velocityAfterFrame2 = particle.Velocity.Y;

        engine.Update(particles, deltaTime);
        var velocityAfterFrame3 = particle.Velocity.Y;

        // Assert
        velocityAfterFrame1.Should().Be(10f);
        velocityAfterFrame2.Should().Be(20f);
        velocityAfterFrame3.Should().Be(30f);
    }

    [Fact]
    public void Particle_ShouldWrapAround_WhenExceedingBounds()
    {
        // Arrange
        var gravity = Vector2.Zero;
        var bounds = new Bounds(0, 1000, 0, 1000);
        var engine = new SimplePhysicsEngine(gravity, bounds);

        var particle = new Particle(
            position: new Vector2(999, 500),
            velocity: new Vector2(100, 0), // Moving right fast
            material: new WaterMaterial()
        );

        var particles = new List<Particle> { particle };
        float deltaTime = 0.1f; // Will move 10 pixels right

        // Act
        engine.Update(particles, deltaTime);

        // Assert
        particle.Position.X.Should().Be(0f, "particle should wrap around to left side");
    }

    [Fact]
    public void MultipleParticles_ShouldAllBeAffected_ByGravity()
    {
        // Arrange
        var gravity = new Vector2(0, 100f);
        var bounds = new Bounds(0, 1000, 0, 1000);
        var engine = new SimplePhysicsEngine(gravity, bounds);

        var particles = new List<Particle>
        {
            new(new Vector2(100, 100), Vector2.Zero, new WaterMaterial()),
            new(new Vector2(200, 200), Vector2.Zero, new WaterMaterial()),
            new(new Vector2(300, 300), Vector2.Zero, new WaterMaterial())
        };

        float deltaTime = 0.1f;

        // Act
        engine.Update(particles, deltaTime);

        // Assert
        particles.Should().AllSatisfy(p =>
        {
            p.Velocity.Y.Should().Be(10f, "all particles should be affected by gravity equally");
        });
    }

    [Fact]
    public void PhysicsEngine_ShouldBeCreatable_WithoutStrideRuntime()
    {
        // Arrange & Act
        var engine = new SimplePhysicsEngine(
            new Vector2(0, 100),
            new Bounds(0, 1000, 0, 1000)
        );

        // Assert
        engine.Should().NotBeNull("physics engine should be pure C# with no framework dependencies");
        engine.Gravity.Should().Be(new Vector2(0, 100));
    }
}
