using Simulation;
using Simulation.Components;
using Transforms.Components;
using Types;
using Unmanaged.Tests;
using Worlds;

namespace Transforms.Tests
{
    public abstract class TransformTests : UnmanagedTests
    {
        protected World world;
        protected Simulator simulator;

        static TransformTests()
        {
            TypeLayout.Register<IsProgram>();
            TypeLayout.Register<IsTransform>();
            TypeLayout.Register<Position>();
            TypeLayout.Register<Rotation>();
            TypeLayout.Register<WorldRotation>();
            TypeLayout.Register<Scale>();
            TypeLayout.Register<Anchor>();
            TypeLayout.Register<Pivot>();
            TypeLayout.Register<LocalToWorld>();
        }

        protected override void SetUp()
        {
            base.SetUp();
            world = new();
            world.Schema.RegisterTag<IsTransform>();
            world.Schema.RegisterComponent<IsProgram>();
            world.Schema.RegisterComponent<Position>();
            world.Schema.RegisterComponent<Rotation>();
            world.Schema.RegisterComponent<WorldRotation>();
            world.Schema.RegisterComponent<Scale>();
            world.Schema.RegisterComponent<Anchor>();
            world.Schema.RegisterComponent<Pivot>();
            world.Schema.RegisterComponent<LocalToWorld>();
            simulator = new(world);
        }

        protected override void TearDown()
        {
            simulator.Dispose();
            world.Dispose();
            base.TearDown();
        }
    }
}
