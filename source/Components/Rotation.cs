using System;
using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Describes the rotation of the entity relative to its parent.   
    /// </summary>
    public struct Rotation
    {
        /// <summary>
        /// The intended <see langword="default"/> state of this component.
        /// </summary>
        public static readonly Rotation Default = new(Quaternion.Identity);

        /// <summary>
        /// The value.
        /// </summary>
        public Quaternion value;

#if NET
        /// <summary>
        /// Creates a default state of this component.
        /// </summary>
        public Rotation()
        {
            value = Quaternion.Identity;
        }
#endif

        /// <summary>
        /// Initializes the component with the given <paramref name="value"/>.
        /// </summary>
        public Rotation(Quaternion value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes the component with the given <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>, and <paramref name="w"/>.
        /// </summary>
        public Rotation(float x, float y, float z, float w)
        {
            value = new Quaternion(x, y, z, w);
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

        /// <summary>
        /// Builds a new <see cref="Rotation"/> component from the given <paramref name="forwardDirection"/>.
        /// </summary>
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
