using System.Numerics;
using Transforms.Components;

namespace Transforms.Tests
{
    public class RotationTests
    {
        [Test]
        public void CheckIdentitiyDirection()
        {
            Vector3 desiredDirection = new(0, 0, 1);
            Quaternion rotation = Rotation.FromDirection(desiredDirection).value;
            Assert.That(rotation, Is.EqualTo(Quaternion.Identity));
        }

        [Test]
        public void CreateRotationFromDirection()
        {
            Check(new(0, 0, 1));
            Check(new(0, 0, -1));
            Check(new(1, 0, 0));
            Check(new(-1, 0, 0));
            Check(new(0, 1, 0));
            Check(new(0, -1, 0));

            static void Check(Vector3 desiredDirection)
            {
                Quaternion rotation = Rotation.FromDirection(desiredDirection).value;
                Vector3 actualDirection = Vector3.Transform(Vector3.UnitZ, rotation);
                Assert.That(actualDirection.X, Is.EqualTo(desiredDirection.X).Within(0.001f));
                Assert.That(actualDirection.Y, Is.EqualTo(desiredDirection.Y).Within(0.001f));
                Assert.That(actualDirection.Z, Is.EqualTo(desiredDirection.Z).Within(0.001f));
            }
        }

        [Test]
        public void VerifyDownIsDown()
        {
            Vector3 direction = -Vector3.UnitY;
            Quaternion rotation = Rotation.FromDirection(direction).value;
            Vector3 actualDirection = Vector3.Transform(Vector3.UnitZ, rotation);
            Assert.That(actualDirection.X, Is.EqualTo(direction.X).Within(0.001f));
            Assert.That(actualDirection.Y, Is.EqualTo(direction.Y).Within(0.001f));
            Assert.That(actualDirection.Z, Is.EqualTo(direction.Z).Within(0.001f));
        }
    }
}