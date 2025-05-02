using System.Numerics;

namespace Transforms.Components
{
    /// <summary>
    /// Implies that the final position of this entity is affected by its scale.
    /// </summary>
    public struct Pivot
    {
        /// <summary>
        /// Offsets the entity by half of its scale.
        /// </summary>
        public static readonly Pivot Centered = new(0.5f, 0.5f, 0.5f);

        /// <summary>
        /// Offsets the entity to the right by its width.
        /// </summary>
        public static readonly Pivot BottomRight = new(1f, 0f, 0f);

        /// <summary>
        /// Applies no offset to the entity (same as <see langword="default"/>).
        /// </summary>
        public static readonly Pivot BottomLeft = new(0f, 0f, 0f);

        /// <summary>
        /// Offsets the entity to the right by its width, and up by its height.
        /// </summary>
        public static readonly Pivot TopRight = new(1f, 1f, 0f);

        /// <summary>
        /// Offsets the entity up by its height.
        /// </summary>
        public static readonly Pivot TopLeft = new(0f, 1f, 0f);

        /// <summary>
        /// The default state.
        /// </summary>
        public static readonly Pivot Default = default;

        /// <summary>
        /// The pivot value.
        /// </summary>
        public Vector3 value;

        /// <summary>
        /// Creates a new <see cref="Pivot"/> component.
        /// </summary>
        public Pivot(float x, float y, float z = default)
        {
            value = new Vector3(x, y, z);
        }

        /// <summary>
        /// Initializes the component with the given <paramref name="value"/>.
        /// </summary>
        public Pivot(Vector3 value)
        {
            this.value = value;
        }
    }
}
