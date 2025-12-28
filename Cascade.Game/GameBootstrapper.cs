using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Physics;
using Cascade.Infrastructure.Stride.Rendering;
using Cascade.Infrastructure.Stride.Systems;
using Stride.Core;
using Stride.Engine;
using Stride.Games;
using Stride.Rendering.Compositing;

namespace Cascade.Game;

/// <summary>
/// Bootstraps the game by registering dependencies and initializing systems.
/// This is where we wire up our clean architecture with Stride's service container.
/// </summary>
public class GameBootstrapper
{
    private readonly IServiceRegistry _services;
    private readonly GameSystemCollection _gameSystems;
    private readonly SceneSystem _sceneSystem;

    public GameBootstrapper(IServiceRegistry services, GameSystemCollection gameSystems, SceneSystem sceneSystem)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _gameSystems = gameSystems ?? throw new ArgumentNullException(nameof(gameSystems));
        _sceneSystem = sceneSystem ?? throw new ArgumentNullException(nameof(sceneSystem));
    }

    /// <summary>
    /// Registers all services and initializes the game systems.
    /// </summary>
    public void Bootstrap()
    {
        // Register core domain services
        RegisterCoreServices();

        // Register and initialize game systems
        RegisterGameSystems();

        // Initialize particle system with 10,000 particles
        InitializeParticles();
    }

    private void RegisterCoreServices()
    {
        // Create particle system
        var particleSystem = new ParticleSystem();
        _services.AddService<IParticleSystem>(particleSystem);

        // Create physics engine with gravity
        var gravity = new Cascade.Core.Domain.Particles.Vector2(0, 200f); // 200 pixels/sec² downward
        var bounds = new Bounds(0, 1280, 0, 720); // 720p resolution
        var physicsEngine = new SimplePhysicsEngine(gravity, bounds);
        _services.AddService<IPhysicsEngine>(physicsEngine);
    }

    private void RegisterGameSystems()
    {
        var particleSystem = _services.GetService<IParticleSystem>()
            ?? throw new InvalidOperationException("Particle system not registered");

        var physicsEngine = _services.GetService<IPhysicsEngine>()
            ?? throw new InvalidOperationException("Physics engine not registered");

        // Get the root scene (created by SetupBase2D in CascadeApp)
        var rootScene = _sceneSystem.SceneInstance?.RootScene
            ?? throw new InvalidOperationException("Scene not initialized. Call SetupBase2D first.");

        // Add physics update system (GameSystem)
        var physicsSystem = new ParticlePhysicsSystem(_services, physicsEngine, particleSystem);
        _gameSystems.Add(physicsSystem);

        // Add particle renderer to the graphics compositor
        var compositor = _sceneSystem.GraphicsCompositor;
        if (compositor?.Game is SceneRendererCollection renderers)
        {
            renderers.Add(new ParticleSceneRenderer(particleSystem));
        }

        // Create entity for performance monitoring (SyncScript)
        var monitorEntity = new Entity("PerformanceMonitor");
        monitorEntity.Add(new PerformanceMonitorSystem(particleSystem));
        rootScene.Entities.Add(monitorEntity);
    }

    private void InitializeParticles()
    {
        var particleSystem = _services.GetService<IParticleSystem>()
            ?? throw new InvalidOperationException("Particle system not registered");

        var physicsEngine = _services.GetService<IPhysicsEngine>()
            ?? throw new InvalidOperationException("Physics engine not registered");

        // Initialize with 10,000 particles
        particleSystem.Initialize(10000, physicsEngine.SimulationBounds);
    }
}
