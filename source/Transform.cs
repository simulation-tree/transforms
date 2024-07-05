using Simulation;
using System;
using System.Numerics;
using Transforms.Components;

namespace Transforms
{
    public readonly struct Transform : IDisposable
    {
        public readonly Entity entity;

        public readonly Vector3 Position
        {
            get => entity.GetComponent<Position>().value;
            set => entity.GetComponentRef<Position>().value = value;
        }

        public readonly Vector3 Scale
        {
            get => entity.GetComponent<Scale>().value;
            set => entity.GetComponentRef<Scale>().value = value;
        }

        public readonly Quaternion Rotation
        {
            get => entity.GetComponent<Rotation>().value;
            set => entity.GetComponentRef<Rotation>().value = value;
        }

        public readonly bool IsDestroyed => entity.IsDestroyed;

        public Transform(World world, EntityID existingEntity)
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

        public readonly void Dispose()
        {
            entity.Dispose();
        }
    }
}