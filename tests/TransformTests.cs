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
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<TransformsSchemaBank>();
            return schema;
        }
    }
}
