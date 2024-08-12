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

        public Position(float x, float y, float z = 0f)
        {
            value = new Vector3(x, y, z);
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[256];
            int length = ToString(buffer);
            return new string(buffer[..length]);
        }

        public readonly int ToString(Span<char> buffer)
        {
            value.X.TryFormat(buffer, out int written);
            buffer[written++] = ',';
            buffer[written++] = ' ';
            value.Y.TryFormat(buffer[written..], out int writtenY);
            buffer[written + writtenY++] = ',';
            buffer[written + writtenY++] = ' ';
            value.Z.TryFormat(buffer[(written + writtenY)..], out int writtenZ);
            return written + writtenY + writtenZ;
        }
    }
}
