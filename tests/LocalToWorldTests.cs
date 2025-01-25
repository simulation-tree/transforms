using System;
using System.Numerics;
using Transforms.Components;
using Worlds;

namespace Transforms.Tests
{
    public class LocalToWorldTests : TransformTests
    {
        [Test]
        public void BuildAndDecomposeMatrix()
        {
            Vector3 position = new(1, 2, 3);
            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(0.1f, 0.2f, 0.3f);
            Vector3 direction = Vector3.Transform(Vector3.UnitZ, rotation);
            Vector3 scale = new(4, 5, 6);
            LocalToWorld ltw = new(position, rotation, scale);
            Vector3 actualPosition = ltw.Position;
            Quaternion actualRotation = ltw.Rotation;
            Vector3 actualScale = ltw.Scale;
            Assert.That(actualPosition.X, Is.EqualTo(position.X).Within(0.001f));
            Assert.That(actualPosition.Y, Is.EqualTo(position.Y).Within(0.001f));
            Assert.That(actualPosition.Z, Is.EqualTo(position.Z).Within(0.001f));

            Vector3 actualDirection = Vector3.Transform(Vector3.UnitZ, actualRotation);
            Assert.That(actualDirection.X, Is.EqualTo(direction.X).Within(0.001f));
            Assert.That(actualDirection.Y, Is.EqualTo(direction.Y).Within(0.001f));
            Assert.That(actualDirection.Z, Is.EqualTo(direction.Z).Within(0.001f));
            Assert.That(actualRotation.X, Is.EqualTo(rotation.X).Within(0.001f));
            Assert.That(actualRotation.Y, Is.EqualTo(rotation.Y).Within(0.001f));
            Assert.That(actualRotation.Z, Is.EqualTo(rotation.Z).Within(0.001f));
            Assert.That(actualRotation.W, Is.EqualTo(rotation.W).Within(0.001f));

            Assert.That(actualScale.X, Is.EqualTo(scale.X).Within(0.001f));
            Assert.That(actualScale.Y, Is.EqualTo(scale.Y).Within(0.001f));
            Assert.That(actualScale.Z, Is.EqualTo(scale.Z).Within(0.001f));
        }

        [Test]
        public void BuildMatrixManually()
        {
            Vector3 position = new(1, 2, 3);
            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(0.1f, 0.2f, 0.3f);
            Vector3 scale = new(4, 5, 6);
            Matrix4x4 expected = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
            LocalToWorld ltw = new(position, rotation, scale);
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    Assert.That(ltw.value[r, c], Is.EqualTo(expected[r, c]).Within(0.001f));
                }
            }

            Matrix4x4.Decompose(ltw.value, out Vector3 actualScale, out Quaternion actualRotation, out Vector3 actualPosition);
            Vector3 actualDirection = Vector3.Transform(Vector3.UnitZ, actualRotation);
            Assert.That(actualPosition.X, Is.EqualTo(position.X).Within(0.001f));
            Assert.That(actualPosition.Y, Is.EqualTo(position.Y).Within(0.001f));
            Assert.That(actualPosition.Z, Is.EqualTo(position.Z).Within(0.001f));

            Vector3 direction = Vector3.Transform(Vector3.UnitZ, rotation);
            Assert.That(actualDirection.X, Is.EqualTo(direction.X).Within(0.001f));
            Assert.That(actualDirection.Y, Is.EqualTo(direction.Y).Within(0.001f));
            Assert.That(actualDirection.Z, Is.EqualTo(direction.Z).Within(0.001f));

            Assert.That(actualRotation.X, Is.EqualTo(rotation.X).Within(0.001f));
            Assert.That(actualRotation.Y, Is.EqualTo(rotation.Y).Within(0.001f));
            Assert.That(actualRotation.Z, Is.EqualTo(rotation.Z).Within(0.001f));
            Assert.That(actualRotation.W, Is.EqualTo(rotation.W).Within(0.001f));

