using Simulation;
using Types;
using Worlds;
using Worlds.Tests;

namespace Transforms.Tests
{
    public abstract class TransformTests : WorldTests
    {
        static TransformTests()
        {
            MetadataRegistry.Load<TransformsMetadataBank>();
            MetadataRegistry.Load<SimulationMetadataBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<TransformsSchemaBank>();
            schema.Load<SimulationSchemaBank>();
            return schema;
        }
    }
}
