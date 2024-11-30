using System.Numerics;
using Worlds;

namespace Transforms.Components
{
    [Component]
    public struct LocalToWorld
    {
        public static readonly LocalToWorld Default = new(Components.Position.Default.value, Components.Rotation.Default.value, Components.Scale.Default.value);

        public Matrix4x4 value;

        public readonly Vector3 Position => value.Translation;
        public readonly Vector3 Right => Vector3.Normalize(new(value.M11, value.M12, value.M13));
        public readonly Vector3 Up => Vector3.Normalize(new(value.M21, value.M22, value.M23));
        public readonly Vector3 Forward => Vector3.Normalize(new(value.M31, value.M32, value.M33));

        public readonly Quaternion Rotation
        {
            get
            {
                Matrix4x4.Decompose(value, out _, out Quaternion rotation, out _);
                return rotation;
            }
        }

        public readonly Vector3 Scale
        {
            get
            {
                Matrix4x4.Decompose(value, out Vector3 scale, out _, out _);
                return scale;
            }
        }

        public readonly (Vector3 position, Quaternion rotation, Vector3 scale) Decomposed
        {
            get
            {
                Matrix4x4.Decompose(value, out Vector3 scale, out Quaternion rotation, out Vector3 position);
                return (position, rotation, scale);
            }
        }

#if NET
        public LocalToWorld()
        {
            value = Default.value;
        }
#endif

        public LocalToWorld(Matrix4x4 value)
        {
            this.value = value;
        }

        public LocalToWorld(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            value = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        }
    }
}
