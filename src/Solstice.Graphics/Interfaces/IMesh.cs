using System.Numerics;

namespace Solstice.Graphics.Interfaces;

/// <summary>
/// A struct containing gemoetry data; to be used with the IMesh interface
/// </summary>
public struct MeshData
{
    /// <summary>
    /// The number of vertices in the mesh
    /// </summary>
    int VertexCount;

    /// <summary>
    /// The number of triangles in the mesh
    /// </summary>
    int TriangleCount;

    /// <summary>
    /// A list of 3D vertex positions (XYZ - 3 components per vertex)
    /// </summary>
    public List<Vector3> Vertices;

    /// <summary>
    /// A list of 2D texture coordinates (UV - 2 components per vertex) (shader-location = 1)
    /// </summary>
    public List<Vector2> TexCoords;

    /// <summary>
    /// A list of second 2D texture coordinates (UV - 2 components per vertex) (shader-location = 5)
    /// </summary>
    public List<Vector2> TexCoords2;

    /// <summary>
    /// A list of the 3D surface normal unit vectors in a mesh, used for lighting (XYZ - 3 components per vertex)
    /// </summary>
    public List<Vector3> Normals;

    /// <summary>
    /// A list of the mesh's vertex tangents, often used (and required) for normal mapping (XYZW - 4 components per vertex)
    /// </summary>
    public List<Vector4> Tangents;

    /// <summary>
    /// A list of optional per-vertex colors in the RGBA format, useful for debugging
    /// </summary>
    public List<Vector4> Colors;

    /// <summary>
    /// A list of the mesh's vertex indices that describe how to form triangles in a indexed renderer
    /// </summary>
    public List<int> Indices;

    public MeshData()
    {
        VertexCount = 0;
        TriangleCount = 0;
        Vertices = new List<Vector3>();
        TexCoords = new List<Vector2>();
        TexCoords2 = new List<Vector2>();
        Normals = new List<Vector3>();
        Tangents = new List<Vector4>();
        Colors = new List<Vector4>();
        Indices = new List<int>();
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
    /// The shader to be used for rendering the mesh
    /// NOTE: This shader is used every frame, so it automatically updates. Calling UpdateMeshData is not required
    /// </summary>
    public IShader Shader { get; }

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