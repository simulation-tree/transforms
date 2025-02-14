using System.Numerics;

namespace Transforms.Components
{
    public struct Pivot
    {
        public static readonly Pivot Centered = new(0.5f, 0.5f, 0.5f);
        public static readonly Pivot BottomRight = new(1f, 0f, 0f);
        public static readonly Pivot BottomLeft = new(0f, 0f, 0f);
        public static readonly Pivot TopRight = new(1f, 1f, 0f);
        public static readonly Pivot TopLeft = new(0f, 1f, 0f);
        public static readonly Pivot Default = default;

        public Vector3 value;

        public Pivot(float x, float y, float z = default)
        {
            value = new Vector3(x, y, z);
        }

        public Pivot(Vector3 value)
        {
            this.value = value;
        }
    }
}
