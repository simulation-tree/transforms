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

        public readonly Vector3 Position
        {
            get
            {
                if (entity.TryGetComponent(out Position component))
                {
                    return component.value;
                }
                else
                {
                    return Components.Position.Default.value;
                }
            }
            set
            {
                ref Position component = ref entity.TryGetComponentRef<Position>(out bool contains);
                if (contains)
                {
                    component.value = value;
                }
                else
                {
                    entity.AddComponent(new Position(value));
                }
            }
        }

        public readonly Quaternion Rotation
        {
            get
            {
                if (entity.TryGetComponent(out Rotation component))
                {
                    return component.value;
                }
                else
                {
                    return Components.Rotation.Default.value;
                }
            }
            set
            {
                ref Rotation component = ref entity.TryGetComponentRef<Rotation>(out bool contains);
                if (contains)
                {
                    component.value = value;
                }
                else
                {
                    entity.AddComponent(new Rotation(value));
                }
            }
        }

        public readonly Vector3 Scale
        {
            get
            {
                if (entity.TryGetComponent(out Scale component))
                {
                    return component.value;
                }
                else
                {
                    return Components.Scale.Default.value;
                }
            }
            set
            {
                ref Scale component = ref entity.TryGetComponentRef<Scale>(out bool contains);
                if (contains)
                {
                    component.value = value;
                }
                else
                {
                    entity.AddComponent(new Scale(value));
                }
            }
        }

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

        public Transform(World world, eint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Transform(World world, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            entity = new(world);
            entity.AddComponent(new Position(position));
            entity.AddComponent(new Scale(scale));
            entity.AddComponent(new Rotation(rotation));
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
    }
}