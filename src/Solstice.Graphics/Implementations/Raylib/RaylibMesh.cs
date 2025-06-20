using System.Runtime.InteropServices;
using Hexa.NET.Raylib;
using Solstice.Common;
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

    private uint[] IndicesArray;
    private float[] VertexArray;
    private float[] NormalArray;

    /// <summary>
    /// A function that updates mesh data
    /// </summary>
    public unsafe void UpdateMeshData(MeshData NewData)
    {
        RLMesh = new Mesh();
        MeshGeometryData = NewData;

        IndicesArray = NewData.Indices.Select(i => (uint)i).ToArray();
        VertexArray = NewData.Vertices.SelectMany(v => new float[] { v.X, v.Y, v.Z }).ToArray();

        fixed (uint* IndexPtr = IndicesArray)
        fixed (float* VertexPtr = VertexArray)
        {
            RLMesh.Indices = (ushort*)IndexPtr;
            RLMesh.Vertices = VertexPtr;
            RLMesh.TriangleCount = NewData.Indices.Count / 3;
            RLMesh.VertexCount = NewData.Vertices.Count;
        }

        if (NewData.Normals != null && NewData.Normals.Count == NewData.Vertices.Count)
        {
            NormalArray = NewData.Normals.SelectMany(n => new float[] { n.X, n.Y, n.Z }).ToArray();
            fixed (float* normalPtr = NormalArray)
            {
                RLMesh.Normals = normalPtr;
            }
        }

        Raylib.UploadMesh(ref RLMesh, false);
    }

    /// <summary>
    /// Unloads the mesh data from the GPU and frees C# memory
    /// </summary>
    public void Destroy()
    {
        Logger.Log(LogLevel.Info, "Destroying mesh");

        // Unload GPU mesh data
        if (RLMesh.VertexCount > 0)
        {
            Raylib.UnloadMesh(RLMesh);
        }
        
        Logger.Log(LogLevel.Info, "Fuckin DONE !!");
    }
}