using System.Numerics;
using Transforms.Components;
using Unmanaged;
using Worlds;

namespace Transforms
{
    public readonly struct Transform : ITransform
    {
        private readonly Entity entity;

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

        public readonly Vector3 WorldPosition
        {
            get
            {
                return entity.GetComponent<LocalToWorld>().Position;
            }
            set
            {
                Matrix4x4 wtl = Matrix4x4.Identity;
                Entity parent = entity.GetParent();
                if (parent != default)
                {
                    Matrix4x4.Invert(parent.GetComponent(Components.LocalToWorld.Default).value, out wtl);
                }

                LocalPosition = Vector3.Transform(value, wtl);
                //todo: fault: local to world isnt being updated, this should be ok because the design is that
                //ltw updates after an iteration, so perhaps these setters for world should be gone
            }
        }

        public readonly Quaternion WorldRotation
        {
            get
            {
                return entity.GetComponent<WorldRotation>().value;
            }
            set
            {
                Entity parent = entity.GetParent();
                Matrix4x4 wtl = Matrix4x4.Identity;
                if (parent != default)
                {
                    Matrix4x4.Invert(parent.GetComponent(Components.LocalToWorld.Default).value, out wtl);
                }

                LocalRotation = Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(wtl) * value);
            }
        }

        public readonly Vector3 WorldScale => entity.GetComponent<LocalToWorld>().Scale;
        public readonly Vector3 WorldRight => Vector3.Transform(Vector3.UnitX, WorldRotation);
        public readonly Vector3 WorldUp => Vector3.Transform(Vector3.UnitY, WorldRotation);
        public readonly Vector3 WorldForward => Vector3.Transform(Vector3.UnitZ, WorldRotation);
        public readonly Matrix4x4 LocalToWorld => entity.GetComponent<LocalToWorld>().value;

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;

        readonly Definition IEntity.GetDefinition(Schema schema)
        {
            return new Definition().AddComponentTypes<IsTransform, LocalToWorld, Position>(schema);
        }

        public Transform(World world, uint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Transform(World world)
        {
            entity = new Entity<IsTransform, Position, Rotation, Scale, LocalToWorld, WorldRotation>(world, default, Position.Default, Rotation.Default, Scale.Default, Components.LocalToWorld.Default, Components.WorldRotation.Default).AsEntity().As<Transform>();
        }

        public Transform(World world, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            entity = new Entity<IsTransform, Position, Rotation, Scale, LocalToWorld, WorldRotation>(world, default, new Position(position), new Rotation(rotation), new Scale(scale), new LocalToWorld(position, rotation, scale), new WorldRotation(rotation)).AsEntity().As<Transform>();
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

        public static implicit operator Entity(Transform transform)
        {
            return transform.entity;
        }
    }
}