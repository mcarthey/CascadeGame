using Stride.Engine;

namespace FluidGame.Game;

/// <summary>
/// Entry point for the application.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        using (var game = new FluidGameApp())
        {
            game.Run();
        }
    }
}
