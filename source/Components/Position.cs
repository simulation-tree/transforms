using System;
using System.Numerics;

namespace Transforms.Components
{
    public struct Position
    {
        public static readonly Position Default = new(Vector3.Zero);

        public Vector3 value;

        public Position(Vector2 value, float z = 0f)
        {
            this.value = new Vector3(value, z);
        }

        public Position(Vector3 value)
        {
            this.value = value;
        }

        public Position(float x, float y)
        {
            value = new Vector3(x, y, Default.value.Z);
        }

        public Position(float x, float y, float z)
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
