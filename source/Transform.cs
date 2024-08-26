using Simulation;
using System;
using System.Numerics;
using Transforms.Components;
using Unmanaged;

namespace Transforms
{
    public readonly struct Transform : ITransform, IDisposable
    {
        private readonly Entity entity;

        public readonly ref Vector3 LocalPosition
        {
            get
            {
                ref Position component = ref entity.TryGetComponentRef<Position>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    component = ref entity.AddComponentRef<Position>();
                    component.value = Position.Default.value;
                    return ref component.value;
                }
            }
        }

        public readonly ref Quaternion LocalRotation
        {
            get
            {
                ref Rotation component = ref entity.TryGetComponentRef<Rotation>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    component = ref entity.AddComponentRef<Rotation>();
                    component.value = Rotation.Default.value;
                    return ref component.value;
                }
            }
        }

        public readonly ref Vector3 LocalScale
        {
            get
            {
                ref Scale component = ref entity.TryGetComponentRef<Scale>(out bool contains);
                if (contains)
                {
                    return ref component.value;
                }
                else
                {
                    component = ref entity.AddComponentRef<Scale>();
                    component.value = Scale.Default.value;
                    return ref component.value;
                }
            }
        }

        public readonly Vector3 LocalRight => Vector3.Transform(Vector3.UnitX, LocalRotation);
        public readonly Vector3 LocalUp => Vector3.Transform(Vector3.UnitY, LocalRotation);
        public readonly Vector3 LocalForward => Vector3.Transform(Vector3.UnitZ, LocalRotation);
        public readonly Vector3 WorldPosition
        {
            get
            {
                return entity.GetComponent<LocalToWorld>().Position;
            }
            set
            {
                Matrix4x4.Invert(entity.GetComponent<LocalToWorld>().value, out Matrix4x4 wtl);
                LocalPosition = Vector3.Transform(value, wtl);
            }
        }

        public readonly Quaternion WorldRotation
        {
            get
            {
                return entity.GetComponent<LocalToWorld>().Rotation;
            }
            set
            {
                Matrix4x4.Invert(entity.GetComponent<LocalToWorld>().value, out Matrix4x4 wtl);
                LocalRotation = Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(wtl) * value);
            }
        }

        public readonly Vector3 WorldScale => entity.GetComponent<LocalToWorld>().Scale;
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);

        World IEntity.World => entity;
        eint IEntity.Value => entity;

        public Transform(World world, eint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Transform(World world)
        {
            entity = new(world);
            entity.AddComponent(Position.Default);
            entity.AddComponent(Rotation.Default);
            entity.AddComponent(Scale.Default);
            entity.AddComponent(new IsTransform());
            entity.AddComponent(new LocalToWorld(Position.Default.value, Rotation.Default.value, Scale.Default.value));
        }

        public Transform(World world, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            entity = new(world);
            entity.AddComponent(new Position(position));
            entity.AddComponent(new Rotation(rotation));
            entity.AddComponent(new Scale(scale));
            entity.AddComponent(new IsTransform());
            entity.AddComponent(new LocalToWorld(position, rotation, scale));
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsTransform>());
        }

        public static implicit operator Entity(Transform transform)
        {
            return transform.entity;
        }
    }
}