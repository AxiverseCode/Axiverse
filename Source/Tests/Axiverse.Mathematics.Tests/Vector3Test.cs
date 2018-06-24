using System;
using NUnit.Framework;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// Unit tests for <see cref="Vector3"/>.
    /// </summary>
    [TestFixture]
    public class Vector3Test
    {
        [Test]
        public void TestIndexerSet()
        {
            Vector3 value = new Vector3();

            value[0] = 1;
            value[1] = 2;
            value[2] = 3;

            Assert.AreEqual(value.X, 1);
            Assert.AreEqual(value.Y, 2);
            Assert.AreEqual(value.Z, 3);
        }

        [Test]
        public void TestIndexerGet()
        {
            Vector3 value = new Vector3(1, 2, 3);

            Assert.AreEqual(value[0], 1);
            Assert.AreEqual(value[1], 2);
            Assert.AreEqual(value[2], 3);
        }

        [Test]
        public void TestAdd()
        {
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(10, 20, 30);

            Vector3 s = a + b;
            Vector3 t = Vector3.Add(a, b);
            Vector3.Add(out var u, ref a, ref b);
            Vector3 v = a.Add(b); // mutates a

            Vector3 r = new Vector3(11, 22, 33);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, v);
        }

        [Test]
        public void TestSubtract()
        {
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(10, 20, 30);

            Vector3 s = a - b;
            Vector3 t = Vector3.Subtract(a, b);
            Vector3.Subtract(out var u, ref a, ref b);
            Vector3 v = a.Subtract(b); // mutates a

            Vector3 r = new Vector3(-9, -18, -27);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, v);
        }

        [Test]
        public void TestComponentMultiply()
        {
            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(5, 7, 11);

            Vector3 s = a * b;
            Vector3 t = Vector3.Multiply(a, b);
            Vector3.Multiply(out var u, ref a, ref b);

            Vector3 r = new Vector3(5, 14, 33);
            Assert.AreEqual(r, s, "a * b");
            Assert.AreEqual(r, t, "Multiply(a, b)");
            Assert.AreEqual(r, u, "Multiply(out r, ref a, ref b)");
        }

        [Test]
        public void TestScalarMultiply()
        {
            Vector3 a = new Vector3(1, 2, 3);
            float b = 5;

            Vector3 s = a * b;
            Vector3 t = Vector3.Multiply(a, b);
            Vector3.Multiply(out var u, ref a, ref b);
            Vector3 x = b * a;
            Vector3 y = Vector3.Multiply(b, a);
            Vector3.Multiply(out var z, ref b, ref a);

            Vector3 r = new Vector3(5, 10, 15);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, x);
            Assert.AreEqual(r, y);
            Assert.AreEqual(r, z);
        }

        [Test]
        public void TestComponentDivide()
        {
            Vector3 a = new Vector3(5, 14, 33);
            Vector3 b = new Vector3(5, 7, 11);

            Vector3 s = a / b;
            Vector3 t = Vector3.Divide(a, b);
            Vector3.Divide(out var u, ref a, ref b);

            Vector3 r = new Vector3(1, 2, 3);
            Assert.AreEqual(r, s, "a / b");
            Assert.AreEqual(r, t, "Divide(a, b)");
            Assert.AreEqual(r, u, "Divide(out r, ref a, ref b)");
        }

        [Test]
        public void TestScalarDivide()
        {
            Vector3 a = new Vector3(4, 8, 16);
            float b = 2;
            float c = 64;

            Vector3 s = a / b;
            Vector3 t = Vector3.Divide(a, b);
            Vector3.Divide(out var u, ref a, ref b);
            Vector3 x = c / a;
            Vector3 y = Vector3.Divide(c, a);
            Vector3.Divide(out var z, ref c, ref a);

            Vector3 r0 = new Vector3(2, 4, 8);
            Assert.AreEqual(r0, s);
            Assert.AreEqual(r0, t);
            Assert.AreEqual(r0, u);

            Vector3 r1 = new Vector3(16, 8, 4);
            Assert.AreEqual(r1, x);
            Assert.AreEqual(r1, y);
            Assert.AreEqual(r1, z);
        }

        [Test]
        public void TestCrossProduct()
        {
            Vector3 a = new Vector3(-2, 1, 3);
            Vector3 b = new Vector3(5, 2, -1);

            Vector3 s = a % b;
            Vector3 t = Vector3.Cross(a, b);
            Vector3.Cross(out var u, ref a, ref b);

            Vector3 r = new Vector3(-7, 13, -9);
            Assert.AreEqual(r, s, "a % b");
            Assert.AreEqual(r, t, "Cross(a, b)");
            Assert.AreEqual(r, u, "Cross(out r, ref a, ref b)");
        }

        [Test]
        public void TestDotProduct()
        {
            Vector3 a = new Vector3(3, 4, 6);
            Vector3 b = new Vector3(2, 3, 4);

            float s = a.Dot(b);
            float t = Vector3.Dot(a, b);
            float u = Vector3.Dot(ref a, ref b);

            float r = 42;
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
        }
    }
}