using Transforms.Components;
using Unmanaged;

namespace Transforms.Tests
{
    public class AnchorTypeTests
    {
        [Test]
        public void CheckAnchorDataType()
        {
            Anchor.value left = new(0.92123f, true);
            Assert.That(left.Absolute, Is.True);
            Assert.That(left.Number, Is.EqualTo(0.92123f).Within(0.1f));

            Anchor.value right = new(456f, false);
            Assert.That(right.Absolute, Is.False);
            Assert.That(right.Number, Is.EqualTo(456f).Within(0.1f));

            Assert.That(left, Is.Not.EqualTo(right));
            Anchor.value up = new(0.5f, true);
            Assert.That(up, Is.Not.EqualTo(left));
            Assert.That(up.Absolute, Is.True);
            Assert.That(up.Number, Is.EqualTo(0.5f).Within(0.1f));

            Anchor.value down = new(-3.1412f, false);
            Assert.That(down, Is.Not.EqualTo(right));
            Assert.That(down.Absolute, Is.False);
            Assert.That(down.Number, Is.EqualTo(-3.1412f).Within(0.1f));

            using RandomGenerator rng = new();
            for (uint i = 0; i < 1024; i++)
            {
                float number = rng.NextFloat(-1000f, 1000f);
                bool isNormalized = rng.NextBool();
                Anchor.value value = new(number, isNormalized);
                value.Number *= 2;
                value.Absolute = !value.Absolute;
                Assert.That(value.Number, Is.EqualTo(number * 2).Within(0.1f));
                Assert.That(value.Absolute, Is.EqualTo(!isNormalized));
            }
        }

        [Test]
        public void BuildAnchorValuesFromText()
        {
            Anchor.value a = "0.5f";
            Assert.That(a.Number, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(a.Absolute, Is.True);

            Anchor.value b = "50%";
            Assert.That(b.Number, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(b.Absolute, Is.False);

            Anchor.value c = "-25%";
            Assert.That(c.Number, Is.EqualTo(-0.25f).Within(0.001f));
            Assert.That(c.Absolute, Is.False);

            Anchor.value d = "0";
            Assert.That(d.Number, Is.EqualTo(0f).Within(0.001f));
            Assert.That(d.Absolute, Is.True);
        }
    }
}
