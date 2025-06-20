using Solstice.Graphics.Implementations;

namespace Solstice.Graphics.Interfaces;

public interface IMaterial
{
    /// <summary>
    /// The shader this material uses.
    /// </summary>
    public IShader Shader { get; }
    
    /// <summary>
    /// The properties of this material, which will be passed to the shader.
    /// </summary>
    public List<MaterialProperty> Properties { get; }

    /// <summary>
    /// The default material used by the engine.
    /// This might differ per graphics implementation, but it should always be a valid material that can be used for rendering.
    /// </summary>
    public static abstract IMaterial DefaultMaterial { get; }
}