using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;
using Assimp;
using ZLinq;
using Camera = Solstice.Common.Classes.Camera;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Solstice.Graphics.Implementations;

public class RaylibGraphics : IGraphics
{
    public List<IMesh> Meshes { get; }
    public List<Camera> Cameras { get; set; }
    
    public Camera3D RLCamera;

    public RaylibGraphics()
    {
        Meshes = new List<IMesh>();
        Cameras = new List<Camera>();
        RLCamera = new Camera3D();
        RLCamera.Up = Vector3.UnitY;
    }

    public void UpdateRLCam(Camera CurrentCamera)
    {
        RLCamera.Projection = (int)CurrentCamera.Projection;
        RLCamera.Position = CurrentCamera.Transform.Position;
        RLCamera.Fovy = CurrentCamera.FOV;

        // Convert the camera's rotation quaternion and to a point on the unit sphere. We use the Z-axis as the default forward direction
        //RLCamera.Target = RLCamera.Position + Vector3.Transform(Vector3.UnitZ, CurrentCamera.Transform.Rotation);

        var forward = Vector3.Transform(Vector3.UnitZ, CurrentCamera.Transform.Rotation);
        var up = Vector3.Transform(Vector3.UnitY, CurrentCamera.Transform.Rotation);

        if (forward.LengthSquared() < 0.0001f)
            forward = Vector3.UnitZ;  // fallback safe forward

        if (up.LengthSquared() < 0.0001f)
            up = Vector3.UnitY;  // fallback safe forward

        //RLCamera.Target = RLCamera.Position + forward;
        RLCamera.Target = RLCamera.Position + Vector3.Transform(Vector3.UnitZ, CurrentCamera.Transform.Rotation);
        RLCamera.Up = up;
    }

    public IMesh? LoadMesh(string Filename)
    {
        AssimpContext Loader = new AssimpContext();
        Scene SceneFile = Loader.ImportFile(Filename, PostProcessSteps.Triangulate);

        if (SceneFile.HasMeshes == false)
        {
            return null;
        }

        RaylibMesh NewMesh = new RaylibMesh();
        MeshData NewMeshData = new MeshData();
        var AssimpMesh = SceneFile.Meshes[0];

        // Since we don't want to deal with indices yet, we need to de-index the vertices
        foreach (var Face in AssimpMesh.Faces)
        {
            foreach (int IndiciesIndex in Face.Indices)
            {
                Vector3D VertexPos = AssimpMesh.Vertices[IndiciesIndex];
                NewMeshData.Vertices.Add(new Vector3(VertexPos.X, VertexPos.Y, VertexPos.Z));

                if (AssimpMesh.HasNormals)
                {
                    var NormalPos = AssimpMesh.Normals[IndiciesIndex];
                    NewMeshData.Normals.Add(new Vector3(NormalPos.X, NormalPos.Y, NormalPos.Z));
                }

                if (AssimpMesh.HasTextureCoords(0))
                {
                    var UVPos = AssimpMesh.TextureCoordinateChannels[0][IndiciesIndex];
                    NewMeshData.TexCoords.Add(new Vector2(UVPos.X, UVPos.Y));
                }

                // etc.
            }
        }

        // Update the raylib internal mesh and add it to the list of meshes to render
        NewMesh.UpdateMeshData(NewMeshData);
        
        NewMesh.Material = RaylibMaterial.DefaultMaterial;
        
        Meshes.Add(NewMesh);
        return NewMesh;
    }

    public void Render()
    {
        foreach (Camera cam in Cameras)
        {
            Raylib.BeginMode3D(RLCamera);

            foreach (var imesh in Meshes.AsValueEnumerable().Where(x => x.Enabled && x is RaylibMesh))
            {
                var mesh = (RaylibMesh)imesh;
                UpdateRLCam(cam);
                Raylib.DrawMesh(mesh.RLMesh, ((RaylibMaterial)mesh.Material).RLMaterial, mesh.Transform.Matrix);
            }
            
            Raylib.EndMode3D();
        }
    }
}