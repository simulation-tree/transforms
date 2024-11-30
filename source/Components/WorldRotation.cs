using System.Numerics;
using Worlds;

namespace Transforms.Components
{
    [Component]
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
