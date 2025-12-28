using FluidGame.Core.Application.Interfaces;
using FluidGame.Core.Domain.Particles;
using FluidGame.Core.Domain.Physics;
using FluidGame.Infrastructure.Stride.Rendering;
using FluidGame.Infrastructure.Stride.Systems;
using Stride.Core;
using Stride.Engine;
using Stride.Games;

namespace FluidGame.Game;

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
        var gravity = new Vector2(0, 200f); // 200 pixels/sec² downward
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

        // Create root scene if needed
        if (_sceneSystem.SceneInstance == null)
        {
            _sceneSystem.SceneInstance = new SceneInstance(new Scene());
        }

        var rootScene = _sceneSystem.SceneInstance.RootScene;

        // Add physics update system (GameSystem)
        var physicsSystem = new ParticlePhysicsSystem(_services, physicsEngine, particleSystem);
        _gameSystems.Add(physicsSystem);

        // Create entity for rendering and monitoring (SyncScripts)
        var renderEntity = new Entity("ParticleRenderer");
        renderEntity.Add(new SimpleParticleRenderer(particleSystem));
        renderEntity.Add(new PerformanceMonitorSystem(particleSystem));
        rootScene.Entities.Add(renderEntity);
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
