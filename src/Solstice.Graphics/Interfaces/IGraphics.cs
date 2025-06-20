using Solstice.Common.Classes;

namespace Solstice.Graphics.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IGraphics
{
    public List<IMesh> Meshes { get; }
    public List<Camera> Cameras { get; set; }
    
    public IMesh LoadMesh(string path);
    public void Render();
}