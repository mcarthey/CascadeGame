using System.Numerics;
using FluentAssertions;
using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Unit tests for the ParticleEmitter system.
/// Ensures particles can be emitted correctly with various materials.
/// </summary>
public class ParticleEmitterTests
{
    [Fact]
    public void EmitParticle_ShouldAddParticleToSystem()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new WaterMaterial();
        var position = new Vector2(100, 100);

        // Act
        emitter.EmitParticle(position, material);

        // Assert
        particleSystem.Count.Should().Be(1);
        particleSystem.Particles[0].Material.Should().Be(material);
    }

    [Fact]
    public void EmitParticle_ShouldUseSpecifiedPosition()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new SnowMaterial();
        var position = new Vector2(250, 350);

        // Act
        emitter.EmitParticle(position, material);

        // Assert
        particleSystem.Particles[0].Position.Should().Be(position);
    }

    [Fact]
    public void EmitSpray_ShouldCreateMultipleParticles()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new LavaMaterial();
        var position = new Vector2(500, 300);
        int count = 10;

        // Act
        emitter.EmitSpray(position, material, count: count);

        // Assert
        particleSystem.Count.Should().Be(count);
        particleSystem.Particles.Should().AllSatisfy(p =>
        {
            p.Material.Should().Be(material);
        });
    }

    [Fact]
    public void EmitSpray_ShouldSpreadParticlesAroundPosition()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new OilMaterial();
        var centerPosition = new Vector2(400, 400);

        // Act
        emitter.EmitSpray(centerPosition, material, count: 20);

        // Assert - particles should be spread around the center
        var positions = particleSystem.Particles.Select(p => p.Position).ToList();
        positions.Should().NotAllBeEquivalentTo(centerPosition, "particles should spread out");

        // Check that particles are reasonably close to the center
        positions.Should().AllSatisfy(pos =>
        {
            var distance = Vector2.Distance(pos, centerPosition);
            distance.Should().BeLessThan(50f, "particles should spawn near spray position");
        });
    }

    [Fact]
    public void EmitSpray_ShouldGiveParticlesInitialVelocity()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new MudMaterial();
        var position = new Vector2(300, 300);

        // Act
        emitter.EmitSpray(position, material, count: 5, velocity: 150f);

        // Assert - all particles should have non-zero velocity
        particleSystem.Particles.Should().AllSatisfy(p =>
        {
            p.Velocity.LengthSquared().Should().BeGreaterThan(0, "sprayed particles should have initial velocity");
        });
    }

    [Theory]
    [InlineData(typeof(SnowMaterial))]
    [InlineData(typeof(WaterMaterial))]
    [InlineData(typeof(LavaMaterial))]
    [InlineData(typeof(OilMaterial))]
    [InlineData(typeof(MudMaterial))]
    [InlineData(typeof(SandMaterial))]
    [InlineData(typeof(SteamMaterial))]
    [InlineData(typeof(IceMaterial))]
    public void EmitParticle_ShouldWorkWithAllMaterialTypes(Type materialType)
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = (MaterialType)Activator.CreateInstance(materialType)!;
        var position = new Vector2(200, 200);

        // Act
        emitter.EmitParticle(position, material);

        // Assert
        particleSystem.Count.Should().Be(1);
        particleSystem.Particles[0].Material.Should().BeOfType(materialType);
        particleSystem.Particles[0].Color.Should().NotBe(default(Color), "particle should have a color from the material");
    }

    [Fact]
    public void EmitSpray_WithCustomSpreadAngle_ShouldAffectParticleDirection()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new SandMaterial();
        var position = new Vector2(500, 500);

        // Act - emit with wide spread
        emitter.EmitSpray(position, material, count: 30, spreadAngle: 1.57f); // ~90 degrees

        // Assert - particles should have varied velocities
        var velocities = particleSystem.Particles.Select(p => p.Velocity).ToList();
        var uniqueDirections = velocities.Select(v => MathF.Atan2(v.Y, v.X)).Distinct().Count();

        uniqueDirections.Should().BeGreaterThan(10, "wide spread should create many different directions");
    }

    [Fact]
    public void EmitSpray_WithZeroCount_ShouldNotAddParticles()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material = new IceMaterial();

        // Act
        emitter.EmitSpray(Vector2.Zero, material, count: 0);

        // Assert
        particleSystem.Count.Should().Be(0);
    }

    [Fact]
    public void ParticleEmitter_ShouldRequireParticleSystem()
    {
        // Arrange & Act
        Action createEmitterWithNull = () => new ParticleEmitter(null!);

        // Assert
        createEmitterWithNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void EmitMultipleSprays_ShouldAccumulateParticles()
    {
        // Arrange
        var particleSystem = new ParticleSystem();
        var emitter = new ParticleEmitter(particleSystem);
        var material1 = new SnowMaterial();
        var material2 = new WaterMaterial();

        // Act - emit two different sprays
        emitter.EmitSpray(new Vector2(100, 100), material1, count: 5);
        emitter.EmitSpray(new Vector2(200, 200), material2, count: 7);

        // Assert
        particleSystem.Count.Should().Be(12);

        var snowParticles = particleSystem.Particles.Count(p => p.Material is SnowMaterial);
        var waterParticles = particleSystem.Particles.Count(p => p.Material is WaterMaterial);

        snowParticles.Should().Be(5);
        waterParticles.Should().Be(7);
    }
}
