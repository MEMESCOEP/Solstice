using System.Numerics;

namespace Solstice.Common.Classes;

public class Transform
{
    /// <summary>
    /// The 3D position; Vector3.Zero is the world origin
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// NOTE: Quaternions are to be used for rotations in order to prevent gimbal lock. To prevent normalizing every frame when this may not be needed, we just do it when the quaternion is changed
    /// </summary>
    private Quaternion _rotation = Quaternion.Identity;
    public Quaternion Rotation
    {
        get => _rotation;
        set => _rotation = Quaternion.Normalize(value);
    }

    /// <summary>
    /// The 3D scale; Vector3.One represents no scalling
    /// </summary>
    public Vector3 Scale;

    /// <summary>
    /// Rotates the transform by euler angles
    /// NOTE: Euler angles are simpler than quaternions, but are susceptible to gimbal lock. See https://en.wikipedia.org/wiki/Gimbal_lock
    /// </summary>
    /// <param name="Angles">The X, Y, and Z angles in radians</param>
    public void RotateByEuler(Vector3 Angles)
    {
        // Create quaternions for each axis
        Quaternion RotX = Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitX, Rotation), Angles.X);
        Quaternion RotY = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Angles.Y);
        Quaternion RotZ = Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitZ, Rotation), Angles.Z);

        // Combine rotations (order matters!) and apply that to the current rotation quaternion
        // A common multiplication order is Y (yaw) -> X (pitch) -> Z (roll)
        Rotation = (RotY * RotX * RotZ) * Rotation;
    }

    /// <summary>
    /// Rotates the transform to euler angles
    /// NOTE: Euler angles are simpler than quaternions, but are susceptible to gimbal lock. See https://en.wikipedia.org/wiki/Gimbal_lock
    /// </summary>
    /// <param name="Angles">The X, Y, and Z angles in radians</param>
    public void RotateToEuler(Vector3 Angles)
    {
        // Create quaternions for each axis
        Quaternion RotX = Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitX, Rotation), Angles.X);
        Quaternion RotY = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Angles.Y);
        Quaternion RotZ = Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitZ, Rotation), Angles.Z);

        // Combine rotations (order matters!) and set the current rotation to that
        // A common multiplication order is Y (yaw) -> X (pitch) -> Z (roll)
        Rotation = RotY * RotX * RotZ;
    }
    
    public Matrix4x4 Matrix => Matrix4x4.CreateScale(Scale) * 
                               Matrix4x4.CreateFromQuaternion(Rotation) * 
                               Matrix4x4.CreateTranslation(Position);
}