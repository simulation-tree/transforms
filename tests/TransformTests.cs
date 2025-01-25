using Types;
using Worlds;
using Worlds.Tests;

namespace Transforms.Tests
{
    public abstract class TransformTests : WorldTests
    {
        static TransformTests()
        {
            TypeRegistry.Load<Transforms.TypeBank>();
            TypeRegistry.Load<Simulation.TypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<Transforms.SchemaBank>();
            return schema;
        }
    }
}
