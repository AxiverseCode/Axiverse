using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    [TestFixture]
    public class Bounds3Test
    {
        [Test]
        public void TestIntersectsNegative()
        {
            var bounds = new Bounds3(Vector3.Zero, Vector3.One);
            var segment = new Segment3(new Vector3(0, 0, 2.1f), new Vector3(2.1f, 2.1f, 0));

            Assert.That(bounds.Intersects(segment) == false);
        }

        [Test]
        public void TestIntersectsPositive()
        {
            var bounds = new Bounds3(Vector3.Zero, Vector3.One);
            var segment = new Segment3(new Vector3(0, 0, 1.9f), new Vector3(1.9f, 1.9f, 0));

            Assert.That(bounds.Intersects(segment) == true);
        }
    }
}
