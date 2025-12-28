using FluidGame.Core.Application.Interfaces;
using Stride.Core;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Core.Mathematics;
using Stride.Rendering.Sprites;

namespace FluidGame.Infrastructure.Stride.Systems;

/// <summary>
/// Monitors and displays performance metrics (FPS, particle count).
/// Uses SyncScript for rendering capabilities.
/// </summary>
public class PerformanceMonitorSystem : SyncScript
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _font;
    private Texture? _pixelTexture;

    private double _fpsTimer;
    private int _frameCount;
    private int _currentFps;

    public PerformanceMonitorSystem(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
    }

    public override void Start()
    {
        base.Start();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 pixel texture for drawing boxes
        _pixelTexture = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        _pixelTexture.SetData(new[] { Color.White });

        // Try to load a default font - if not available, we'll skip text rendering
        try
        {
            _font = Content.Load<SpriteFont>("StrideDefaultFont");
        }
        catch
        {
            // Font loading failed - we'll render without text
            _font = null;
        }
    }

    public override void Update()
    {
        _frameCount++;
        _fpsTimer += Game.UpdateTime.Elapsed.TotalSeconds;

        if (_fpsTimer >= 1.0)
        {
            _currentFps = _frameCount;
            _frameCount = 0;
            _fpsTimer = 0;
        }
    }

    public override void Draw()
    {
        if (_spriteBatch == null || _pixelTexture == null) return;

        _spriteBatch.Begin(GraphicsContext, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Draw semi-transparent background box
        _spriteBatch.Draw(_pixelTexture,
            new RectangleF(10, 10, 200, 80),
            new Color(0, 0, 0, 180));

        // Draw performance text if font is available
        if (_font != null)
        {
            var text = $"FPS: {_currentFps}\n" +
                       $"Particles: {_particleSystem.Count:N0}\n" +
                       $"Target: 60 FPS";

            _spriteBatch.DrawString(_font, text, new Vector2(20, 20), Color.White);
        }

        _spriteBatch.End();
    }

    protected override void Destroy()
    {
        _spriteBatch?.Dispose();
        _pixelTexture?.Dispose();
        base.Destroy();
    }
}
