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
        
        static Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsTransform>());
        }
    }
}