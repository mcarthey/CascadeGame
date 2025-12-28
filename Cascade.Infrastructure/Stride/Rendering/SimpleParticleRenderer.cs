using Cascade.Core.Application.Interfaces;
using Stride.Engine;
using Stride.Graphics;
using Stride.Core.Mathematics;
using Stride.Rendering.Sprites;

namespace Cascade.Infrastructure.Stride.Rendering;

/// <summary>
/// Simple particle renderer using SpriteBatch.
/// Good for POC - will upgrade to vertex buffers for better performance.
/// Uses SyncScript for rendering capabilities.
/// </summary>
public class SimpleParticleRenderer : SyncScript
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private Texture? _pixelTexture;

    public SimpleParticleRenderer(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
    }

    public override void Start()
    {
        base.Start();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 white pixel texture for drawing particles
        _pixelTexture = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        _pixelTexture.SetData(new[] { Color.White });
    }

    public override void Update()
    {
        // Rendering happens here in SyncScript's Update method
        if (_spriteBatch == null || _pixelTexture == null) return;

        var particles = _particleSystem.Particles;
        if (particles.Count == 0) return;

        // Begin sprite batch
        _spriteBatch.Begin(GraphicsContext, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Draw each particle as a small square
        foreach (var particle in particles)
        {
            var position = TypeConverter.ToStrideVector2(particle.Position);
            var color = TypeConverter.ToStrideColor(particle.Color);

            // Draw a 2x2 pixel square for each particle
            _spriteBatch.Draw(_pixelTexture,
                new RectangleF(position.X - 1, position.Y - 1, 2, 2),
                color);
        }

        _spriteBatch.End();
    }

    public override void Cancel()
    {
        _pixelTexture?.Dispose();
        _spriteBatch?.Dispose();
        base.Cancel();
    }
}
