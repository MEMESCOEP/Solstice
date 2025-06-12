using System.Numerics;

namespace Solstice.Graphics.Implementations.Raylib;

public class RaylibWindow : IWindow
{
    public string Title { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 Position { get; set; }
    public WindowState State { get; set; }
    public WindowStyle Style { get; set; }
    public IInput Input { get; }
    public Time Time { get; }
    public Action<IWindow>? OnUpdate { get; set; }

    public RaylibWindow(WindowSettings settings)
    {
        // TODO: Do stuff.
    }
    
    public void Run()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}