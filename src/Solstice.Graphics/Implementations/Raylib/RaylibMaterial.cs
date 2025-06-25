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

    public Material RLMaterial; // { get; private set; }

    public static IMaterial DefaultMaterial { get; } = CreateDefaultMaterial();

    private RaylibMaterial(IShader shader, List<MaterialProperty> properties, Material rlMaterial)
    {
        Shader = shader;
        Properties = properties;
        RLMaterial = rlMaterial;
    }

    public void UpdateProperties(List<MaterialProperty> NewProperties)
    {
        Properties.Clear();
        Properties.AddRange(NewProperties);
        
        Raylib.SetMaterialTexture(ref RLMaterial, (int)MaterialMapIndex.Albedo, (Texture)NewProperties[0].Value);
    }
    
    public static IMaterial CreateDefaultMaterial()
    {
        Texture checkerTexture = Raylib.LoadTextureFromImage(Raylib.GenImageChecked(60, 60, 2, 2, Raylib.Magenta, Raylib.Black));

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