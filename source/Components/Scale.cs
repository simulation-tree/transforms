using System.Numerics;

namespace Transforms.Components
{
    public struct Scale
    {
        public static readonly Scale Default = new(Vector3.One);

        public Vector3 value;

        public Scale()
        {
            value = Vector3.One;
        }

        public Scale(Vector2 value, float z = 1f)
        {
            this.value = new(value, z);
        }

        public Scale(Vector3 value)
        {
            this.value = value;
        }

        public Scale(float x, float y, float z = 1f)
        {
            value = new Vector3(x, y, z);
        }
    }
}