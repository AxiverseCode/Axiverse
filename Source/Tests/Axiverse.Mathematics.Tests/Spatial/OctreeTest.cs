using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

using Axiverse.Mathematics;
using Axiverse.Mathematics.Spatial;

namespace Axiverse.Mathematics.Spatial
{
    [TestFixture]
    public class OctreeTest
    {
        [Test]
        public void FindsIntersecting()
        {
            // setup
            var octree = new Octree<Spatial3>(Vector3.Zero, 1024);
            var spatial = new Spatial3(Vector3.Zero, 1);
            octree.Add(spatial);

            // test
            var contains = octree.GetIntersecting(new Sphere3(Vector3.Zero, 1));

            // assert
            Assert.IsTrue(contains.Count == 1);
        }

        [Test]
        public void FindsIntersecting2()
        {
            // setup
            var octree = new Octree<Spatial3>(Vector3.Zero, 1024);
            var spatial = new Spatial3(new Vector3(0, 0, 1.9f), 1);
            octree.Add(spatial);

            Assert.IsTrue(spatial.BoundingSphere.Intersects((new Sphere3(Vector3.Zero, 1))));

            // test
            var contains = octree.GetIntersecting(new Sphere3(Vector3.Zero, 1));

            // assert
            // Assert.IsTrue(contains.Count == 1);
        }
    }
}
