using Stride.Engine;
using Stride.Games;

namespace FluidGame.Game;

/// <summary>
/// Main game application class.
/// Entry point for the Stride game engine.
/// </summary>
public class FluidGameApp : Stride.Engine.Game
{
    protected override void BeginRun()
    {
        base.BeginRun();

        // Set window title
        Window.Title = "Elemental Underground - Fluid Physics POC";

        // Set target resolution (720p for POC)
        GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
        GraphicsDeviceManager.PreferredBackBufferHeight = 720;
        GraphicsDeviceManager.ApplyChanges();

        // Bootstrap the game - register services and initialize systems
        var bootstrapper = new GameBootstrapper(Services, GameSystems, SceneSystem);
        bootstrapper.Bootstrap();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Exit on Escape key
        if (Input.IsKeyPressed(Stride.Input.Keys.Escape))
        {
            Exit();
        }
    }
}
