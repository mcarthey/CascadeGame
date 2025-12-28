using Stride.Engine;

namespace Cascade.Game;

/// <summary>
/// Entry point for the application.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        using (var game = new CascadeApp())
        {
            game.Run();
        }
    }
}
