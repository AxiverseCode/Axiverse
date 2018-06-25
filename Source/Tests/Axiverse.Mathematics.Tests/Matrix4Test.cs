using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    [TestFixture]
    public class Matrix4Test
    {
        public void Adds()
        {
            Matrix4 a = new Matrix4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 10, 11, 12,
                13, 14, 15, 16);
            Matrix4 b = new Matrix4(
                100, 200, 300, 400,
                500, 600, 700, 800,
                900, 1000, 1100, 1200,
                1300, 1400, 1500, 1600);
            Matrix4 c = new Matrix4(
                101, 202, 303, 404,
                505, 606, 707, 808,
                909, 1010, 1111, 1212,
                1313, 1414, 1515, 1616);

            Assert.AreEqual(c, a + b);
            Assert.AreEqual(c, Matrix4.Add(a, b));
        }
    }
}
