using FluidGame.Core.Application.Interfaces;
using Stride.Engine;
using Stride.Graphics;
using Stride.Core.Mathematics;
using Stride.Rendering;

namespace FluidGame.Infrastructure.Stride.Systems;

/// <summary>
/// Monitors and displays performance metrics (FPS, particle count).
/// </summary>
public class PerformanceMonitorSystem : GameSystem
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _font;
    private GraphicsDevice? _graphicsDevice;

    private double _fpsTimer;
    private int _frameCount;
    private int _currentFps;

    public PerformanceMonitorSystem(IServiceRegistry services, IParticleSystem particleSystem)
        : base(services)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
        Enabled = true;
        DrawOrder = 10000; // Draw on top of everything
    }

    public override void Initialize()
    {
        base.Initialize();

        var graphicsDeviceService = Services.GetService<IGraphicsDeviceService>();
        _graphicsDevice = graphicsDeviceService?.GraphicsDevice
            ?? throw new InvalidOperationException("Graphics device not available");

        _spriteBatch = new SpriteBatch(_graphicsDevice);

        // Try to load a default font - if not available, we'll skip rendering
        try
        {
            var contentManager = Services.GetService<Stride.Core.Serialization.Contents.IContentManager>();
            _font = contentManager?.Load<SpriteFont>("StrideDefaultFont");
        }
        catch
        {
            // Font loading failed - we'll render without text
            _font = null;
        }
    }

    public override void Update(GameTime gameTime)
    {
        _frameCount++;
        _fpsTimer += gameTime.Elapsed.TotalSeconds;

        if (_fpsTimer >= 1.0)
        {
            _currentFps = _frameCount;
            _frameCount = 0;
            _fpsTimer = 0;
        }
    }

    public override void Draw(RenderContext context)
    {
        if (!Enabled || _spriteBatch == null || _graphicsDevice == null)
            return;

        var commandList = context.CommandList;

        _spriteBatch.Begin(commandList, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Draw background box
        DrawInfoBox(commandList);

        // Draw performance text if font is available
        if (_font != null)
        {
            DrawPerformanceText();
        }

        _spriteBatch.End();
    }

    private void DrawInfoBox(CommandList commandList)
    {
        // Create a semi-transparent background for the text
        var boxTexture = Texture.New2D(_graphicsDevice!, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        boxTexture.SetData(new[] { new Color(0, 0, 0, 180) });

        _spriteBatch!.Draw(boxTexture, new RectangleF(10, 10, 200, 80), Color.White);

        boxTexture.Dispose();
    }

    private void DrawPerformanceText()
    {
        if (_font == null) return;

        var text = $"FPS: {_currentFps}\n" +
                   $"Particles: {_particleSystem.Count:N0}\n" +
                   $"Target: 60 FPS";

        _spriteBatch!.DrawString(_font, text, new Vector2(20, 20), Color.White);
    }

    protected override void Destroy()
    {
        _spriteBatch?.Dispose();
        base.Destroy();
    }
}
