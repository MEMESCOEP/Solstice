using System.ComponentModel;
using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;

namespace Solstice.Graphics.Implementations.RaylibImpl;

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
        // Initialize the window with the provided settings, as well as any audio devices
        Raylib.InitWindow((int)settings.Size.X, (int)settings.Size.Y, settings.Title);
        Time = new Time();

        // Apply the window settings
        if (settings.VSyncEnabled == true)
        {
            Raylib.SetWindowState((uint)ConfigFlags.FlagVsyncHint);
        }
        else
        {
            Raylib.ClearWindowState((uint)ConfigFlags.FlagVsyncHint);
        }

        // Assume a position of (-1, -1) means "start in the center of the screen"
        if (settings.Position != Vector2.One * -1f)
        {
            Raylib.SetWindowPosition((int)settings.Position.X, (int)settings.Position.Y);
        }

        switch (settings.Style)
        {
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
    }
    
    public void Run()
    {
        while (Raylib.WindowShouldClose() == false)
        {
            // Update and draw the graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.Black);

            // Draw 3D objects
            // TODO: Add BeginMode3D and EndMode3D, as well as a check to see if a 3D camera exists in the scene

            // Draw 2D objects
            // TODO: implement 2D drawing and ImGui
            Raylib.EndDrawing();

            // Update the window's time related properties
            Time.DeltaTime = Raylib.GetFrameTime();
            Time.TotalTime += Time.DeltaTime;
            Time.FrameNumber++;

            // Finally, invoke the "OnUpdate" action if it is not null
            OnUpdate?.Invoke(this);
        }

        // Unload resources and close devices and the window to prevent memory leaks and device errors
        Close();
    }

    public void Close()
    {
        // TODO: tell the engine to clean up any allocated / loaded resources
        // Close any audio devices and the window
        Raylib.CloseWindow();
    }
}