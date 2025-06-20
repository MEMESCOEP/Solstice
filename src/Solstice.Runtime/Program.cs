using System.Numerics;
using Solstice.Audio;
using Solstice.Audio.Enums;
using Solstice.Audio.Implementations;
using Solstice.Audio.Implementations.Generators;
using Solstice.Audio.Utilities;
using Solstice.Common;
using Solstice.Engine.Classes;
using Solstice.Engine.Components;
using Solstice.Graphics;
using Solstice.Graphics.Enums;

var scene = new Scene();

var window = WindowFactory.CreateWindow(WindowSettings.Default with
{
    Title = "Runtime",
    VSyncEnabled = true,
});

var audio = AudioFactory.CreateAudio(AudioSettings.Default); // Creates a thread.

// audio.MasterChannel.Sources.Add(new AudioSamplerGenerator(AudioLoaders.LoadFile("./song.mp3", true), channels: 2,
//     looping: true, amplitude: 0.5f));

GameObject camera = null!;
GameObject gameObject = null!;

window.OnLoad += (w) =>
{
    camera = scene.GetGameObject("Camera")!;
    
    gameObject = new GameObject("Mesh")
    {
        Components = { new MeshComponent(w.Graphics.LoadMesh("./dragon.obj")) }
    };
    
    scene.AddGameObject(gameObject);
};

window.OnUpdate += (w) =>
{
    scene.Update(w);

    Vector3 moveDir = Vector3.Zero;

    if (w.Input.IsKeyDown(KeyCode.W)) moveDir += camera.Transform.Forward;
    if (w.Input.IsKeyDown(KeyCode.S)) moveDir -= camera.Transform.Forward;
    if (w.Input.IsKeyDown(KeyCode.A)) moveDir += camera.Transform.Right;
    if (w.Input.IsKeyDown(KeyCode.D)) moveDir -= camera.Transform.Right;
    if (w.Input.IsKeyDown(KeyCode.Space)) moveDir += camera.Transform.Up;
    if (w.Input.IsKeyDown(KeyCode.LeftShift)) moveDir -= camera.Transform.Up;

    if (moveDir != Vector3.Zero)
        camera.Transform.Position += Vector3.Normalize(moveDir) * w.Time.DeltaTime;
    
    if (w.Input.IsMouseButtonDown(MouseButton.Right))
    {
        const float sensitivity = 0.1f;
        
        w.Input.SetMouseState(MouseState.Locked);
        var delta = w.Input.GetMouseDelta();
        camera.Transform.RotateByEuler(new Vector3(delta.Y * 0.005f * sensitivity, -delta.X * 0.005f * sensitivity, 0f));
    }
    else
    {
        w.Input.SetMouseState(MouseState.Normal);
    }
    
    if (w.Input.IsKeyDown(KeyCode.Escape))
    {
        w.Close();
    }
};

window.OnRender += (w) =>
{
    scene.Render(w.Graphics);
};

window.Run();
audio.Close();