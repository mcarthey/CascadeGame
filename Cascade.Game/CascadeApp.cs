using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Physics;
using Cascade.Infrastructure.Stride.Rendering;
using Stride.CommunityToolkit.Engine;
using Stride.Engine;
using Stride.Engine.Processors;
using Stride.Games;
using System.Numerics; // The new Core Vector2

namespace Cascade.Game;

public class CascadeApp : Stride.Engine.Game
{
    protected override void BeginRun()
    {
        base.BeginRun();

        Window.Title = "Cascade - SPH Fluid Simulation";

        // 1. Resolution Setup
        GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
        GraphicsDeviceManager.PreferredBackBufferHeight = 720;
        GraphicsDeviceManager.ApplyChanges();

        // 2. Initialize Core Services
        var particleSystem = new ParticleSystem();
        Services.AddService<IParticleSystem>(particleSystem);

        var particleEmitter = new ParticleEmitter(particleSystem);
        Services.AddService<IParticleEmitter>(particleEmitter);

        // 3. Setup Physics with System.Numerics.Vector2
        // Bounds: Leave margin from screen edges so particles are visible when stacked
        // In Stride's Y-down coordinate system: MinY=0 (top), MaxY=720 (bottom/floor)
        var gravity = new System.Numerics.Vector2(0, 400f);
        var bounds = new Bounds(10, 1270, 10, 700);  // 10px margin on all sides

        var physicsEngine = new SimplePhysicsEngine(gravity, bounds);
        Services.AddService<IPhysicsEngine>(physicsEngine);

        // 4. Initialize particles
        // Start with ZERO particles - use the spray gun to add materials interactively!
        // This lets you see each material type's unique behavior clearly
        particleSystem.Initialize(0, bounds);

        // 5. Setup Rendering Pipeline
        this.Add2DGraphicsCompositor();
        this.AddSceneRenderer(new ParticleSceneRenderer(particleSystem));
        this.Add2DCamera(); // Just add it normally; it defaults to Y-down.

        // 6. Bootstrap Systems (PhysicsSystem, etc.)
        var bootstrapper = new GameBootstrapper(this, Services, GameSystems, SceneSystem);
        bootstrapper.Bootstrap();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Input.IsKeyPressed(Stride.Input.Keys.Escape))
        {
            Exit();
        }
    }
}