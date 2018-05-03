using System;
using NUnit.Framework;

namespace Axiverse.Mathematics.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Vector4"/>.
    /// </summary>
    [TestFixture]
    public class Vector4Test
    {
        [Test]
        public void TestAdd()
        {
            Vector4 a = new Vector4(1, 2, 3, 4);
            Vector4 b = new Vector4(10, 20, 30, 40);

            Vector4 s = a + b;
            Vector4 t = Vector4.Add(a, b);
            Vector4.Add(out var u, ref a, ref b);
            Vector4 v = a.Add(b); // mutates a

            Vector4 r = new Vector4(11, 22, 33, 44);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, v);
        }

        [Test]
        public void TestSubtract()
        {
            Vector4 a = new Vector4(1, 2, 3, 4);
            Vector4 b = new Vector4(10, 20, 30, 40);

            Vector4 s = a - b;
            Vector4 t = Vector4.Subtract(a, b);
            Vector4.Subtract(out var u, ref a, ref b);
            Vector4 v = a.Subtract(b); // mutates a

            Vector4 r = new Vector4(-9, -18, -27, -36);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, v);
        }
    }
}