            Assert.That(actualScale.X, Is.EqualTo(scale.X).Within(0.001f));
            Assert.That(actualScale.Y, Is.EqualTo(scale.Y).Within(0.001f));
            Assert.That(actualScale.Z, Is.EqualTo(scale.Z).Within(0.001f));
        }

        [Test]
        public void LocalDirectionVectors()
        {
            Quaternion desiredRotation = Quaternion.Identity;
            LocalToWorld ltw = new(Vector3.Zero, desiredRotation, Vector3.One);
            Vector3 forward = ltw.Forward;
            Vector3 up = ltw.Up;
            Vector3 right = ltw.Right;
            Assert.That(forward, Is.EqualTo(Vector3.UnitZ));
            Assert.That(up, Is.EqualTo(Vector3.UnitY));
            Assert.That(right, Is.EqualTo(Vector3.UnitX));

            desiredRotation = Quaternion.CreateFromYawPitchRoll(0.1f, 0.2f, 0.3f);
            ltw = new(Vector3.Zero, desiredRotation, Vector3.One);
            forward = ltw.Forward;
            up = ltw.Up;
            right = ltw.Right;
            Vector3 actualForward = Vector3.Transform(Vector3.UnitZ, desiredRotation);
            Vector3 actualUp = Vector3.Transform(Vector3.UnitY, desiredRotation);
            Vector3 actualRight = Vector3.Transform(Vector3.UnitX, desiredRotation);
            Assert.That(forward.X, Is.EqualTo(actualForward.X).Within(0.001f));
            Assert.That(forward.Y, Is.EqualTo(actualForward.Y).Within(0.001f));
            Assert.That(forward.Z, Is.EqualTo(actualForward.Z).Within(0.001f));
            Assert.That(up.X, Is.EqualTo(actualUp.X).Within(0.001f));
            Assert.That(up.Y, Is.EqualTo(actualUp.Y).Within(0.001f));
            Assert.That(up.Z, Is.EqualTo(actualUp.Z).Within(0.001f));
            Assert.That(right.X, Is.EqualTo(actualRight.X).Within(0.001f));
            Assert.That(right.Y, Is.EqualTo(actualRight.Y).Within(0.001f));
            Assert.That(right.Z, Is.EqualTo(actualRight.Z).Within(0.001f));
        }

        [Test]
        public void SetWorldPositionWithInverseLTW()
        {
            using World world = CreateWorld();
            Transform parentTransform = new(world, new(2f, 4f, -32f), Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI * 0.25f), new(2f, 2f, 2f));
            Transform transform = new(world);
            transform.SetParent(parentTransform);
            transform.LocalPosition = parentTransform.LocalToWorld.TransformInverse(new Vector3(1, 2, 3));
            Vector3 localPosition = transform.LocalPosition;
            LocalToWorld parentLtw = parentTransform.LocalToWorld;
            Vector3 worldPosition = parentLtw.Transform(localPosition);
            Assert.That(worldPosition.X, Is.EqualTo(1f).Within(0.001f));
            Assert.That(worldPosition.Y, Is.EqualTo(2f).Within(0.001f));
            Assert.That(worldPosition.Z, Is.EqualTo(3f).Within(0.001f));
        }

        [Test]
        public void ReadDirectionAfterMoving()
        {
            LocalToWorld ltw = new(Vector3.Zero, Quaternion.Identity, Vector3.One);
            Assert.That(ltw.Position.X, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Position.Y, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Position.Z, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Forward.X, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Forward.Y, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Forward.Z, Is.EqualTo(1f).Within(0.1f));

            ltw = new(new(1, 2, 3), Quaternion.Identity, Vector3.One);
            Assert.That(ltw.Position.X, Is.EqualTo(1f).Within(0.1f));
            Assert.That(ltw.Position.Y, Is.EqualTo(2f).Within(0.1f));
            Assert.That(ltw.Position.Z, Is.EqualTo(3f).Within(0.1f));
            Assert.That(ltw.Forward.X, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Forward.Y, Is.EqualTo(0f).Within(0.1f));
            Assert.That(ltw.Forward.Z, Is.EqualTo(1f).Within(0.1f));
        }

        [Test]
        public void LocalPositionFromWorld()
        {
            LocalToWorld child = new(new Vector3(1, 2, 3), Quaternion.Identity, new Vector3(1, 1, 1));
            LocalToWorld parent = new(new Vector3(5, 0, 0), Quaternion.Identity, new Vector3(2, 3, 2));
            Vector3 localPosition = child.Position;
            Vector3 worldPosition = Vector3.Transform(localPosition, parent.value);
            Assert.That(worldPosition.X, Is.EqualTo(7).Within(0.1f));
            Assert.That(worldPosition.Y, Is.EqualTo(6).Within(0.1f));
            Assert.That(worldPosition.Z, Is.EqualTo(6).Within(0.1f));

            Vector3 desiredWorldPosition = new(1, -2, -2);
            Matrix4x4.Invert(parent.value, out Matrix4x4 invParent);
            Vector3 desiredLocalPosition = Vector3.Transform(desiredWorldPosition, invParent);
            Assert.That(desiredLocalPosition.X, Is.EqualTo(-2f).Within(0.1f));
            Assert.That(desiredLocalPosition.Y, Is.EqualTo(-0.6666f).Within(0.1f));
            Assert.That(desiredLocalPosition.Z, Is.EqualTo(-1f).Within(0.1f));
        }

        [Test]
        public void ConvertEulerToRotation()
        {
            EulerAngles euler = EulerAngles.CreateFromDegrees(0f, 90f, 0f);
            Quaternion a = Quaternion.CreateFromYawPitchRoll(euler.value.Y, euler.value.X, euler.value.Z);
            Quaternion b = euler.AsQuaternion();
            Assert.That(a.X, Is.EqualTo(b.X).Within(0.001f));
            Assert.That(a.Y, Is.EqualTo(b.Y).Within(0.001f));
            Assert.That(a.Z, Is.EqualTo(b.Z).Within(0.001f));
            Assert.That(a.W, Is.EqualTo(b.W).Within(0.001f));
        }
    }
}