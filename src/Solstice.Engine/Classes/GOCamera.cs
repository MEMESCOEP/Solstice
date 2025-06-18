using System.ComponentModel;
using Solstice.Engine.Classes;
using Solstice.Common.Classes;

namespace Solstice.Engine.Classes;

public class GOCamera : GameObject
{
    /// <summary>
    /// The backend-agnostic instance of the Camera class
    /// </summary>
    public Camera CameraInstance { get; private set; }

    public override void Setup()
    {
        
    }

    public override void Start()
    {
        
    }

    public override void Update(float DeltaTime)
    {
        Console.WriteLine($"Hello from {this.Name}!");
        
        foreach (GOComponent Component in Components)
        {
            //Component.Update(DeltaTime);
        }
    }
}