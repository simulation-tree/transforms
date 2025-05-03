using System;
using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Describes the position of the entity relative to its parent.
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// The <see langword="default"/> value.
        /// </summary>
        public static readonly Position Default = new(Vector3.Zero);

        /// <summary>
        /// The value.
        /// </summary>
        public Vector3 value;

        /// <summary>
        /// Initializes the component with the given <paramref name="value"/>.
        /// </summary>
        public Position(Vector2 value, float z = 0f)
        {
            this.value = new Vector3(value, z);
        }

        /// <summary>
        /// Initializes the component with the given <paramref name="value"/>.
        /// </summary>
        public Position(Vector3 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes the component with the given <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        public Position(float x, float y)
        {
            value = new Vector3(x, y, Default.value.Z);
        }

        /// <summary>
        /// Initializes the component with the given <paramref name="x"/>, <paramref name="y"/>, and <paramref name="z"/>.
        /// </summary>
        public Position(float x, float y, float z)
        {
            value = new Vector3(x, y, z);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        /// <inheritdoc/>
        public readonly int ToString(Span<char> destination)
        {
            return value.ToString(destination);
        }
    }
}
