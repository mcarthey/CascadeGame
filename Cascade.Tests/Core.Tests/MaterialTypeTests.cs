using FluentAssertions;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Unit tests for the MaterialType system.
/// Ensures all material types have valid properties and behave correctly.
/// </summary>
public class MaterialTypeTests
{
    [Theory]
    [InlineData(typeof(SnowMaterial))]
    [InlineData(typeof(WaterMaterial))]
    [InlineData(typeof(LavaMaterial))]
    [InlineData(typeof(OilMaterial))]
    [InlineData(typeof(MudMaterial))]
    [InlineData(typeof(SandMaterial))]
    [InlineData(typeof(SteamMaterial))]
    [InlineData(typeof(IceMaterial))]
    public void AllMaterials_ShouldHaveValidNames(Type materialType)
    {
        // Arrange & Act
        var material = (MaterialType)Activator.CreateInstance(materialType)!;

        // Assert
        material.Name.Should().NotBeNullOrWhiteSpace("all materials should have a name");
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
    public void AllMaterials_ShouldHavePositiveMass(Type materialType)
    {
        // Arrange & Act
        var material = (MaterialType)Activator.CreateInstance(materialType)!;

        // Assert
        material.Mass.Should().BeGreaterThan(0, "all materials should have positive mass");
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
    public void AllMaterials_ShouldHaveValidPhysicsProperties(Type materialType)
    {
        // Arrange & Act
        var material = (MaterialType)Activator.CreateInstance(materialType)!;

        // Assert
        material.Stiffness.Should().BeGreaterThan(0, "stiffness should be positive");
        material.Viscosity.Should().BeGreaterThanOrEqualTo(0, "viscosity should be non-negative");
        material.Restitution.Should().BeInRange(0, 1, "restitution (bounce) should be between 0 and 1");
        material.Friction.Should().BeInRange(0, 1, "friction should be between 0 and 1");
        material.RestDensity.Should().BeGreaterThan(0, "rest density should be positive");
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
    public void AllMaterials_ShouldGenerateOpaqueColors(Type materialType)
    {
        // Arrange & Act
        var material = (MaterialType)Activator.CreateInstance(materialType)!;
        var color = material.GetParticleColor();

        // Assert
        color.A.Should().Be(255, "particles should be fully opaque");
    }

    [Fact]
    public void SnowMaterial_ShouldHaveHighCohesion()
    {
        // Arrange & Act
        var snow = new SnowMaterial();

        // Assert
        snow.CohesionStrength.Should().BeGreaterThan(10f, "snow should be sticky and clumpy");
        snow.Viscosity.Should().BeGreaterThan(2f, "snow should be viscous");
        snow.Restitution.Should().BeLessThan(0.1f, "snow should not bounce much");
    }

    [Fact]
    public void WaterMaterial_ShouldBeLowViscosity()
    {
        // Arrange & Act
        var water = new WaterMaterial();

        // Assert
        water.Viscosity.Should().BeLessThan(2f, "water should flow easily");
        water.Mass.Should().BeApproximately(1.0f, 0.1f, "water should be standard density");
    }

    [Fact]
    public void LavaMaterial_ShouldBeHotAndDense()
    {
        // Arrange & Act
        var lava = new LavaMaterial();

        // Assert
        lava.Temperature.Should().BeGreaterThan(500f, "lava should be very hot");
        lava.Mass.Should().BeGreaterThan(1.5f, "lava should be dense");
        lava.Viscosity.Should().BeGreaterThan(3f, "lava should be viscous");
    }

    [Fact]
    public void SteamMaterial_ShouldBeLightAndRise()
    {
        // Arrange & Act
        var steam = new SteamMaterial();

        // Assert
        steam.Mass.Should().BeLessThan(0.3f, "steam should be very light");
        steam.AffectedByGravity.Should().BeTrue("steam should have effective negative gravity");
    }

    [Fact]
    public void OilMaterial_ShouldBeSlippery()
    {
        // Arrange & Act
        var oil = new OilMaterial();

        // Assert
        oil.Friction.Should().BeLessThan(0.3f, "oil should be slippery");
        oil.Mass.Should().BeLessThan(1.0f, "oil should float on water");
    }

    [Fact]
    public void SandMaterial_ShouldHaveHighFriction()
    {
        // Arrange & Act
        var sand = new SandMaterial();

        // Assert
        sand.Friction.Should().BeGreaterThan(0.7f, "sand should have high friction");
        sand.IsFluid.Should().BeFalse("sand is granular, not truly fluid");
    }

    [Fact]
    public void MudMaterial_ShouldBeVeryViscous()
    {
        // Arrange & Act
        var mud = new MudMaterial();

        // Assert
        mud.Viscosity.Should().BeGreaterThan(4f, "mud should be very viscous");
        mud.Mass.Should().BeGreaterThan(1.2f, "mud should be heavier than water");
    }

    [Fact]
    public void IceMaterial_ShouldBeSlipperyAndHeavy()
    {
        // Arrange & Act
        var ice = new IceMaterial();

        // Assert
        ice.Friction.Should().BeLessThan(0.3f, "ice should be slippery");
        ice.Mass.Should().BeGreaterThan(0.9f, "ice should be nearly as dense as water");
        ice.Temperature.Should().BeLessThan(5f, "ice should be cold");
    }

    [Fact]
    public void MaterialColorVariation_ShouldProduceDifferentColors()
    {
        // Arrange
        var material = new SnowMaterial();
        var colors = new HashSet<Color>();

        // Act - generate 50 particle colors
        for (int i = 0; i < 50; i++)
        {
            colors.Add(material.GetParticleColor());
        }

        // Assert - should have some variation (not all identical)
        colors.Count.Should().BeGreaterThan(1, "material colors should have variation");
    }

    [Fact]
    public void Particle_ShouldUseMatchingMaterialColor()
    {
        // Arrange
        var material = new LavaMaterial();
        var position = new System.Numerics.Vector2(100, 100);
        var velocity = System.Numerics.Vector2.Zero;

        // Act
        var particle = new Particle(position, velocity, material);

        // Assert
        particle.Color.R.Should().BeGreaterThan(200, "lava should be red/orange");
        particle.Material.Should().Be(material);
    }

    [Fact]
    public void Particle_Mass_ShouldMatchMaterialMass()
    {
        // Arrange
        var heavyMaterial = new LavaMaterial(); // Mass 1.8f
        var lightMaterial = new SteamMaterial(); // Mass 0.2f

        var heavyParticle = new Particle(
            System.Numerics.Vector2.Zero,
            System.Numerics.Vector2.Zero,
            heavyMaterial
        );

        var lightParticle = new Particle(
            System.Numerics.Vector2.Zero,
            System.Numerics.Vector2.Zero,
            lightMaterial
        );

        // Assert
        heavyParticle.Mass.Should().Be(heavyMaterial.Mass);
        lightParticle.Mass.Should().Be(lightMaterial.Mass);
        heavyParticle.Mass.Should().BeGreaterThan(lightParticle.Mass);
    }
}
