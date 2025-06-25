using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Audio;
using Solstice.Audio.Implementations;
using Solstice.Audio.Implementations.Generators;
using Solstice.Audio.Utilities;
using Solstice.Engine.Classes;
using Solstice.Engine.Components;
using Solstice.Graphics;
using Solstice.Graphics.Enums;
using Solstice.Graphics.Implementations;
using MouseButton = Solstice.Graphics.Enums.MouseButton;

var scene = new Scene();

var window = WindowFactory.CreateWindow(WindowSettings.Default with
{
    Title = "Runtime",
    VSyncEnabled = false,
});

var audio = AudioFactory.CreateAudio(AudioSettings.Default); // Creates a thread.
var channel = new AudioChannel();
audio.AudioChannels.Add(channel);
scene.InitAudio(audio);

GameObject camera = null!;
GameObject gameObject = null!;
GameObject gameObject2 = null!;
GameObject gameObject3 = null!;
GameObject gameObject4 = null!;

window.OnLoad += (w) =>
{
    Console.WriteLine(Directory.GetCurrentDirectory());
    camera = scene.GetGameObject("Camera")!;
    
    gameObject = new GameObject("Suzzane")
    {
        Components =
        {
            new MeshComponent(w.Graphics.LoadMesh("../../../TestAssets/Suzzane.obj")),
        }
    };
    
    gameObject2 = new GameObject("Stanford Dragon")
    {
        Components =
        {
            new MeshComponent(w.Graphics.LoadMesh("../../../TestAssets/Dragon2.obj")), 
            new AudioSourceComponent(channel, new AudioSamplerGenerator(AudioLoaders.LoadFile("../../../TestAssets/FUNKY_TOWN_LQ.wav"), looping:true, isPlaying:true))
        }
    };
    
    gameObject3 = new GameObject("BoomBox")
    {
        Components =
        {
            new MeshComponent(w.Graphics.LoadMesh("../../../TestAssets/BoomBox.glb"))
        }
    };

    gameObject4 = new GameObject("Fisj")
    {
        Components =
        {
            new MeshComponent(w.Graphics.LoadMesh("../../../TestAssets/Fish.glb"))
        }
    };
    
    gameObject.Transform.Position = new Vector3(1.25f, 0f, 0f);
    gameObject.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
    gameObject2.Transform.Position = new Vector3(0f, 0f, 1.25f);
    gameObject3.Transform.Scale = new Vector3(50f, 50f, 50f);
    gameObject3.Transform.RotateToEuler(new Vector3(float.DegreesToRadians(45f), 0f, 0f));
    gameObject4.Transform.Position = new Vector3(0f, 1.25f, 0f);
    
    scene.AddGameObject(gameObject);
    scene.AddGameObject(gameObject2);
    scene.AddGameObject(gameObject3);
    scene.AddGameObject(gameObject4);
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
    
    gameObject.Transform.RotateByEuler(Vector3.UnitY * float.DegreesToRadians(25f) * w.Time.DeltaTime);
    gameObject2.Transform.RotateByEuler(Vector3.UnitY * float.DegreesToRadians(90f) * w.Time.DeltaTime);
    gameObject3.Transform.RotateByEuler(Vector3.UnitX * float.DegreesToRadians(10f) * w.Time.DeltaTime);
};

window.OnRender += (w) =>
{
    scene.Render(w.Graphics);
    Raylib.DrawText($"Tris: {((RaylibGraphics)w.Graphics).GetTriangleCount()}", 0, 20, 20, Raylib.White);
};

window.Run();
audio.Close();