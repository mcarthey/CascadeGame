using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering.Sprites;

namespace Cascade.Infrastructure.Stride.Scripts;

/// <summary>
/// Displays the currently selected material type on screen.
/// Shows material name and color as visual feedback using SpriteBatch rendering.
/// </summary>
public class MaterialSelectionDisplay : SyncScript
{
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _font;
    private Texture? _pixelTexture;
    private MaterialType _currentMaterial;

    public MaterialSelectionDisplay()
    {
        _currentMaterial = new SnowMaterial(); // Default
    }

    public override void Start()
    {
        base.Start();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 pixel texture for drawing boxes
        _pixelTexture = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        _pixelTexture.SetData(Game.GraphicsContext.CommandList, new[] { Color.White });

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

    /// <summary>
    /// Updates the display to show the selected material.
    /// Called by MaterialSprayController when material changes.
    /// </summary>
    public void UpdateMaterial(MaterialType material)
    {
        _currentMaterial = material;
    }

    public override void Update()
    {
        if (_spriteBatch == null || _pixelTexture == null) return;

        _spriteBatch.Begin(Game.GraphicsContext, SpriteSortMode.Deferred, BlendStates.AlphaBlend);

        // Draw material indicator box (top-left)
        var materialColor = _currentMaterial.BaseColor;
        var strideColor = new Color(materialColor.R, materialColor.G, materialColor.B, 255);

        // Background for material display
        _spriteBatch.Draw(_pixelTexture,
            new RectangleF(220, 10, 250, 60),
            new Color(0, 0, 0, 180));

        // Material color indicator
        _spriteBatch.Draw(_pixelTexture,
            new RectangleF(230, 20, 40, 40),
            strideColor);

        // Draw material name if font is available
        if (_font != null)
        {
            var materialText = $"Material: {_currentMaterial.Name}";
            _spriteBatch.DrawString(_font, materialText, new Vector2(280, 30), Color.White, 0, Vector2.Zero, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0);

            // Draw controls help at bottom
            var controlsText = "1-8: Select Material | TAB: Cycle | LMB: Spray | RMB: Stream";
            _spriteBatch.DrawString(_font, controlsText, new Vector2(20, 690), new Color(200, 200, 200, 200), 0, Vector2.Zero, new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);
        }

        _spriteBatch.End();
    }

    public override void Cancel()
    {
        _spriteBatch?.Dispose();
        _pixelTexture?.Dispose();
        base.Cancel();
    }
}
