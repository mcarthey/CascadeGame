using Stride.Engine;
using Stride.Games;

namespace Cascade.Game;

/// <summary>
/// Main game application class.
/// Entry point for the Stride game engine.
/// </summary>
public class CascadeApp : Stride.Engine.Game
{
    protected override void BeginRun()
    {
        base.BeginRun();

        // Set window title
        Window.Title = "Cascade";

        // Set target resolution (720p)
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
