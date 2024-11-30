using System.Numerics;
using Unmanaged;
using Worlds;

namespace Transforms.Components
{
    [Component]
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
            USpan<char> buffer = stackalloc char[32];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return value.ToString(buffer);
        }
    }
}