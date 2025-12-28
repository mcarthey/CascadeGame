using Cascade.Core.Application.Interfaces;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace Cascade.Infrastructure.Stride.Rendering;

/// <summary>
/// Custom scene renderer that draws particles using SpriteBatch.
/// This integrates properly with Stride's graphics compositor pipeline.
/// </summary>
public class ParticleSceneRenderer : SceneRendererBase
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private Texture? _pixelTexture;
    private bool _textureInitialized;

    public ParticleSceneRenderer(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
    }

    protected override void InitializeCore()
    {
        base.InitializeCore();

        var graphicsDevice = Context.GraphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice);

        // Create a 1x1 white pixel texture for drawing particles
        _pixelTexture = Texture.New2D(graphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm, TextureFlags.ShaderResource);
    }

    protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
    {
        if (_spriteBatch == null || _pixelTexture == null) return;

        // Initialize the pixel texture data on first draw (need command list)
        if (!_textureInitialized)
        {
            _pixelTexture.SetData(drawContext.CommandList, new[] { Color.White });
            _textureInitialized = true;
        }

        var particles = _particleSystem.Particles;
        if (particles.Count == 0) return;

        // Begin sprite batch - draw directly to screen
        _spriteBatch.Begin(drawContext.GraphicsContext, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Debug: Draw a big white rectangle to confirm rendering works
        _spriteBatch.Draw(_pixelTexture, new RectangleF(100, 100, 200, 200), Color.White);

        // Draw each particle as a small square
        foreach (var particle in particles)
        {
            var position = TypeConverter.ToStrideVector2(particle.Position);
            var color = TypeConverter.ToStrideColor(particle.Color);

            // Draw a 4x4 pixel square for each particle
            _spriteBatch.Draw(
                _pixelTexture,
                new RectangleF(position.X - 2, position.Y - 2, 4, 4),
                color);
        }

        _spriteBatch.End();
    }

    protected override void Destroy()
    {
        _pixelTexture?.Dispose();
        _spriteBatch?.Dispose();
        base.Destroy();
    }
}
