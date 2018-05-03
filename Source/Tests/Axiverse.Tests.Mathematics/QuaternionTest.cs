using System;
using NUnit.Framework;

namespace Axiverse.Mathematics.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Quaternion"/>.
    /// </summary>
    [TestFixture]
    public class QuaternionTest
    {
        [Test]
        public void TestAdd()
        {
            Quaternion a = new Quaternion(1, 2, 3, 4);
            Quaternion b = new Quaternion(10, 20, 30, 40);

            Quaternion s = a + b;
            Quaternion t = Quaternion.Add(a, b);
            Quaternion.Add(out var u, ref a, ref b);
            a.Add(b); // mutates a

            Quaternion r = new Quaternion(11, 22, 33, 44);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, a);
        }

        [Test]
        public void TestSubtract()
        {
            Quaternion a = new Quaternion(1, 2, 3, 4);
            Quaternion b = new Quaternion(10, 20, 30, 40);

            Quaternion s = a - b;
            Quaternion t = Quaternion.Subtract(a, b);
            Quaternion.Subtract(out var u, ref a, ref b);
            a.Subtract(b); // mutates a

            Quaternion r = new Quaternion(-9, -18, -27, -36);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, a);
        }

        [Test]
        public void TestMultiply()
        {
            Quaternion a = new Quaternion(2, 5, 4, 3);
            Quaternion b = new Quaternion(5, 3, 1, 4);

            Quaternion s = a * b;
            Quaternion t = Quaternion.Multiply(a, b);
            Quaternion.Multiply(out var u, ref a, ref b);
            a.Multiply(b); // mutates a

            Quaternion r = new Quaternion(16, 47, 0, -17);
            Assert.AreEqual(r, s);
            Assert.AreEqual(r, t);
            Assert.AreEqual(r, u);
            Assert.AreEqual(r, a);
        }

        [Test]
        public void TestConjugate()
        {
            Quaternion a = new Quaternion(1, 2, 3, 4);

            Quaternion s = Quaternion.Conjugate(a);
            Quaternion.Conjugate(out var t, ref a);
            Quaternion u = a.Conjugate();

            Quaternion r = new Quaternion(-1, -2, -3, 4);
            Assert.AreEqual(r, s, "Conjugate(a)");
            Assert.AreEqual(r, t, "Conjugate(out r, ref a)");
            Assert.AreEqual(r, u, "a.Conjugate()");
        }

        public void TestNormalize()
        {
            Quaternion a = new Quaternion(1, 2, 3, 4);

            Quaternion s = a.Normal();

            Quaternion r = new Quaternion();
        }

        //[Test]
        public void TestFromEuler()
        {
            Quaternion a = new Quaternion(2, 3, 4, 1).Normal();

            Matrix3 s = Matrix3.FromQuaternion(a);

            Matrix3 r = new Matrix3(
                -2f / 3, 2f / 15, 11f / 15,
                2f / 3, -1f / 3, 2f / 3,
                1f / 3, 14f / 15, 2f / 15);
            Assert.AreEqual(r, s);
        }
    }
}