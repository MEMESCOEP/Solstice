using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;
using Assimp;
using ZLinq;
using Camera = Solstice.Common.Classes.Camera;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Mesh = Assimp.Mesh;

namespace Solstice.Graphics.Implementations;

public class RaylibGraphics : IGraphics
{
    public List<IMesh> Meshes { get; }
    public List<Camera> Cameras { get; set; }
    public Camera3D RLCamera;
    
    private long TriangleCount = 0;
    private AssimpContext Loader;

    public RaylibGraphics()
    {
        Loader = new AssimpContext();
        Meshes = new List<IMesh>();
        Cameras = new List<Camera>();
        RLCamera = new Camera3D();
        RLCamera.Up = Vector3.UnitY;
    }
    
    public long GetTriangleCount() => TriangleCount;

    public void UpdateRLCam(Camera CurrentCamera)
    {
        RLCamera.Projection = (int)CurrentCamera.Projection;
        RLCamera.Position = CurrentCamera.Transform.Position;
        RLCamera.Fovy = CurrentCamera.FOV;

        // Convert the camera's rotation quaternion and to a point on the unit sphere. We use the Z-axis as the default forward direction
        var Forward = Vector3.Transform(Vector3.UnitZ, CurrentCamera.Transform.Rotation);
        var Up = Vector3.Transform(Vector3.UnitY, CurrentCamera.Transform.Rotation);

        if (Forward.LengthSquared() < 0.0001f)
            Forward = Vector3.UnitZ;  // fallback safe forward

        if (Up.LengthSquared() < 0.0001f)
            Up = Vector3.UnitY;  // fallback safe forward

        RLCamera.Target = RLCamera.Position + Forward;
        //RLCamera.Target = RLCamera.Position + Vector3.Transform(Vector3.UnitZ, CurrentCamera.Transform.Rotation);
        RLCamera.Up = Up;
    }

    public IMesh? LoadMesh(string filepath)
    {
        // Load the scene and check if it has any meshes
        Scene NewScene = Loader.ImportFile(filepath,
            PostProcessSteps.Triangulate |
            PostProcessSteps.CalculateTangentSpace | PostProcessSteps.FlipUVs | PostProcessSteps.PreTransformVertices |
            PostProcessSteps.ValidateDataStructure | PostProcessSteps.FixInFacingNormals);
        
        if (!NewScene.HasMeshes)
            return null;

        Mesh AssimpMesh = NewScene.Meshes[0];
        bool IsMeshIndexed = AssimpMesh.Faces.Any() && AssimpMesh.Faces.All(MeshFace => MeshFace.Indices.Count == 3);
        int WriteIndex = 0;
        MeshData NewMeshData = new MeshData(AssimpMesh.VertexCount, AssimpMesh.FaceCount, IsMeshIndexed);

        // Only de-index the mesh if required
        if (IsMeshIndexed == true)
        {
            // Check if the normals array is valid
            if (AssimpMesh.HasNormals)
            {
                if (NewMeshData.Normals == null || NewMeshData.Normals.Length != NewMeshData.Vertices.Length)
                {
                    Console.WriteLine($"Warning: Normals count ({NewMeshData.Normals?.Length ?? 0}) does not match Vertices count ({NewMeshData.Vertices.Length}). Ignoring normals.");
                    NewMeshData.Normals = null;
                }
            }

            // Check if the texcoords array is valid
            if (AssimpMesh.HasTextureCoords(0))
            {
                if (NewMeshData.TexCoords == null || NewMeshData.TexCoords.Length != NewMeshData.Vertices.Length)
                {
                    Console.WriteLine($"Warning: TexCoords count ({NewMeshData.TexCoords?.Length ?? 0}) does not match Vertices count ({NewMeshData.Vertices.Length}). Ignoring texcoords.");
                    NewMeshData.TexCoords = null;
                }
            }

            // Now loop over vertices and copy data only if arrays are valid
            for (int i = 0; i < NewMeshData.Vertices.Length; i++)
            {
                // Copy vertex position (assumed always valid)
                NewMeshData.Vertices[i] = new Vector3(
                    AssimpMesh.Vertices[i].X,
                    AssimpMesh.Vertices[i].Y,
                    AssimpMesh.Vertices[i].Z);

                if (NewMeshData.Normals != null)
                {
                    NewMeshData.Normals[i] = new Vector3(
                        AssimpMesh.Normals[i].X,
                        AssimpMesh.Normals[i].Y,
                        AssimpMesh.Normals[i].Z);
                }

                if (NewMeshData.TexCoords != null)
                {
                    NewMeshData.TexCoords[i] = new Vector2(
                        AssimpMesh.TextureCoordinateChannels[0][i].X,
                        1.0f - AssimpMesh.TextureCoordinateChannels[0][i].Y);
                }
            }
            
            // Now we need to fill the index buffer
            int writeIndex = 0;
            
            for (int faceIndex = 0; faceIndex < AssimpMesh.FaceCount; faceIndex++)
            {
                var face = AssimpMesh.Faces[faceIndex];

                // All faces should have 3 indices
                for (int i = 0; i < face.Indices.Count; i++)
                {
                    NewMeshData.Indices[writeIndex] = face.Indices[i];
                    writeIndex++;
                }
            }
        }
        else
        {
            for (int FaceIndex = 0; FaceIndex < AssimpMesh.FaceCount; FaceIndex++)
            {
                for (int IndicesIndex = 0; IndicesIndex < AssimpMesh.Faces[FaceIndex].Indices.Count; IndicesIndex++)
                {
                    int Index = AssimpMesh.Faces[FaceIndex].Indices[IndicesIndex];

                    // Vertex position
                    NewMeshData.Vertices[WriteIndex] = new Vector3(AssimpMesh.Vertices[Index].X,
                        AssimpMesh.Vertices[Index].Y, AssimpMesh.Vertices[Index].Z);

                    // Normal
                    if (AssimpMesh.HasNormals == true)
                    {
                        NewMeshData.Normals[WriteIndex] = new Vector3(AssimpMesh.Normals[Index].X,
                            AssimpMesh.Normals[Index].Y, AssimpMesh.Normals[Index].Z);
                    }

                    // UV0
                    if (AssimpMesh.HasTextureCoords(0) == true)
                    {
                        NewMeshData.TexCoords[WriteIndex] = new Vector2(
                            AssimpMesh.TextureCoordinateChannels[0][Index].X,
                            1.0f - AssimpMesh.TextureCoordinateChannels[0][Index].Y);
                    }

                    WriteIndex++;
                }
            }
        }

        RaylibMesh NewMesh = new RaylibMesh();
        NewMesh.UpdateMeshData(NewMeshData);
        NewMesh.Material = RaylibMaterial.DefaultMaterial;
        Meshes.Add(NewMesh);
        return NewMesh;
    }

    public Texture LoadTexture(string Filepath) => Raylib.LoadTexture(Filepath);

    public void Render()
    {
        TriangleCount = 0;
        
        foreach (Camera cam in Cameras)
        {
            UpdateRLCam(cam);
            Raylib.BeginMode3D(RLCamera);

            foreach (var imesh in Meshes.AsValueEnumerable().Where(x => x.Enabled && x is RaylibMesh))
            {
                var mesh = (RaylibMesh)imesh;
                TriangleCount += mesh.RLMesh.VertexCount / 3;
                
                Raylib.DrawMesh(mesh.RLMesh, ((RaylibMaterial)mesh.Material).RLMaterial, Matrix4x4.Transpose(mesh.Transform.Matrix));
            }
            
            Raylib.EndMode3D();
        }
    }
}