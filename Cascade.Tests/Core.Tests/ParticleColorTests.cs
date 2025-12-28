using FluentAssertions;
using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Tests to ensure particles are visible against common backgrounds.
/// These tests help prevent rendering issues where particles blend into the background.
/// </summary>
public class ParticleColorTests
{
    // Cornflower blue (common Stride/XNA clear color) RGB: 100, 149, 237
    private static readonly Color CornflowerBlue = new(100, 149, 237, 255);

    [Fact]
    public void InitializedParticles_ShouldHaveVisibleColors()
    {
        // Arrange
        var system = new ParticleSystem();
        var bounds = new Bounds(0, 1280, 0, 720);

        // Act
        system.Initialize(100, bounds);

        // Assert - particles should have colors that contrast with blue background
        system.Particles.Should().AllSatisfy(p =>
        {
            // At least one color channel should be significantly different from cornflower blue
            var colorDistance = CalculateColorDistance(p.Color, CornflowerBlue);
            colorDistance.Should().BeGreaterThan(50,
                $"Particle color ({p.Color.R}, {p.Color.G}, {p.Color.B}) should contrast with background");
        });
    }

    [Fact]
    public void InitializedParticles_ShouldNotBeTransparent()
    {
        // Arrange
        var system = new ParticleSystem();
        var bounds = new Bounds(0, 1280, 0, 720);

        // Act
        system.Initialize(100, bounds);

        // Assert - all particles should be fully opaque
        system.Particles.Should().AllSatisfy(p =>
        {
            p.Color.A.Should().Be(255, "particles should be fully opaque");
        });
    }

    [Fact]
    public void InitializedParticles_ShouldHaveValidPositions()
    {
        // Arrange
        var system = new ParticleSystem();
        var bounds = new Bounds(0, 1280, 0, 720);

        // Act
        system.Initialize(100, bounds);

        // Assert - all positions should be within visible screen area
        system.Particles.Should().AllSatisfy(p =>
        {
            p.Position.X.Should().BeInRange(0, 1280, "X should be on screen");
            p.Position.Y.Should().BeInRange(0, 720, "Y should be on screen");
        });
    }

    [Theory]
    [InlineData(0, 0)]       // Top-left
    [InlineData(1280, 0)]    // Top-right
    [InlineData(0, 720)]     // Bottom-left
    [InlineData(1280, 720)]  // Bottom-right
    [InlineData(640, 360)]   // Center
    public void ManualParticle_ShouldBeCreatableAtAnyPosition(float x, float y)
    {
        // Arrange & Act
        var particle = new Particle(
            new Vector2(x, y),
            Vector2.Zero,
            Color.White,
            ParticleType.Water,
            1.0f
        );

        // Assert
        particle.Position.X.Should().Be(x);
        particle.Position.Y.Should().Be(y);
    }

    private static double CalculateColorDistance(Color a, Color b)
    {
        // Simple Euclidean distance in RGB space
        var dr = a.R - b.R;
        var dg = a.G - b.G;
        var db = a.B - b.B;
        return Math.Sqrt(dr * dr + dg * dg + db * db);
    }
}
