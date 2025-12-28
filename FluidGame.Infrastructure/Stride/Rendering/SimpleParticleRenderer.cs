using FluidGame.Core.Application.Interfaces;
using Stride.Engine;
using Stride.Graphics;
using Stride.Core.Mathematics;
using Stride.Rendering;
using Stride.Rendering.Sprites;

namespace FluidGame.Infrastructure.Stride.Rendering;

/// <summary>
/// Simple particle renderer using SpriteBatch.
/// Good for POC - will upgrade to vertex buffers for better performance.
/// </summary>
public class SimpleParticleRenderer : GameSystem
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private Texture? _pixelTexture;
    private GraphicsDevice? _graphicsDevice;

    public SimpleParticleRenderer(IServiceRegistry services, IParticleSystem particleSystem)
        : base(services)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
        Enabled = true;
        DrawOrder = 100;
    }

    public override void Initialize()
    {
        base.Initialize();

        var graphicsDeviceService = Services.GetService<IGraphicsDeviceService>();
        _graphicsDevice = graphicsDeviceService?.GraphicsDevice
            ?? throw new InvalidOperationException("Graphics device not available");

        _spriteBatch = new SpriteBatch(_graphicsDevice);

        // Create a 1x1 white pixel texture for drawing particles
        _pixelTexture = Texture.New2D(_graphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        _pixelTexture.SetData(new[] { Color.White });
    }

    public override void Draw(RenderContext context)
    {
        if (!Enabled || _spriteBatch == null || _pixelTexture == null || _graphicsDevice == null)
            return;

        var particles = _particleSystem.Particles;
        if (particles.Count == 0) return;

        var commandList = context.CommandList;

        // Begin sprite batch
        _spriteBatch.Begin(commandList, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

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

    protected override void Destroy()
    {
        _pixelTexture?.Dispose();
        _spriteBatch?.Dispose();
        base.Destroy();
    }
}
