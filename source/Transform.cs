using System;
using System.Numerics;
using Transforms.Components;
using Unmanaged;
using Worlds;

namespace Transforms
{
    //todo: add extension methods
    public readonly struct Transform : ITransform, IEquatable<Transform>
    {
        private readonly Entity entity;

        /// <summary>
        /// Position of this entity relative to the parent entity.
        /// </summary>
        public readonly ref Vector3 LocalPosition
        {
            get
            {
                if (!entity.ContainsComponent<Position>())
                {
                    entity.AddComponent(Position.Default);
                }

                ref Position component = ref entity.GetComponent<Position>();
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
                if (!entity.ContainsComponent<Rotation>())
                {
                    entity.AddComponent(Rotation.Default);
                }

                ref Rotation component = ref entity.GetComponent<Rotation>();
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
                if (!entity.ContainsComponent<Scale>())
                {
                    entity.AddComponent(Scale.Default);
                }

                ref Scale component = ref entity.GetComponent<Scale>();
                return ref component.value;
            }
        }

        public readonly Vector3 LocalRight => Vector3.Transform(Vector3.UnitX, LocalRotation);
        public readonly Vector3 LocalUp => Vector3.Transform(Vector3.UnitY, LocalRotation);
        public readonly Vector3 LocalForward => Vector3.Transform(Vector3.UnitZ, LocalRotation);
        public readonly Vector3 WorldPosition => entity.GetComponent<LocalToWorld>().Position;
        public readonly Quaternion WorldRotation => entity.GetComponent<WorldRotation>().value;
        public readonly Vector3 WorldScale => entity.GetComponent<LocalToWorld>().Scale;
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);
        public readonly LocalToWorld LocalToWorld => entity.GetComponent<LocalToWorld>();

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<LocalToWorld>();
            archetype.AddTagType<IsTransform>();
        }

        public Transform(World world, uint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Transform(World world)
        {
            entity = new Entity<Position, Rotation, Scale, LocalToWorld, WorldRotation>(world, Position.Default, Rotation.Default, Scale.Default, Components.LocalToWorld.Default, Components.WorldRotation.Default).AsEntity().As<Transform>();
            entity.AddTag<IsTransform>();
        }

        public Transform(World world, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            entity = new Entity<Position, Rotation, Scale, LocalToWorld, WorldRotation>(world, new Position(position), new Rotation(rotation), new Scale(scale), new LocalToWorld(position, rotation, scale), new WorldRotation(rotation)).AsEntity().As<Transform>();
            entity.AddTag<IsTransform>();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[64];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return entity.ToString(buffer);
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Transform transform && Equals(transform);
        }

        public readonly bool Equals(Transform other)
        {
            return entity.Equals(other.entity);
        }

        public readonly override int GetHashCode()
        {
            return entity.GetHashCode();
        }

        public static implicit operator Entity(Transform transform)
        {
            return transform.entity;
        }

        public static bool operator ==(Transform left, Transform right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Transform left, Transform right)
        {
            return !(left == right);
        }
    }
}