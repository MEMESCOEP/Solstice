namespace Solstice.Graphics.Interfaces;

public class ShaderProperty
{
    public ShaderProperty(string Name, object Value)
    {
        this.Name = Name;
        this.Value = Value;
        this.PropertyType = Value.GetType();
    }

    public string Name { get; }
    public object Value { get; }
    public Type PropertyType { get; }
}

/// <summary>
/// A shader is a GPU program that is used to <INSERT PROPER DEFINITION>
/// </summary>
public interface IShader
{
    public List<ShaderProperty> ShaderProperties { get; }

    internal void Use();
}