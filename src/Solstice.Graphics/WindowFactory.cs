using System.Numerics;
using Solstice.Graphics.Implementations.RaylibImpl;

namespace Solstice.Graphics;

public record WindowSettings(
    string Title,
    bool VSyncEnabled,
    Vector2 Size,
    Vector2 Position,
    WindowState State,
    WindowStyle Style,
    Backend Backend)
{
    public static WindowSettings Default() => new(
        Title: "Solstice Window",
        VSyncEnabled: true,
        Size: new Vector2(800, 600),
        Position: new Vector2(-1, -1),
        State: WindowState.Normal,
        Style: WindowStyle.Default,
        Backend: Backend.Raylib
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
            _ => throw new NotSupportedException($"The {settings.Backend} rendering backend is not supported.")
        };
    }
}