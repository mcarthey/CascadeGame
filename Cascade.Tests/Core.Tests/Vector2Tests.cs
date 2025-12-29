using System.Numerics;
using FluentAssertions;
using Xunit;

namespace Cascade.Tests.Core.Tests;

/// <summary>
/// Unit tests for Vector2 math operations using System.Numerics.
/// Verifies that our core math assumptions remain consistent after the library swap.
/// </summary>
public class Vector2Tests
{
    [Fact]
    public void Addition_ShouldCombineVectors()
    {
        // Arrange
        var v1 = new Vector2(3, 4);
        var v2 = new Vector2(1, 2);

        // Act
        var result = v1 + v2;

        // Assert
        result.X.Should().Be(4);
        result.Y.Should().Be(6);
    }

    [Fact]
    public void Subtraction_ShouldSubtractVectors()
    {
        // Arrange
        var v1 = new Vector2(5, 7);
        var v2 = new Vector2(2, 3);

        // Act
        var result = v1 - v2;

        // Assert
        result.X.Should().Be(3);
        result.Y.Should().Be(4);
    }

    [Fact]
    public void ScalarMultiplication_ShouldScaleVector()
    {
        // Arrange
        var v = new Vector2(3, 4);

        // Act
        var result = v * 2;

        // Assert
        result.X.Should().Be(6);
        result.Y.Should().Be(8);
    }

    [Fact]
    public void Length_ShouldCalculateCorrectly()
    {
        // Arrange
        var v = new Vector2(3, 4); // Classic 3-4-5 triangle

        // Act
        // Note: System.Numerics uses a method Length(), not a property
        var length = v.Length();

        // Assert
        length.Should().BeApproximately(5f, 0.0001f);
    }

    [Fact]
    public void Normalized_ShouldReturnUnitVector()
    {
        // Arrange
        var v = new Vector2(3, 4);

        // Act
        // Note: System.Numerics uses static Vector2.Normalize
        var normalized = Vector2.Normalize(v);

        // Assert
        normalized.Length().Should().BeApproximately(1f, 0.0001f);
    }

    [Fact]
    public void Distance_ShouldCalculateCorrectly()
    {
        // Arrange
        var v1 = new Vector2(0, 0);
        var v2 = new Vector2(3, 4);

        // Act
        var distance = Vector2.Distance(v1, v2);

        // Assert
        distance.Should().BeApproximately(5f, 0.0001f);
    }
}