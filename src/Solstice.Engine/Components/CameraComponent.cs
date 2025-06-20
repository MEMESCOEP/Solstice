using Solstice.Common.Classes;
using Solstice.Engine.Classes;

namespace Solstice.Engine.Components;

public class CameraComponent : Component
{
    private Camera _camera;
    
    public CameraComponent()
    {
        _camera = new Camera();
    }
    
    public override void Setup()
    {
        
    }

    public override void Update(float DeltaTime)
    {
        _camera.Transform = Owner.Transform;
    }

    public override void Start()
    {
        
    }
}