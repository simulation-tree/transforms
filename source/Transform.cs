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

        /// <summary>
        /// Vector pointing to the right of this entity relative to the parent entity.
        /// </summary>
        public readonly Vector3 LocalRight => Vector3.Transform(Vector3.UnitX, LocalRotation);

        /// <summary>
        /// Vector pointing up of this entity relative to the parent entity.
        /// </summary>
        public readonly Vector3 LocalUp => Vector3.Transform(Vector3.UnitY, LocalRotation);

        /// <summary>
        /// Vector pointing forward of this entity relative to the parent entity.
        /// </summary>
        public readonly Vector3 LocalForward => Vector3.Transform(Vector3.UnitZ, LocalRotation);

        /// <summary>
        /// Position of this entity in world space.
        /// </summary>
        public readonly Vector3 WorldPosition => GetComponent<LocalToWorld>().Position;

        /// <summary>
        /// Rotation of this entity in world space.
        /// </summary>
        public readonly Quaternion WorldRotation => GetComponent<WorldRotation>().value;

        /// <summary>
        /// Scale of this entity in world space.
        /// </summary>
        public readonly Vector3 WorldScale => GetComponent<LocalToWorld>().Scale;

        /// <summary>
        /// Vector pointing to the right of this entity in world space.
        /// </summary>
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);

        /// <summary>
        /// Vector pointing up of this entity in world space.
        /// </summary>
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);

        /// <summary>
        /// Vector pointing forward of this entity in world space.
        /// </summary>
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);

        /// <summary>
        /// The final calculated matrix for transforming the local components of this entity to world space.
        /// </summary>
        public readonly LocalToWorld LocalToWorld => GetComponent<LocalToWorld>();

        /// <summary>
        /// Creates a new transform entity.
        /// </summary>
        public Transform(World world)
        {
            this.world = world;
            value = world.CreateEntity(Position.Default, Rotation.Default, Scale.Default, LocalToWorld.Default, Components.WorldRotation.Default);
            AddTag<IsTransform>();
        }

        /// <summary>
        /// Creates a new transform entity with the given <paramref name="position"/>, <paramref name="rotation"/>, and <paramref name="scale"/>.
        /// </summary>
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

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[64];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        /// <inheritdoc/>
        public readonly int ToString(Span<char> buffer)
        {
            return value.ToString(buffer);
        }
    }
}