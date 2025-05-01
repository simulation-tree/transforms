using Transforms.Components;
using Unmanaged;

namespace Transforms.Tests
{
    public class AnchorTypeTests
    {
        [Test]
        public void CheckAnchorDataType()
        {
            Anchor.Number left = new(0.92123f, true);
            Assert.That(left.IsAbsolute, Is.True);
            Assert.That(left.Value, Is.EqualTo(0.92123f).Within(0.1f));

            Anchor.Number right = new(456f, false);
            Assert.That(right.IsAbsolute, Is.False);
            Assert.That(right.Value, Is.EqualTo(456f).Within(0.1f));

            Assert.That(left, Is.Not.EqualTo(right));
            Anchor.Number up = new(0.5f, true);
            Assert.That(up, Is.Not.EqualTo(left));
            Assert.That(up.IsAbsolute, Is.True);
            Assert.That(up.Value, Is.EqualTo(0.5f).Within(0.1f));

            Anchor.Number down = new(-3.1412f, false);
            Assert.That(down, Is.Not.EqualTo(right));
            Assert.That(down.IsAbsolute, Is.False);
            Assert.That(down.Value, Is.EqualTo(-3.1412f).Within(0.1f));

            using RandomGenerator rng = new();
            for (uint i = 0; i < 1024; i++)
            {
                float number = rng.NextFloat(-1000f, 1000f);
                bool isNormalized = rng.NextBool();
                Anchor.Number value = new(number, isNormalized);
                value.Value *= 2;
                value.IsAbsolute = !value.IsAbsolute;
                Assert.That(value.Value, Is.EqualTo(number * 2).Within(0.1f));
                Assert.That(value.IsAbsolute, Is.EqualTo(!isNormalized));
            }
        }

        [Test]
        public void BuildAnchorValuesFromText()
        {
            Anchor.Number a = "0.5f";
            Assert.That(a.Value, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(a.IsAbsolute, Is.True);

            Anchor.Number b = "50%";
            Assert.That(b.Value, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(b.IsAbsolute, Is.False);

            Anchor.Number c = "-25%";
            Assert.That(c.Value, Is.EqualTo(-0.25f).Within(0.001f));
            Assert.That(c.IsAbsolute, Is.False);

            Anchor.Number d = "0";
            Assert.That(d.Value, Is.EqualTo(0f).Within(0.001f));
            Assert.That(d.IsAbsolute, Is.True);
        }
    }
}
