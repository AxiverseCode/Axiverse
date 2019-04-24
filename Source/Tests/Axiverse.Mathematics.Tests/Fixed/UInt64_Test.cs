using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Axiverse.Mathematics.Fixed
{
    [TestFixture]
    public class UInt64_Test
    {
        [Test]
        public void ConvertUint64()
        {
            Assert.AreEqual((ulong)new UInt64_(ulong.MaxValue), ulong.MaxValue);
        }

        [Test]
        public void Add()
        {
            var int32Max = (UInt64_)uint.MaxValue;
            UInt64_ result;

            UInt64_.Add(ref int32Max, ref int32Max, out result);
            Assert.AreEqual((ulong)uint.MaxValue + uint.MaxValue, (ulong)result);
        }

        [Test]
        public void Subtract()
        {
            var a = (UInt64_)((ulong)uint.MaxValue + 10);
            var b = (UInt64_)uint.MaxValue;
            UInt64_ result;

            UInt64_.Subtract(ref a, ref b, out result);
            Assert.AreEqual(10, (ulong)result);
        }

        [Test]
        public void Multiply()
        {
            var int32Max = (UInt64_)uint.MaxValue;
            var two = (UInt64_)2;
            UInt64_ result;

            UInt64_.Multiply(ref int32Max, 2, out result);
            Assert.AreEqual((ulong)uint.MaxValue * 2, (ulong)result);
        }

        [Test]
        public void Divide()
        {
            var top = (UInt64_)(2L * uint.MaxValue);
            UInt64_ result;

            UInt64_.Divide(ref top, uint.MaxValue, out result);
            Assert.AreEqual(2, (ulong)result);
        }
    }
}
