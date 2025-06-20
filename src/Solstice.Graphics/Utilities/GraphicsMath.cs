using System.Numerics;

namespace Solstice.Graphics.Utilities;

public static class GraphicsMath
{
    public static Vector3 QuaternionToEuler(Quaternion q)
    {
        // Pitch (X axis rotation)
        float sinp = 2.0f * (q.W * q.X + q.Y * q.Z);
        float cosp = 1.0f - 2.0f * (q.X * q.X + q.Y * q.Y);
        float pitch = MathF.Atan2(sinp, cosp);

        // Yaw (Y axis rotation)
        float siny = 2.0f * (q.W * q.Y - q.Z * q.X);
        float yaw;
        if (MathF.Abs(siny) >= 1f)
            yaw = MathF.CopySign(MathF.PI / 2f, siny); // use 90 degrees if out of range
        else
            yaw = MathF.Asin(siny);

        // Roll (Z axis rotation)
        float sinr = 2.0f * (q.W * q.Z + q.X * q.Y);
        float cosr = 1.0f - 2.0f * (q.Y * q.Y + q.Z * q.Z);
        float roll = MathF.Atan2(sinr, cosr);

        return new Vector3(pitch, yaw, roll); // in radians
    }
}