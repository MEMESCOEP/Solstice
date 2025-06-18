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
    private RaylibGraphics RLGraphics;

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
        RLGraphics = new();
    }
    
    public void Run()
    {
        Engine.Classes.Scene NewScene = new();
        Camera NewCam = new Camera();
        RLGraphics.Cameras.Add(NewCam);

        RLGraphics.LoadMesh("./dragon.obj");

        Material NewMat = Raylib.LoadMaterialDefault();
        Raylib.SetMaterialTexture(ref NewMat, (int)MaterialMapIndex.Albedo, Raylib.LoadTextureFromImage(Raylib.GenImageChecked(30, 30, 6, 6, Raylib.Magenta, Raylib.Black)));

        Raylib.DisableCursor();
        while (Raylib.WindowShouldClose() == false)
        {
            // Update all gameobjects and their components
            foreach (GameObject GO in NewScene.GameObjects)
            {
                GO.Update(0f);
            }
            
            // Update and draw the graphics
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.Black);

            // Draw 3D objects
            foreach (Camera Cam in RLGraphics.Cameras)
            {
                foreach (RaylibMesh Mesh in RLGraphics.Meshes.Cast<RaylibMesh>())
                {
                    RLGraphics.UpdateRLCam(Cam);
                    Raylib.BeginMode3D(RLGraphics.RLCamera);
                    Raylib.DrawSphere(Vector3.UnitZ * 5f, 0.5f, Raylib.Red);
                    Raylib.DrawSphere(Vector3.UnitZ * -5f, 0.5f, Raylib.Blue);
                    Raylib.DrawSphere(Vector3.UnitX * 5f, 0.5f, Raylib.Green);
                    Raylib.DrawSphere(Vector3.UnitX * -5f, 0.5f, Raylib.Magenta);
                    Raylib.DrawMesh(Mesh.RLMesh, NewMat, Matrix4x4.Identity);
                    Raylib.EndMode3D();
                }
            }

            Vector2 MouseDelta = Raylib.GetMouseDelta();

            NewCam.Transform.RotateByEuler(new Vector3(MouseDelta.Y * 0.0005f, -MouseDelta.X * 0.0005f, 0f));

            if (Raylib.IsKeyDown((int)KeyboardKey.W))
            {
                NewCam.Transform.Position += Vector3.Transform(Vector3.UnitZ, NewCam.Transform.Rotation) * 5f * Raylib.GetFrameTime();
            }

            if (Raylib.IsKeyDown((int)KeyboardKey.S))
            {
                NewCam.Transform.Position += Vector3.Transform(Vector3.UnitZ, NewCam.Transform.Rotation) * -5f * Raylib.GetFrameTime();
            }

            // Draw 2D objects
            // TODO: implement 2D drawing and ImGui
            Raylib.DrawFPS(0, 0);
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
        // Tell raylib to clean up any allocated / loaded resources
        //RLGraphics.UnloadData();

        // Close any audio devices and the window
        Raylib.CloseWindow();
    }

    public static Vector3 QuaternionToEuler(Quaternion q)
    {
        // Pitch (X axis rotation)
        float sinp = 2.0f * (q.W * q.X + q.Y * q.Z);
        float cosp = 1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
        float pitch = MathF.Atan2(sinp, cosp);

        // Yaw (Y axis rotation)
        float siny = 2.0f * (q.W * q.Y - q.Z * q.X);
        float yaw;
        if (MathF.Abs(siny) >= 1f)
            yaw = MathF.CopySign(MathF.PI / 2f, siny); // use 90 degrees if out of range
        else
            yaw = MathF.Asin(siny);

        // Roll (Z axis rotation)
        float sinr = 2.0f * (q.W * q.Z + q.X * q.Y);
        float cosr = 1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z);
        float roll = MathF.Atan2(sinr, cosr);

        return new Vector3(pitch, yaw, roll); // in radians
    }

}