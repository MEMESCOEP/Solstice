using Solstice.Common.Classes;
using Solstice.Engine.Classes;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Components;

public class CameraComponent : Component
{
    public Camera Camera;
    
    public override void Setup()
    {
        Camera = new Camera();
        Camera.Transform = Owner.Transform;
    }

    public override void Update(IWindow window)
    {
        Camera.Transform = Owner.Transform;
    }

    public override void Start()
    {
        
    }
}