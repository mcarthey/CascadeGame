using FluidGame.Core.Application.Interfaces;
using FluidGame.Infrastructure.Stride.Rendering;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Core.Mathematics;

namespace FluidGame.Infrastructure.Stride.Systems;

/// <summary>
/// Stride GameSystem that renders particles using vertex buffers.
/// High-performance rendering for thousands of particles.
/// </summary>
public class ParticleRenderSystem : GameSystem
{
    private readonly IParticleSystem _particleSystem;
    private readonly IGraphicsDeviceService _graphicsDeviceService;

    private GraphicsDevice? _graphicsDevice;
    private VertexBuffer? _vertexBuffer;
    private Effect? _effect;
    private VertexPositionColor[]? _vertices;
    private PipelineState? _pipelineState;

    private const int MaxParticles = 100000;

    public ParticleRenderSystem(IServiceRegistry services, IParticleSystem particleSystem)
        : base(services)
    {
        _particleSystem = particleSystem ?? throw new ArgumentNullException(nameof(particleSystem));
        _graphicsDeviceService = Services.GetService<IGraphicsDeviceService>()
            ?? throw new InvalidOperationException("Graphics device service not found");

        Enabled = true;
        DrawOrder = 1000; // Draw after most things
    }

    public override void Initialize()
    {
        base.Initialize();

        _graphicsDevice = _graphicsDeviceService.GraphicsDevice;
        if (_graphicsDevice == null)
            throw new InvalidOperationException("Graphics device not available");

        // Allocate vertex buffer for maximum particle count
        _vertices = new VertexPositionColor[MaxParticles];

        _vertexBuffer = VertexBuffer.New(
            _graphicsDevice,
            _vertices,
            GraphicsResourceUsage.Dynamic
        );

        // Load a simple effect for rendering colored points
        LoadEffect();
        CreatePipelineState();
    }

    private void LoadEffect()
    {
        // Create a simple shader effect
        // For now, we'll use Stride's built-in effect system
        var effectSystem = Services.GetService<EffectSystem>();
        if (effectSystem != null)
        {
            _effect = effectSystem.LoadEffect("ParticleEffect").WaitForResult();
        }

        // Fallback: create basic effect if custom shader isn't found
        if (_effect == null && _graphicsDevice != null)
        {
            _effect = new Effect(_graphicsDevice, EffectBytecode.FromByteCode(GetBasicShaderBytecode()));
        }
    }

    private void CreatePipelineState()
    {
        if (_graphicsDevice == null || _effect == null) return;

        var pipelineStateDescription = new PipelineStateDescription
        {
            BlendState = BlendStates.AlphaBlend,
            RasterizerState = RasterizerStateDescription.Default,
            DepthStencilState = DepthStencilStates.None,
            PrimitiveType = PrimitiveType.PointList,
            InputElements = VertexPositionColor.Layout.CreateInputElements(),
            EffectBytecode = _effect.Bytecode,
            RootSignature = _effect.RootSignature,
            Output = new RenderOutputDescription(PixelFormat.R8G8B8A8_UNorm)
        };

        _pipelineState = PipelineState.New(_graphicsDevice, ref pipelineStateDescription);
    }

    public override void Draw(RenderContext context)
    {
        if (!Enabled || _graphicsDevice == null || _vertexBuffer == null || _pipelineState == null)
            return;

        var particles = _particleSystem.Particles;
        if (particles.Count == 0) return;

        // Update vertex buffer with current particle positions
        UpdateVertexBuffer(particles);

        // Set up rendering state
        var commandList = context.CommandList;
        commandList.SetPipelineState(_pipelineState);
        commandList.SetVertexBuffer(0, _vertexBuffer, 0, VertexPositionColor.Size);

        // Draw particles as points
        commandList.Draw(particles.Count);
    }

    private void UpdateVertexBuffer(IReadOnlyList<Core.Domain.Particles.Particle> particles)
    {
        if (_vertices == null || _vertexBuffer == null || _graphicsDevice == null) return;

        int particleCount = Math.Min(particles.Count, MaxParticles);

        // Convert our domain particles to Stride vertex format
        for (int i = 0; i < particleCount; i++)
        {
            var particle = particles[i];
            _vertices[i] = new VertexPositionColor(
                TypeConverter.ToStrideVector3(particle.Position),
                TypeConverter.ToStrideColor(particle.Color)
            );
        }

        // Upload to GPU
        _vertexBuffer.SetData(_graphicsDevice, _vertices, 0, particleCount);
    }

    protected override void Destroy()
    {
        _vertexBuffer?.Dispose();
        _effect?.Dispose();
        _pipelineState?.Dispose();
        base.Destroy();
    }

    private byte[] GetBasicShaderBytecode()
    {
        // This is a placeholder - in a real implementation, we'd compile a proper shader
        // For now, we'll need to create a proper effect file
        return Array.Empty<byte>();
    }
}
