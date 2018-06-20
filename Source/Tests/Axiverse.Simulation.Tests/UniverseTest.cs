using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Axiverse.Simulation
{
    [TestFixture]
    public class UniverseTest
    {
        [Test]
        public void EntityAdded_Raises()
        {
            var universe = new Universe();
            var entity = new Entity();

            bool raised = false;

            universe.EntityAdded += (s, e) =>
            {
                raised = true;
                Assert.AreEqual(s, universe);
                Assert.AreEqual(e.Entity, entity);
            };

            universe.Add(entity);

            Assert.IsTrue(raised);
        }

        [Test]
        public void EntityRemoved_Raises()
        {
            var universe = new Universe();
            var entity = new Entity();
            universe.Add(entity);

            bool raised = false;

            universe.EntityRemoved += (s, e) =>
            {
                raised = true;
                Assert.AreEqual(s, universe);
                Assert.AreEqual(e.Entity, entity);
            };

            universe.Remove(entity);

            Assert.IsTrue(raised);
        }
    }
}
