using Simulation;
using System.Numerics;
using Transforms.Components;

namespace Transforms
{
    public readonly struct Transform : ITransform
    {
        public readonly Entity entity;

        public readonly Entity Parent
        {
            get => entity.Parent;
            set => entity.Parent = value;
        }

        public readonly ref Vector3 LocalPosition
        {
            get
            {
                if (!entity.ContainsComponent<Position>())
                {
                    entity.AddComponent(Position.Default);
                }

                ref Position component = ref entity.GetComponentRef<Position>();
                return ref component.value;
            }
        }

        public readonly ref Quaternion LocalRotation
        {
            get
            {
                if (!entity.ContainsComponent<Rotation>())
                {
                    entity.AddComponent(Rotation.Default);
                }

                ref Rotation component = ref entity.GetComponentRef<Rotation>();
                return ref component.value;
            }
        }

        public readonly ref Vector3 LocalScale
        {
            get
            {
                if (!entity.ContainsComponent<Scale>())
                {
                    entity.AddComponent(Scale.Default);
                }

                ref Scale component = ref entity.GetComponentRef<Scale>();
                return ref component.value;
            }
        }

        public readonly Vector3 LocalRight => Vector3.Transform(Vector3.UnitX, LocalRotation);
        public readonly Vector3 LocalUp => Vector3.Transform(Vector3.UnitY, LocalRotation);
        public readonly Vector3 LocalForward => Vector3.Transform(Vector3.UnitZ, LocalRotation);

        public readonly Vector3 WorldPosition
        {
            get
            {
                return entity.GetComponentRef<LocalToWorld>().Position;
            }
            set
            {
                Matrix4x4 wtl = Matrix4x4.Identity;
                Entity parent = entity.Parent;
                if (parent != default)
                {
                    Matrix4x4.Invert(parent.GetComponent(Components.LocalToWorld.Default).value, out wtl);
                }

                LocalPosition = Vector3.Transform(value, wtl);
            }
        }

        public readonly Quaternion WorldRotation
        {
            get
            {
                return entity.GetComponentRef<WorldRotation>().value;
            }
            set
            {
                Matrix4x4 wtl = Matrix4x4.Identity;
                Entity parent = entity.Parent;
                if (parent != default)
                {
                    Matrix4x4.Invert(parent.GetComponent(Components.LocalToWorld.Default).value, out wtl);
                }

                LocalRotation = Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(wtl) * value);
            }
        }

        public readonly Vector3 WorldScale => entity.GetComponentRef<LocalToWorld>().Scale;
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);
        public readonly Matrix4x4 LocalToWorld => entity.GetComponentRef<LocalToWorld>().value;

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsTransform>();

        public Transform(World world, uint existingEntity)
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
            entity.AddComponent(Components.LocalToWorld.Default);
        }

        public Transform(World world, Vector3 position, Quaternion rotation, Vector3 scale)
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
    }
}