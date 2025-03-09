using System;
using System.Numerics;

namespace Transforms.Components
{
    public struct Rotation
    {
        public static readonly Rotation Default = new(Quaternion.Identity);

        public Quaternion value;

#if NET
        public Rotation()
        {
            value = Quaternion.Identity;
        }
#endif

        public Rotation(Quaternion value)
        {
            this.value = value;
        }

        public Rotation(float x, float y, float z, float w)
        {
            value = new Quaternion(x, y, z, w);
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            return value.ToString(destination);
        }

        public static Rotation FromDirection(Vector3 forwardDirection)
        {
            Vector3 identity = Vector3.UnitZ;
            float dot = Vector3.Dot(identity, forwardDirection);
            if (dot < -0.9999f)
            {
                return new Rotation(Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI));
            }
            else if (dot > 0.9999f)
            {
                return new Rotation(Quaternion.Identity);
            }
            else
            {
                float angle = (float)System.Math.Acos(dot);
                Vector3 axis = Vector3.Cross(identity, forwardDirection);
                axis = Vector3.Normalize(axis);
                return new Rotation(Quaternion.CreateFromAxisAngle(axis, angle));
            }
        }
    }
}
