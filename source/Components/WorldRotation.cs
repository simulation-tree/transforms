using System.Numerics;

namespace Transforms.Components
{
    public struct WorldRotation
    {
        public static readonly WorldRotation Default = new(Quaternion.Identity);

        public Quaternion value;

#if NET
        public WorldRotation()
        {
            value = Default.value;
        }
#endif

        public WorldRotation(Quaternion value)
        {
            this.value = value;
        }
    }
}
