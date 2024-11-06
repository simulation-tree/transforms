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
            (float s, float c) z = MathF.SinCos(value.Z);
            (float s, float c) y = MathF.SinCos(value.Y);
            (float s, float c) x = MathF.SinCos(value.X);
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
