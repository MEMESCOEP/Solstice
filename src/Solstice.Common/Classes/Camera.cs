using System.Drawing;
using System.Numerics;

namespace Solstice.Common.Classes;

public enum CameraProjection
{
    Perspective,
    Orthographic
}

public class Camera
{
    public CameraProjection Projection = CameraProjection.Perspective;
    public Transform Transform = new Transform();
    public float FOV = 75f;
    public Size Size;

    /// <summary>
    /// The view plane for the camera
    /// NOTE: The X component is used to define the near plane, and the Y component is used to define the far plane
    /// </summary>
    public Vector2 ViewPlanes;
}