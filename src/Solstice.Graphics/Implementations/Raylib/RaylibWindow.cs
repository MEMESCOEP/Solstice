using System.ComponentModel;
using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Engine.Classes;
using Solstice.Graphics.Interfaces;
using Camera = Solstice.Common.Classes.Camera;
using Material = Hexa.NET.Raylib.Material;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Quaternion = System.Numerics.Quaternion;
using Scene = Assimp.Scene;

namespace Solstice.Graphics.Implementations;

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
    
    public IGraphics Graphics { get; }

    public RaylibWindow(WindowSettings settings)
    {
        // Initialize the window with the provided settings. The size setting only applies for non-fullscreen windows; Raylib sets the resolution to the current monitor's if
        // the window is configured for fullscreen mode
        Raylib.InitWindow((int)settings.Size.X, (int)settings.Size.Y, settings.Title);
        Time = new Time();

        // Apply the window settings
        // It's better to explicitly set the VSync enabled flag even though Raylib defaults to this mode. If there's some bug that causes this flag to not be setwhen the window
        // is created, this will still result in correct operation
        if (settings.VSyncEnabled == true)
        {
            Raylib.SetWindowState((uint)ConfigFlags.FlagVsyncHint);
        }
        else
        {
            Raylib.ClearWindowState((uint)ConfigFlags.FlagVsyncHint);
        }

        // Assume a position of (-1, -1) means "start in the center of the screen". This only applies if the window isn't in fullscreen mode; if we need to change the position
        // in this mode it's most likely just going to mean moving the window to (0, 0) on a certain monitor
        if (settings.State != WindowState.Fullscreen && settings.Position != Vector2.One * -1f)
        {
            Raylib.SetWindowPosition((int)settings.Position.X, (int)settings.Position.Y);
        }

        switch (settings.Style)
        {
            // FixedSize is the default here, so we don't need to handle that case separately
            case WindowStyle.Default:
            case WindowStyle.FixedSize:
                break;

            case WindowStyle.Borderless:
                Raylib.SetWindowState((uint)ConfigFlags.FlagBorderlessWindowedMode);
                break;

            case WindowStyle.Resizable:
                Raylib.SetWindowState((uint)ConfigFlags.FlagWindowResizable);
                break;

            default:
                throw new InvalidEnumArgumentException($"Window style \"{Style}\" is not valid.");
        }

        switch (settings.State)
        {
            case WindowState.Normal:
                break;

            case WindowState.Minimized:
                Raylib.SetWindowState((uint)ConfigFlags.FlagWindowMinimized);
                break;

            case WindowState.Maximized:
                Raylib.SetWindowState((uint)ConfigFlags.FlagWindowMaximized);
                break;

            case WindowState.Fullscreen:
                Raylib.SetWindowState((uint)ConfigFlags.FlagFullscreenMode);
                break;

            default:
                throw new InvalidEnumArgumentException($"Window state \"{State}\" is not valid.");
        }

        // Now we create an instance of RaylibGraphics, it'll store a list of cameras in the scene as well as the meshes
        Graphics = new RaylibGraphics();
        
        Input = new RaylibInput();
        
        // Disable the default ESC key to close the window, so we can handle it ourselves
        Raylib.SetExitKey(0);
    }
    
    public void Run()
    {
        while (Raylib.WindowShouldClose() == false)
        {
            OnUpdate?.Invoke(this);

            Graphics.Render();
            
            // Update the window's time related properties
            Time.DeltaTime = Raylib.GetFrameTime();
            Time.TotalTime += Time.DeltaTime;
            Time.FrameNumber++;
        }

        // Unload resources and close devices and the window to prevent memory leaks and device errors
        Close();
    }

    public void Close()
    {
        // Tell raylib to clean up any allocated / loaded resources
        //RLGraphics.UnloadData();

        // Close any audio devices and the window
        Raylib.CloseWindow();
    }
}