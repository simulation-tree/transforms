using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Describes the calculated and final rotation of the entity.
    /// </summary>
    public struct WorldRotation
    {
        /// <summary>
        /// The intended <see langword="default"/> state of this component.
        /// </summary>
        public static readonly WorldRotation Default = new(Quaternion.Identity);

        /// <summary>
        /// The value.
        /// </summary>
        public Quaternion value;

#if NET
        /// <summary>
        /// Creates a default state of this component.
        /// </summary>
        public WorldRotation()
        {
            value = Default.value;
        }
#endif

        /// <summary>
        /// Creates a new <see cref="WorldRotation"/> component from the given <paramref name="value"/>.
        /// </summary>
        public WorldRotation(Quaternion value)
        {
            this.value = value;
        }
    }
}
