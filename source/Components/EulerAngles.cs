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

        public readonly Quaternion AsQuaternion()
        {
            float cy = (float)Math.Cos(value.Z * 0.5);
            float sy = (float)Math.Sin(value.Z * 0.5);
            float cp = (float)Math.Cos(value.Y * 0.5);
            float sp = (float)Math.Sin(value.Y * 0.5);
            float cr = (float)Math.Cos(value.X * 0.5);
            float sr = (float)Math.Sin(value.X * 0.5);
            return new Quaternion
            {
                W = cr * cp * cy + sr * sp * sy,
                X = sr * cp * cy - cr * sp * sy,
                Y = cr * sp * cy + sr * cp * sy,
                Z = cr * cp * sy - sr * sp * cy
            };
        }

        public static EulerAngles CreateFromDegrees(float x, float y, float z)
        {
            float ratio = MathF.PI / 180f;
            return new EulerAngles(x * ratio, y * ratio, z * ratio);
        }
    }
}
