using System;
using System.Numerics;

namespace Transforms.Components
{
    public struct Scale
    {
        public static readonly Scale Default = new(Vector3.One);

        public Vector3 value;

#if NET
        public Scale()
        {
            value = Vector3.One;
        }
#endif

        public Scale(Vector2 value, float z = 1f)
        {
            this.value = new(value, z);
        }

        public Scale(Vector3 value)
        {
            this.value = value;
        }

        public Scale(float x, float y)
        {
            value = new Vector3(x, y, Default.value.Z);
        }

        public Scale(float x, float y, float z)
        {
            value = new Vector3(x, y, z);
        }

        public unsafe readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            return value.ToString(destination);
        }
    }
}