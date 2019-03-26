using Axiverse.Injection;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Axiverse.Resources.Tests
{
    [TestFixture]
    public class InjectorTest
    {
        Injector injector;

        [SetUp]
        public void SetUp()
        {
            injector = new Injector();
            injector.Activate = true;
        }

        [Test]
        public void ResolveSucceeds()
        {
            var defaultActivation = injector.Resolve<DefaultActivation>();
            Assert.That(defaultActivation.value == "constructed");

            injector.Bind("bound");
            var attributedActivation = injector.Resolve<AttributedActivation>();
            Assert.That(attributedActivation.value == "bound");

            var boundActivate = injector.Resolve<AttributedActivation>();
            Assert.That(boundActivate.value == "bound");
        }

        [Test]
        public void ResolveFails()
        {
            Assert.Throws<MissingMethodException>(() => injector.Resolve<MissingActivation>());
            Assert.Throws<AmbiguousMatchException>(() => injector.Resolve<ConflictingActivation>());
        }

        class DefaultActivation
        {
            public string value;

            public DefaultActivation()
            {
                value = "constructed";
            }
        }

        class MissingActivation
        {
            public string value;
            
            public MissingActivation(string value)
            {
                this.value = value;
            }
        }

        class AttributedActivation
        {
            public string value;

            [Inject]
            public AttributedActivation(string value)
            {
                this.value = value;
            }
        }

        class BoundActivation
        {
            [Bind]
            public string value;
        }

        class ConflictingActivation
        {
            [Inject]
            public ConflictingActivation(int value)
            {

            }

            [Inject]
            public ConflictingActivation(string value)
            {

            }
        }
    }
}
