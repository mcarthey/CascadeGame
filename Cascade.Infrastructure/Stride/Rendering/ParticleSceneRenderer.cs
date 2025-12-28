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
    private bool _initialized;

    public ParticleSceneRenderer(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
    }

    protected override void InitializeCore()
    {
        base.InitializeCore();

        var graphicsDevice = Context.GraphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice) { VirtualResolution = new Vector3(1280, 720, 1000) };

        // Create a 1x1 white pixel texture for drawing particles
        _pixelTexture = Texture.New2D(graphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);

        _initialized = true;
    }

    protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
    {
        if (!_initialized || _spriteBatch == null || _pixelTexture == null) return;

        // Initialize the pixel texture data on first draw (need command list)
        if (_pixelTexture.Width == 1)
        {
            try
            {
                _pixelTexture.SetData(drawContext.CommandList, new[] { Color.White });
            }
            catch
            {
                // Texture already has data
            }
        }

        var particles = _particleSystem.Particles;
        if (particles.Count == 0) return;

        // Begin sprite batch with the draw context's graphics context
        _spriteBatch.Begin(drawContext.GraphicsContext, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Draw each particle as a small square
        foreach (var particle in particles)
        {
            var position = TypeConverter.ToStrideVector2(particle.Position);
            var color = TypeConverter.ToStrideColor(particle.Color);

            // Draw a 4x4 pixel square for each particle (larger for visibility)
            _spriteBatch.Draw(_pixelTexture,
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
