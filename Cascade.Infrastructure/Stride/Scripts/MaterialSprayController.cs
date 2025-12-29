using Cascade.Core.Application.Interfaces;
using Cascade.Core.Domain.Particles;
using Cascade.Core.Domain.Particles.Materials;
using Cascade.Infrastructure.Stride.Rendering;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using System.Numerics;
using CoreVector2 = System.Numerics.Vector2;
using StrideVector2 = Stride.Core.Mathematics.Vector2;

namespace Cascade.Infrastructure.Stride.Scripts;

/// <summary>
/// Handles material selection and spray mechanics for testing.
///
/// Controls:
/// - Keys 1-8: Select material type
/// - Left Mouse Button: Spray selected material
/// - Right Mouse Button: Continuous spray
/// </summary>
public class MaterialSprayController : SyncScript
{
    private IParticleEmitter? _emitter;
    private MaterialType _selectedMaterial;
    private readonly Dictionary<Keys, MaterialType> _materialHotkeys;
    private int _currentMaterialIndex = 0;
    private readonly MaterialType[] _allMaterials;

    public MaterialSprayController()
    {
        _allMaterials = new MaterialType[]
        {
            new SnowMaterial(),
            new WaterMaterial(),
            new LavaMaterial(),
            new OilMaterial(),
            new MudMaterial(),
            new SandMaterial(),
            new SteamMaterial(),
            new IceMaterial()
        };

        _selectedMaterial = _allMaterials[0]; // Start with Snow

        _materialHotkeys = new Dictionary<Keys, MaterialType>
        {
            { Keys.D1, _allMaterials[0] }, // 1 = Snow
            { Keys.D2, _allMaterials[1] }, // 2 = Water
            { Keys.D3, _allMaterials[2] }, // 3 = Lava
            { Keys.D4, _allMaterials[3] }, // 4 = Oil
            { Keys.D5, _allMaterials[4] }, // 5 = Mud
            { Keys.D6, _allMaterials[5] }, // 6 = Sand
            { Keys.D7, _allMaterials[6] }, // 7 = Steam
            { Keys.D8, _allMaterials[7] }  // 8 = Ice
        };
    }

    public override void Start()
    {
        base.Start();

        // Get the emitter service
        _emitter = Services.GetService<IParticleEmitter>();
        if (_emitter == null)
        {
            throw new InvalidOperationException("IParticleEmitter service not registered");
        }

        System.Diagnostics.Debug.WriteLine($"[MaterialSprayController] Started - Selected material: {_selectedMaterial.Name}");
    }

    public override void Update()
    {
        if (_emitter == null) return;

        // Handle material selection
        HandleMaterialSelection();

        // Handle spraying
        HandleSpraying();
    }

    private void HandleMaterialSelection()
    {
        foreach (var kvp in _materialHotkeys)
        {
            if (Input.IsKeyPressed(kvp.Key))
            {
                _selectedMaterial = kvp.Value;
                _currentMaterialIndex = Array.IndexOf(_allMaterials, _selectedMaterial);
                System.Diagnostics.Debug.WriteLine($"[MaterialSprayController] Selected material: {_selectedMaterial.Name}");
            }
        }

        // Tab key to cycle through materials
        if (Input.IsKeyPressed(Keys.Tab))
        {
            _currentMaterialIndex = (_currentMaterialIndex + 1) % _allMaterials.Length;
            _selectedMaterial = _allMaterials[_currentMaterialIndex];
            System.Diagnostics.Debug.WriteLine($"[MaterialSprayController] Cycled to material: {_selectedMaterial.Name}");
        }
    }

    private void HandleSpraying()
    {
        if (_emitter == null) return;

        // Get mouse position in world space
        var mousePosition = Input.MousePosition;

        // Convert from normalized screen coordinates (0-1) to world coordinates
        // Assuming a 1280x720 coordinate system matching the simulation bounds
        var worldX = mousePosition.X * 1280f;
        var worldY = mousePosition.Y * 720f;
        var sprayPosition = new CoreVector2(worldX, worldY);

        // Left mouse button: Single burst
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _emitter.EmitSpray(sprayPosition, _selectedMaterial, count: 10, velocity: 150f);
            System.Diagnostics.Debug.WriteLine($"[MaterialSprayController] Sprayed {_selectedMaterial.Name} at ({worldX:F0}, {worldY:F0})");
        }

        // Right mouse button: Continuous spray (lower particle count per frame)
        if (Input.IsMouseButtonDown(MouseButton.Right))
        {
            _emitter.EmitSpray(sprayPosition, _selectedMaterial, count: 3, velocity: 120f);
        }
    }
}
