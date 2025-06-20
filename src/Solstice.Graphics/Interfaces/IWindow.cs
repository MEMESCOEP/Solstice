using System.Numerics;

namespace Solstice.Graphics.Interfaces;

public enum WindowState
{
    Normal,
    Minimized,
    Maximized,
    Fullscreen
}

public enum WindowStyle
{
    Default,
    Borderless,
    Resizable,
    FixedSize
}

public interface IWindow
{
    /// <summary>
    /// The title of the window.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// The size of the window in pixels.
    /// </summary>
    public Vector2 Size { get; set; }
    
    /// <summary>
    /// The position of the window on the screen in pixels. (from the origin (0,0) at the top-left corner of the most left screen)
    /// </summary>
    public Vector2 Position { get; set; }
    
    /// <summary>
    /// The state of the window. Changing this will change the window's behavior.
    /// </summary>
    public WindowState State { get; set; }
    
    /// <summary>
    /// The style of the window. This can affect the appearance and behavior of the window.
    /// </summary>
    public WindowStyle Style { get; set; }
    
    /// <summary>
    /// The input state. This contains information about the current input state, such as mouse position, keyboard state, etc.
    /// </summary>
    public IInput Input { get; }
    
    /// <summary>
    /// Time information. Information about the window's time, such as the current frame time, delta time, etc.
    /// </summary>
    public Time Time { get; }
    
    /// <summary>
    /// Called every frame before rendering. This is where you can update the window's state, handle input, etc.
    /// </summary>
    public Action<IWindow>? OnUpdate { get; set; }
    
    /// <summary>
    /// This is the function where IGraphics and IWindow are initialized. This is called once when the window is created.
    /// Do NOT call any graphics functions before this is called, as the graphics context may not be initialized yet.
    /// </summary>
    public Action<IWindow>? OnLoad { get; set; }
    
    /// <summary>
    /// After the window has been updated, this is called to render the window. This is where you can draw graphics to the window.
    /// </summary>
    public Action<IWindow>? OnRender { get; set; }
    
    /// <summary>
    /// The graphics interface. This is used to render graphics to the window.
    /// </summary>
    public IGraphics Graphics { get; }

    /// <summary>
    /// Blocking call. This will run the window's main loop until the window is closed or an exit condition is met.
    /// </summary>
    public void Run();

    /// <summary>
    /// Close the window. Also triggers Graphics cleanup.
    /// </summary>
    public void Close();
}