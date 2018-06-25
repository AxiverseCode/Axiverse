using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Axiverse.Simulation
{
    [TestFixture]
    public class ProcessorTest
    {
        Universe universe;
        Processor processor;

        [SetUp]
        public void SetUpUniverse()
        {
            universe = new Universe();
            processor = new Processor(typeof(TestComponent));
            universe.Add(processor);
        }

        [Test]
        public void Entities_CapturesNewEntitesWithComponents()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            entity1.Components.Add(new TestComponent());
            entity2.Components.Add(new TestComponent());

            universe.Add(entity1);
            universe.Add(entity2);

            Assert.AreEqual(2, processor.Entities.Count);
        }

        [Test]
        public void Entities_CapturesExistingEntitiesWithNewComponents()
        {
            var entity = new Entity();

            universe.Add(entity);
            entity.Components.Add(new TestComponent());

            Assert.AreEqual(1, processor.Entities.Count);
        }

        [Test]
        public void Entities_IgnoresMissingComponents()
        {
            var entity = new Entity();

            universe.Add(entity);

            Assert.AreEqual(0, processor.Entities.Count);
        }
    }
}
