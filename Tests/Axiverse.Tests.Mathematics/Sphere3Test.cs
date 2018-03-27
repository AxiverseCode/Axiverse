using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Axiverse.Mathematics;

namespace Axiverse.Tests.Mathematics
{
    [TestClass]
    public class Sphere3Test
    {
        [TestMethod]
        public void IntersectsOverlapping()
        {
            Sphere3 value = new Sphere3(new Vector3(0, 0, 0), 1);

            Assert.IsTrue(value.Intersects(value));
        }

        [TestMethod]
        public void IntersectsTouching()
        {
            Sphere3 left = new Sphere3(new Vector3(0, 0, 1), 1);
            Sphere3 right = new Sphere3(new Vector3(0, 0, -1), 1);

            Assert.IsTrue(left.Intersects(right));
        }
    }
}
