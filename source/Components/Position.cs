using System.Numerics;
using Unmanaged;

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
            USpan<char> buffer = stackalloc char[32];
            uint length = ToString(buffer);
            return buffer.GetSpan(length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return value.ToString(buffer);
        }
    }
}
