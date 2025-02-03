using System.Numerics;
using Transforms.Components;
using Unmanaged;
using Worlds;

namespace Transforms
{
    public readonly partial struct Transform : IEntity
    {
        /// <summary>
        /// Position of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Vector3 LocalPosition
        {
            get
            {
                ref Position component = ref TryGetComponent<Position>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    return ref AddComponent(Position.Default).value;
                }
            }
        }

        /// <summary>
        /// Rotation of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Quaternion LocalRotation
        {
            get
            {
                ref Rotation component = ref TryGetComponent<Rotation>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    return ref AddComponent(Rotation.Default).value;
                }
            }
        }

        /// <summary>
        /// Scale of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Vector3 LocalScale
        {
            get
            {
                ref Scale component = ref TryGetComponent<Scale>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    return ref AddComponent(Scale.Default).value;
                }
            }
        }

        public readonly Vector3 LocalRight => Vector3.Transform(Vector3.UnitX, LocalRotation);
        public readonly Vector3 LocalUp => Vector3.Transform(Vector3.UnitY, LocalRotation);
        public readonly Vector3 LocalForward => Vector3.Transform(Vector3.UnitZ, LocalRotation);
        public readonly Vector3 WorldPosition => GetComponent<LocalToWorld>().Position;
        public readonly Quaternion WorldRotation => GetComponent<WorldRotation>().value;
        public readonly Vector3 WorldScale => GetComponent<LocalToWorld>().Scale;
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);
        public readonly LocalToWorld LocalToWorld => GetComponent<LocalToWorld>();

        public Transform(World world)
        {
            this.world = world;
            value = world.CreateEntity(Position.Default, Rotation.Default, Scale.Default, LocalToWorld.Default, Components.WorldRotation.Default);
            AddTag<IsTransform>();
        }

        public Transform(World world, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.world = world;
            value = world.CreateEntity(new Position(position), new Rotation(rotation), new Scale(scale), new LocalToWorld(position, rotation, scale), new WorldRotation(rotation));
            AddTag<IsTransform>();
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<LocalToWorld>();
            archetype.AddTagType<IsTransform>();
        }

        public readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[64];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return value.ToString(buffer);
        }
    }
}