using System.Numerics;
using Solstice.Common.Classes;

namespace Solstice.Graphics.Interfaces;

/// <summary>
/// A struct containing gemoetry data; to be used with the IMesh interface
/// </summary>
public class MeshData
{
    public int VertexCount;
    public int TriangleCount;

    public Vector3[] Vertices;
    public Vector2[]? TexCoords;
    public Vector2[]? TexCoords2;
    public Vector3[]? Normals;
    public Vector4[]? Tangents;
    public Vector4[]? Colors;
    public int[]? Indices;

    public MeshData(int vertexCount, int triangleCount, bool IsIndexed)
    {
        VertexCount = vertexCount;
        TriangleCount = triangleCount;
        
        Vertices = new Vector3[vertexCount];
        TexCoords = new Vector2[vertexCount];
        //TexCoords2 = new Vector2[vertexCount];
        Normals = new Vector3[vertexCount];
        //Tangents = new Vector4[vertexCount];
        //Colors = new Vector4[vertexCount];

        if (IsIndexed == true)
        {
            Indices = new int[triangleCount * 3];
        }
        else
        {
            Indices = null;
        }
    }
}

/// <summary>
/// A cross-backend mesh interface based on Raylib's Mesh struct. Backend-specific mesh types will inherit from this interface
/// NOTE: Don't use this interface directly, instead use IGraphics.GenerateMesh to create a mesh that works on the current backend. Also be aware that
/// meshes do not automatically update if their properties are changed, so you'll need to call UpdateMeshData after modifying, or passing in a new MeshData struct
/// </summary>
public interface IMesh
{
    /// <summary>
    /// A struct containing geometry data that makes up a mesh
    /// </summary>
    public MeshData MeshGeometryData { get; }

    /// <summary>
    /// The material to be used for rendering the mesh
    /// </summary>
    public IMaterial Material { get; set; }
    
    /// <summary>
    /// The transform of the mesh in the world space
    /// </summary>
    public Transform Transform { get; set; }

    /// <summary>
    /// Wether the mesh is enabled or not. If false, the mesh will not be rendered
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// A function that updates mesh data
    /// NOTE: This needs to be implemented individually in each backend mesh class
    /// </summary>
    public void UpdateMeshData(MeshData NewData);

    /// <summary>
    /// A function that destroys and unloads the mesh data from the GPU
    /// NOTE: This needs to be implemented individually in each backend mesh class
    /// </summary>
    public void Destroy();
}