using System;
using System.Numerics;

namespace Transforms.Components
{
    public struct EulerAngles
    {
        public static readonly EulerAngles Default = new(0, 0, 0);

        public Vector3 value;

        public EulerAngles(Vector3 value)
        {
            this.value = value;
        }

        public EulerAngles(float x, float y, float z)
        {
            value = new Vector3(x, y, z);
        }

        public EulerAngles(Quaternion rotation)
        {
            Quaternion normalized = Quaternion.Normalize(rotation);
            float sinPitch = 2f * (normalized.Y * normalized.Z + normalized.W * normalized.X);
            float cosPitch = 1f - 2f * (normalized.X * normalized.X + normalized.Y * normalized.Y);
            float pitch = MathF.Atan2(sinPitch, cosPitch);

            float sinYaw = 2f * (normalized.W * normalized.Y - normalized.X * normalized.Z);
            float yaw;
            if (MathF.Abs(sinYaw) >= 1f)
            {
                yaw = MathF.CopySign(MathF.PI * 0.5f, sinYaw);
            }
            else
            {
                yaw = MathF.Asin(sinYaw);
            }

            float sinRoll = 2f * (normalized.X * normalized.Y + normalized.W * normalized.Z);
            float cosRoll = 1f - 2f * (normalized.Y * normalized.Y + normalized.Z * normalized.Z);
            float roll = MathF.Atan2(sinRoll, cosRoll);
            value = new Vector3(pitch, yaw, roll);
        }

        public readonly Quaternion AsQuaternion()
        {
            Vector3 value = this.value * 0.5f;
            (float s, float c) x = MathF.SinCos(value.X);
            (float s, float c) y = MathF.SinCos(value.Y);
            (float s, float c) z = MathF.SinCos(value.Z);
            return new Quaternion
            {
                W = x.c * y.c * z.c + x.s * y.s * z.s,
                X = x.s * y.c * z.c - x.c * y.s * z.s,
                Y = x.c * y.s * z.c + x.s * y.c * z.s,
                Z = x.c * y.c * z.s - x.s * y.s * z.c
            };
        }

        public static EulerAngles CreateFromDegrees(float x, float y, float z)
        {
            float ratio = MathF.PI / 180f;
            return new EulerAngles(x * ratio, y * ratio, z * ratio);
        }
    }
}
