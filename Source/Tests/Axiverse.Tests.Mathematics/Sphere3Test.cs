using System;
using NUnit.Framework;
using Axiverse.Mathematics;

namespace Axiverse.Tests.Mathematics
{
    [TestFixture]
    public class Sphere3Test
    {
        [Test]
        public void IntersectsOverlapping()
        {
            Sphere3 value = new Sphere3(new Vector3(0, 0, 0), 1);

            Assert.IsTrue(value.Intersects(value));
        }

        [Test]
        public void IntersectsTouching()
        {
            Sphere3 left = new Sphere3(new Vector3(0, 0, 1), 1);
            Sphere3 right = new Sphere3(new Vector3(0, 0, -1), 1);

            Assert.IsTrue(left.Intersects(right));
        }

        [Test]
        public void Constructs()
        {
            var position = new Vector3(1, 2, 3);
            var radius = 4f;

            var value = new Sphere3(position, radius);

            Assert.AreEqual(value.Position, position);
            Assert.AreEqual(value.Radius, radius);
        }
    }
}
