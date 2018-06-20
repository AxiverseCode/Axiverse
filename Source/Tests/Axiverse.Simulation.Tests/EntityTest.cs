using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    [TestFixture]
    public class EntityTest
    {
        [Test]
        public void ComponentAdded_Raises()
        {
            var entity = new Entity();
            var component = new TestComponent();

            bool raised = false;

            entity.ComponentAdded += (s, e) =>
            {
                raised = true;
                Assert.AreEqual(s, entity);
                Assert.AreEqual(e.Key, Key.From<TestComponent>());
                Assert.AreEqual(e.Component, component);
            };

            entity.Components.Add(component);

            Assert.IsTrue(raised);
        }

        [Test]
        public void ComponentRemoved_Raises()
        {
            var entity = new Entity();
            var component = new TestComponent();
            entity.Components.Add(component);

            bool raised = false;

            entity.ComponentRemoved += (s, e) =>
            {
                raised = true;
                Assert.AreEqual(s, entity);
                Assert.AreEqual(e.Key, Key.From<TestComponent>());
                Assert.AreEqual(e.Component, component);
            };

            var result = entity.Components.Remove<TestComponent>();

            Assert.IsTrue(result);
            Assert.IsTrue(raised);
        }
    }
}
