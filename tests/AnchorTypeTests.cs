using Transforms.Components;

namespace Transforms.Tests
{
    public class AnchorTypeTests
    {
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
