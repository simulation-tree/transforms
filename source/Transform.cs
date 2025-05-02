using System;
using System.Numerics;
using Transforms.Components;
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
                int type = world.Schema.GetComponentType<Position>();
                ref Position component = ref TryGetComponent<Position>(type, out bool contains);
                if (!contains)
                {
                    component = ref AddComponent<Position>(type);
                    component = Position.Default;
                }

                return ref component.value;
            }
        }

        /// <summary>
        /// Rotation of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Quaternion LocalRotation
        {
            get
            {
                int type = world.Schema.GetComponentType<Rotation>();
                ref Rotation component = ref TryGetComponent<Rotation>(type, out bool contains);
                if (!contains)
                {
                    component = ref AddComponent<Rotation>(type);
                    component = Rotation.Default;
                }

                return ref component.value;
            }
        }

        /// <summary>
        /// The anchor of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Anchor Anchor
        {
            get
            {
                int type = world.Schema.GetComponentType<Anchor>();
                ref Anchor component = ref TryGetComponent<Anchor>(type, out bool contains);
                if (!contains)
                {
                    component = ref AddComponent<Anchor>(type);
                    component = Anchor.Default;
                }

                return ref component;
            }
        }

        /// <summary>
        /// The pivot of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Vector3 Pivot
        {
            get
            {
                int type = world.Schema.GetComponentType<Pivot>();
                ref Pivot component = ref TryGetComponent<Pivot>(type, out bool contains);
                if (!contains)
                {
                    component = ref AddComponent<Pivot>(type);
                    component = Components.Pivot.Default;
                }

                return ref component.value;
            }
        }

        /// <summary>
        /// Scale of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Vector3 LocalScale
        {
            get
            {
                int type = world.Schema.GetComponentType<Scale>();
                ref Scale component = ref TryGetComponent<Scale>(type, out bool contains);
                if (!contains)
                {
                    component = ref AddComponent<Scale>(type);
                    component = Scale.Default;
                }

                return ref component.value;
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
            Span<char> buffer = stackalloc char[64];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> buffer)
        {
            return value.ToString(buffer);
        }
    }
}