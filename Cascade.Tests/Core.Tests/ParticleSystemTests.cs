using System.Numerics;
using FluentAssertions;
using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Unit tests for the particle system.
/// </summary>
public class ParticleSystemTests
{
    [Fact]
    public void Initialize_ShouldCreateSpecifiedNumberOfParticles()
    {
        // Arrange
        var system = new ParticleSystem();
        var bounds = new Bounds(0, 1000, 0, 1000);

        // Act
        system.Initialize(1000, bounds);

        // Assert
        system.Count.Should().Be(1000);
        system.Particles.Should().HaveCount(1000);
    }

    [Fact]
    public void Initialize_ShouldCreateParticlesWithinBounds()
    {
        // Arrange
        var system = new ParticleSystem();
        var bounds = new Bounds(100, 900, 100, 900);

        // Act
        system.Initialize(100, bounds);

        // Assert
        system.Particles.Should().AllSatisfy(p =>
        {
            p.Position.X.Should().BeInRange(100, 900);
            p.Position.Y.Should().BeInRange(100, 900);
        });
    }

    [Fact]
    public void AddParticle_ShouldIncreaseCount()
    {
        // Arrange
        var system = new ParticleSystem();
        var particle = new Particle(Vector2.Zero, Vector2.Zero, new WaterMaterial());

        // Act
        system.AddParticle(particle);

        // Assert
        system.Count.Should().Be(1);
        system.Particles.Should().Contain(particle);
    }

    [Fact]
    public void Clear_ShouldRemoveAllParticles()
    {
        // Arrange
        var system = new ParticleSystem();
        system.Initialize(100, new Bounds(0, 1000, 0, 1000));

        // Act
        system.Clear();

        // Assert
        system.Count.Should().Be(0);
        system.Particles.Should().BeEmpty();
    }

    [Fact]
    public void ParticleSystem_ShouldBeCreatable_WithoutStrideRuntime()
    {
        // Arrange & Act
        var system = new ParticleSystem();

        // Assert
        system.Should().NotBeNull("particle system should be pure C# with no framework dependencies");
    }
}
