using Cascade.Core.Application.Interfaces;
using Cascade.Infrastructure.Stride.Systems;
using Cascade.Infrastructure.Stride.Scripts;
using Stride.Core;
using Stride.Engine;
using Stride.Games;

namespace Cascade.Game;

/// <summary>
/// Bootstraps the game by registering dependencies and initializing systems.
/// This is where we wire up our clean architecture with Stride's service container.
/// </summary>
public class GameBootstrapper
{
    private readonly Stride.Engine.Game _game;
    private readonly IServiceRegistry _services;
    private readonly GameSystemCollection _gameSystems;
    private readonly SceneSystem _sceneSystem;

    public GameBootstrapper(Stride.Engine.Game game, IServiceRegistry services, GameSystemCollection gameSystems, SceneSystem sceneSystem)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _gameSystems = gameSystems ?? throw new ArgumentNullException(nameof(gameSystems));
        _sceneSystem = sceneSystem ?? throw new ArgumentNullException(nameof(sceneSystem));
    }

    /// <summary>
    /// Registers game systems (services should already be registered in CascadeApp).
    /// </summary>
    public void Bootstrap()
    {
        RegisterGameSystems();
    }

    private void RegisterGameSystems()
    {
        var particleSystem = _services.GetService<IParticleSystem>()
            ?? throw new InvalidOperationException("Particle system not registered");

        var physicsEngine = _services.GetService<IPhysicsEngine>()
            ?? throw new InvalidOperationException("Physics engine not registered");

        // Add physics update system (GameSystem)
        var physicsSystem = new ParticlePhysicsSystem(_services, physicsEngine, particleSystem);
        _gameSystems.Add(physicsSystem);

        // Note: ParticleSceneRenderer is added in CascadeApp via compositor chain
        // Note: Scene entities require a scene - skip for now if no scene exists
        if (_sceneSystem.SceneInstance?.RootScene != null)
        {
            var monitorEntity = new Entity("PerformanceMonitor");
            monitorEntity.Add(new PerformanceMonitorSystem(particleSystem));
            _sceneSystem.SceneInstance.RootScene.Entities.Add(monitorEntity);

            // Add material selection display (UI overlay)
            var displayEntity = new Entity("MaterialSelectionDisplay");
            displayEntity.Add(new MaterialSelectionDisplay());
            _sceneSystem.SceneInstance.RootScene.Entities.Add(displayEntity);

            // Add material spray controller for testing
            var sprayControllerEntity = new Entity("MaterialSprayController");
            sprayControllerEntity.Add(new MaterialSprayController());
            _sceneSystem.SceneInstance.RootScene.Entities.Add(sprayControllerEntity);
        }
    }
}
