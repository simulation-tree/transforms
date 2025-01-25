using Types;
using Unmanaged.Tests;
using Worlds;

namespace Transforms.Tests
{
    public abstract class TransformTests : UnmanagedTests
    {
        static TransformTests()
        {
            TypeRegistry.Load<Transforms.TypeBank>();
            TypeRegistry.Load<Simulation.TypeBank>();
        }

        protected virtual Schema CreateSchema()
        {
            Schema schema = new();
            schema.Load<Transforms.SchemaBank>();
            return schema;
        }

        protected World CreateWorld()
        {
            return new(CreateSchema());
        }
    }
}
