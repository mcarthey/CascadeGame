using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Physics;
using Cascade.Infrastructure.Stride.Rendering;
using Stride.CommunityToolkit.Engine;
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

        // Create core services first
        var particleSystem = new ParticleSystem();
        Services.AddService<IParticleSystem>(particleSystem);

        var gravity = new Cascade.Core.Domain.Particles.Vector2(0, 200f);
        var bounds = new Bounds(0, 1280, 0, 720);
        var physicsEngine = new SimplePhysicsEngine(gravity, bounds);
        Services.AddService<IPhysicsEngine>(physicsEngine);

        // Initialize particles
        particleSystem.Initialize(10000, bounds);

        // Set up 2D graphics compositor with our particle renderer
        this.Add2DGraphicsCompositor();
        this.AddSceneRenderer(new ParticleSceneRenderer(particleSystem));
        this.Add2DCamera();

        // Bootstrap remaining game systems
        var bootstrapper = new GameBootstrapper(this, Services, GameSystems, SceneSystem);
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
