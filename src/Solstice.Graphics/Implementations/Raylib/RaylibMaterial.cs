using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;

namespace Solstice.Graphics.Implementations;

public class MaterialProperty(string name, object value)
{
    public string Name { get; } = name;
    public object Value { get; } = value;
    public Type PropertyType { get; } = value.GetType();
}

public class RaylibMaterial : IMaterial
{
    public IShader Shader { get; }

    public List<MaterialProperty> Properties { get; }

    public Material RLMaterial { get; }

    public static IMaterial DefaultMaterial { get; } = CreateDefaultMaterial();

    private RaylibMaterial(IShader shader, List<MaterialProperty> properties, Material rlMaterial)
    {
        Shader = shader;
        Properties = properties;
        RLMaterial = rlMaterial;
    }
    
    public static IMaterial CreateDefaultMaterial()
    {
        Image checkerImage = Raylib.GenImageChecked(30, 30, 6, 6, Raylib.Magenta, Raylib.Black);
        Texture checkerTexture = Raylib.LoadTextureFromImage(checkerImage);

        Material mat = Raylib.LoadMaterialDefault();
        Raylib.SetMaterialTexture(ref mat, (int)MaterialMapIndex.Albedo, checkerTexture);

        IShader shader = new RaylibShader(mat.Shader);

        var props = new List<MaterialProperty>
        {
            new MaterialProperty("AlbedoTexture", checkerTexture),
            new MaterialProperty("Shader", shader)
        };

        return new RaylibMaterial(shader, props, mat);
    }
}