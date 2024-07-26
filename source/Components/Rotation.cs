using System.Numerics;

namespace Transforms.Components
{
    public struct Rotation
    {
        public static readonly Rotation Default = new(Quaternion.Identity);

        public Quaternion value;

#if NET5_0_OR_GREATER
        public Rotation()
        {
            value = Quaternion.Identity;
        }
#endif

        public Rotation(Quaternion value)
        {
            this.value = value;
        }

        public Rotation(float x, float y, float z, float w)
        {
            value = new Quaternion(x, y, z, w);
        }
    }
}
