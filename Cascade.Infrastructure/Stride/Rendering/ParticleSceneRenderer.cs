using Cascade.Core.Application.Interfaces;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using System;

namespace Cascade.Infrastructure.Stride.Rendering;

public class ParticleSceneRenderer : SceneRendererBase
{
    private readonly IParticleSystem _particleSystem;
    private SpriteBatch? _spriteBatch;
    private Texture? _particleTexture;
    private bool _textureInitialized;
    private const int TextureSize = 32;

    public ParticleSceneRenderer(IParticleSystem particleSystem)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
    }

    protected override void InitializeCore()
    {
        base.InitializeCore();
        var graphicsDevice = Context.GraphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice);

        _particleTexture = Texture.New2D(graphicsDevice, TextureSize, TextureSize,
            PixelFormat.R8G8B8A8_UNorm, TextureFlags.ShaderResource);
    }

    protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
    {
        if (_spriteBatch == null || _particleTexture == null) return;

        if (!_textureInitialized)
        {
            Color[] data = new Color[TextureSize * TextureSize];
            float center = (TextureSize - 1) / 2f;
            float maxRadius = TextureSize / 2f;

            for (int y = 0; y < TextureSize; y++)
            {
                for (int x = 0; x < TextureSize; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    float normalizedDist = Math.Clamp(distance / maxRadius, 0, 1);

                    // Softer edge for a blurred look
                    float alphaF = MathF.Pow(1.0f - normalizedDist, 3.0f);
                    byte alpha = (byte)(Math.Clamp(alphaF, 0f, 1f) * 255f);

                    // Premultiply RGB by alpha so it renders correctly with premultiplied blending
                    byte prem = (byte)((255 * alpha) / 255);

                    data[y * TextureSize + x] = new Color(prem, prem, prem, alpha);
                }
            }

            _particleTexture.SetData(drawContext.CommandList, data);
            _textureInitialized = true;
        }

        var particles = _particleSystem.Particles;

        // Use LinearClamp and premultiplied alpha blending (AlphaBlend expects premultiplied texels)
        _spriteBatch.Begin(drawContext.GraphicsContext,
            SpriteSortMode.Deferred,
            BlendStates.AlphaBlend,
            drawContext.GraphicsDevice.SamplerStates.LinearClamp,
            null,
            null,
            null);

        foreach (var particle in particles)
        {
            var position = TypeConverter.ToStrideVector2(particle.Position);
            var color = TypeConverter.ToStrideColor(particle.Color);

            float displaySize = 12f;
            float halfSize = displaySize / 2f;

            _spriteBatch.Draw(
                _particleTexture,
                new RectangleF(position.X - halfSize, position.Y - halfSize, displaySize, displaySize),
                color);
        }

        var floorColor = new Color(255, 0, 0, 255);
        _spriteBatch.Draw(
            _particleTexture,
            new RectangleF(0, 715, 1280, 5),
            floorColor);

        _spriteBatch.End();
    }

    protected override void Destroy()
    {
        _particleTexture?.Dispose();
        _spriteBatch?.Dispose();
        base.Destroy();
    }
}