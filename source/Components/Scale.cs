using System;
using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Describes the scale of the entity relative to its parent.
    /// </summary>
    public struct Scale
    {
        /// <summary>
        /// The intended <see langword="default"/> state of the component.
        /// </summary>
        public static readonly Scale Default = new(Vector3.One);

        /// <summary>
        /// The value.
        /// </summary>
        public Vector3 value;

#if NET
        /// <summary>
        /// Creates a new default <see cref="Scale"/> component.
        /// </summary>
        public Scale()
        {
            value = Default.value;
        }
#endif

        /// <summary>
        /// Creates a new scale component from the given <paramref name="value"/>.
        /// </summary>
        public Scale(Vector2 value, float z = 1f)
        {
            this.value = new(value, z);
        }

        /// <summary>
        /// Creates a new scale component from the given <paramref name="value"/>.
        /// </summary>
        public Scale(Vector3 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new scale component from the given <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        public Scale(float x, float y)
        {
            value = new Vector3(x, y, Default.value.Z);
        }

        /// <summary>
        /// Creates a new scale component from the given <paramref name="x"/>, <paramref name="y"/>, and <paramref name="z"/>.
        /// </summary>
        public Scale(float x, float y, float z)
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