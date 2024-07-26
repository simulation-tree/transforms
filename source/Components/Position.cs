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
    }
}
