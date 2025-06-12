using System.Numerics;
using Solstice.Graphics.Implementations.Raylib;

namespace Solstice.Graphics;

public record WindowSettings(
    string Title,
    Vector2 Size,
    Vector2 Position,
    WindowState State,
    WindowStyle Style,
    Backend Backend)
{
    public static WindowSettings Default() => new(
        Title: "Solstice Window",
        Size: new Vector2(800, 600),
        Position: new Vector2(100, 100),
        State: WindowState.Normal,
        Style: WindowStyle.Default,
        Backend: Backend.OpenGL
    );
}



public static class WindowFactory
{
    public static IWindow CreateWindow(WindowSettings settings)
    {
        // Validate settings
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings), "Window settings cannot be null.");
        }

        // Create the window based on the backend
        return settings.Backend switch
        {
            Backend.Raylib => new RaylibWindow(settings),
            _ => throw new NotSupportedException($"Backend {settings.Backend} is not supported.")
        };
    }
}