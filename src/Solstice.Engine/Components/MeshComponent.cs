using Solstice.Engine.Classes;
using Solstice.Engine.Interfaces;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Components;

public class MeshComponent : Component, IRenderable
{
    public IMesh Mesh { get; set; }

    public MeshComponent(IMesh mesh)
    {
        Mesh = mesh;
    }

    public override void Setup()
    {
        
    }

    public override void Update(IWindow window)
    {
        Mesh.Transform = Owner.Transform;
    }

    public override void Start()
    {
        
    }

    public void Render(IGraphics graphics)
    {
        Mesh.Enabled = this.Enabled && Owner.Enabled;
    }
}