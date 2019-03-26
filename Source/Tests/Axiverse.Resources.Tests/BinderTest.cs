using Axiverse.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.Tests
{
    [TestFixture]
    public class BinderTest
    {
        Injector injector;

        [SetUp]
        public void SetUp()
        {
            injector = new Injector();
            injector.Activate = false;

            injector.Bind("public field", "public-field");
            injector.Bind("protected field", "protected-field");
            injector.Bind("private field", "private-field");
            injector.Bind("public property", "public-property");
            injector.Bind("protected property", "protected-property");
            injector.Bind("private property", "private-property");
            injector.Bind("parameter", "parameter");
            injector.Bind("bound-string");
        }

        [Test]
        public void Bind()
        {
            var value = new BindingTester();

            Binder.Bind(ref value, injector);

            Assert.AreEqual("public-field", value.BoundPublicField);
            Assert.AreEqual("protected-field", value.BoundProtectedField);
            Assert.AreEqual("private-field", value.BoundPrivateField);
            Assert.AreEqual("public-property", value.BoundPublicProperty);
            Assert.AreEqual("protected-property", value.BoundProtectedProperty);
            Assert.AreEqual("private-property", value.BoundPrivateProperty);
            Assert.IsNull(value.PublicField);
        }

        [Test]
        public void Activate()
        {
            var value = Binder.Activate<ActivationTester>(injector);

            Assert.IsTrue(value.Constructed);
            Assert.IsNull(value.PublicField);
        }

        public class BindingTester
        {
            public string PublicField;

            [Bind]
            [Named("public field")]
            public string BoundPublicField;

            [Bind]
            [Named("protected field")]
            protected string boundProtectedField;
            public string BoundProtectedField => boundProtectedField;

            [Bind]
            [Named("private field")]
            private string boundPrivateField;
            public string BoundPrivateField => boundPrivateField;

            [Bind]
            [Named("public property")]
            public string BoundPublicProperty { get; set; }

            [Bind]
            [Named("protected property")]
            public string BoundProtectedProperty { get; set; }

            [Bind]
            [Named("private property")]
            public string BoundPrivateProperty { get; set; }
        }

        public class ActivationTester
        {
            public bool Constructed;

            public string PublicField;

            [Bind]
            [Named("public field")]
            public string BoundPublicField;

            [Bind]
            [Named("public property")]
            public string BoundPublicProperty { get; set; }

            [Inject]
            public ActivationTester(string parameter, [Named("parameter")] string namedParameter)
            {
                Constructed = true;

                Assert.AreEqual("bound-string", parameter, "Constructor parameter not bound");
                Assert.AreEqual("parameter", namedParameter, "Constructor named parameter not bound");

                Assert.AreEqual("public-field", BoundPublicField, "Field not bound before constructor");
                Assert.AreEqual("public-property", BoundPublicProperty, "Property not bound before constructor");
            }
        }
    }
}
