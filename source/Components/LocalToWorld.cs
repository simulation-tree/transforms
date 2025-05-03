using System;
using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Calculated <see cref="Matrix4x4"/> for transforming local space to world space.
    /// </summary>
    public struct LocalToWorld : IEquatable<LocalToWorld>
    {
        /// <summary>
        /// Default state of this component.
        /// </summary>
        public static readonly LocalToWorld Default = new(Components.Position.Default.value, Components.Rotation.Default.value, Components.Scale.Default.value);

        /// <summary>
        /// The matrix value.
        /// </summary>
        public Matrix4x4 value;

        /// <summary>
        /// Decomposed position of the matrix value.
        /// </summary>
        public Vector3 Position
        {
            readonly get => value.Translation;
            set => this.value.Translation = value;
        }

        /// <summary>
        /// Vector referring to the right direction of the matrix value.
        /// </summary>
        public readonly Vector3 Right => Vector3.Normalize(new(value.M11, value.M12, value.M13));

        /// <summary>
        /// Vector referring to the up direction of the matrix value.
        /// </summary>
        public readonly Vector3 Up => Vector3.Normalize(new(value.M21, value.M22, value.M23));

        /// <summary>
        /// Vector referring to the forward direction of the matrix value.
        /// </summary>
        public readonly Vector3 Forward => Vector3.Normalize(new(value.M31, value.M32, value.M33));

        /// <summary>
        /// Decomposed rotation of the matrix value.
        /// </summary>
        public readonly Quaternion Rotation
        {
            get
            {
                Matrix4x4.Decompose(value, out _, out Quaternion rotation, out _);
                return rotation;
            }
        }

        /// <summary>
        /// Decomposed scale of the matrix value.
        /// </summary>
        public readonly Vector3 Scale
        {
            get
            {
                Matrix4x4.Decompose(value, out Vector3 scale, out _, out _);
                return scale;
            }
        }

        /// <summary>
        /// Decomposed state of this component.
        /// </summary>
        public readonly (Vector3 position, Quaternion rotation, Vector3 scale) Decomposed
        {
            get
            {
                Matrix4x4.Decompose(value, out Vector3 scale, out Quaternion rotation, out Vector3 position);
                return (position, rotation, scale);
            }
        }

#if NET
        /// <summary>
        /// Creates a default <see cref="LocalToWorld"/> component.
        /// </summary>
        public LocalToWorld()
        {
            value = Default.value;
        }
#endif

        /// <summary>
        /// Initializes a new component from the given <paramref name="value"/>.
        /// </summary>
        public LocalToWorld(Matrix4x4 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new component from the given <paramref name="position"/>, <paramref name="rotation"/>, and <paramref name="scale"/>.
        /// </summary>
        public LocalToWorld(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            value = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            return obj is LocalToWorld world && Equals(world);
        }

        /// <inheritdoc/>
        public readonly bool Equals(LocalToWorld other)
        {
            return value.Equals(other.value);
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return HashCode.Combine(value);
        }

        /// <summary>
        /// Transforms the given <paramref name="worldPosition"/> to local space.
        /// </summary>
        public readonly Vector3 TransformInverse(Vector3 worldPosition)
        {
            Matrix4x4.Invert(value, out Matrix4x4 invValue);
            return Vector3.Transform(worldPosition, invValue);
        }

        /// <summary>
        /// Transforms the given <paramref name="localPosition"/> to world space.
        /// </summary>
        public readonly Vector3 Transform(Vector3 localPosition)
        {
            return Vector3.Transform(localPosition, value);
        }

        /// <summary>
        /// Transforms the given <paramref name="worldRotation"/> into local space.
        /// </summary>
        public readonly Quaternion TransformInverse(Quaternion worldRotation)
        {
            Matrix4x4.Invert(value, out Matrix4x4 invValue);
            return Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(invValue) * worldRotation);
        }

        /// <summary>
        /// Transforms the given <paramref name="localRotation"/> into world space.
        /// </summary>
        public readonly Quaternion Transform(Quaternion localRotation)
        {
            return Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(value) * localRotation);
        }

        /// <inheritdoc/>
        public static bool operator ==(LocalToWorld left, LocalToWorld right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(LocalToWorld left, LocalToWorld right)
        {
            return !(left == right);
        }
    }
}
