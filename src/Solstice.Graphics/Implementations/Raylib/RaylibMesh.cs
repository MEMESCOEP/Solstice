using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;
using Transform = Solstice.Common.Classes.Transform;

namespace Solstice.Graphics.Implementations;

public class RaylibMesh : IMesh
{
    /// <summary>
    /// A struct containing geometry data that makes up a mesh
    /// </summary>
    public MeshData MeshGeometryData { get; private set; }

    public Mesh RLMesh;

    /// <summary>
    /// The shader to be used for rendering the mesh
    /// NOTE: This shader is used every frame, so it automatically updates. Calling UpdateMeshData is not required
    /// </summary>
    public IMaterial Material { get; set; }

    public Transform Transform { get; set; } = new Transform();
    public bool Enabled { get; set; } = false;
    
    private uint[] IndicesArray;
    private float[] VertexArray;
    private float[] NormalArray;
    private float[] TexCoordArray;

    /// <summary>
    /// A function that sets this mesh's data to the passed in data and uploads this new data to the GPU
    /// </summary>
    public unsafe void UpdateMeshData(MeshData NewData)
    {
        RLMesh = new Mesh();
        MeshGeometryData = NewData;

        // Prepare arrays for Raylib upload
        ushort[]? IndicesArray = NewData.Indices != null ? NewData.Indices.Select(i => (ushort)i).ToArray() : null;

        float[] VertexArray = NewData.Vertices.SelectMany(v => new float[] { v.X, v.Y, v.Z }).ToArray();

        float[]? TexCoordArray = (NewData.TexCoords != null && NewData.TexCoords.Length == NewData.Vertices.Length)
            ? NewData.TexCoords.SelectMany(uv => new float[] { uv.X, uv.Y }).ToArray()
            : null;

        float[]? NormalArray = (NewData.Normals != null && NewData.Normals.Length == NewData.Vertices.Length)
            ? NewData.Normals.SelectMany(n => new float[] { n.X, n.Y, n.Z }).ToArray()
            : null;

        // Assign arrays as pointers — these pointers are valid only during this call
        fixed (float* vertexPtr = VertexArray)
        fixed (float* texPtr = TexCoordArray)
        fixed (float* normalPtr = NormalArray)
        {
            RLMesh.Vertices = vertexPtr;
            RLMesh.Texcoords = texPtr;
            RLMesh.Normals = normalPtr;
            RLMesh.VertexCount = NewData.VertexCount;
            RLMesh.TriangleCount = NewData.TriangleCount;

            if (IndicesArray != null)
            {
                fixed (ushort* indexPtr = IndicesArray)
                {
                    RLMesh.Indices = indexPtr;
                }
            }
            else
            {
                RLMesh.Indices = null;
            }

            Raylib.UploadMesh(ref RLMesh, false);
        }

        // Null out pointers so RLMesh no longer references managed memory
        RLMesh.Vertices = null;
        RLMesh.Texcoords = null;
        RLMesh.Normals = null;
        RLMesh.Indices = null;
    }
    
    public void SetMaterial(IMaterial NewMaterial)
    {
        Material = NewMaterial;
    }

    /// <summary>
    /// Unloads the mesh data from the GPU and frees C# memory
    /// </summary>
    public void Destroy()
    {
        if (RLMesh.VertexCount > 0)
            Raylib.UnloadMesh(RLMesh);
    }
}