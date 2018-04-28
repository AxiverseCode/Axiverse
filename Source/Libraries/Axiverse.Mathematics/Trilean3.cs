using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Trilean3
    {
        public Trilean X;

        public Trilean Y;

        public Trilean Z;

        public Trilean3(Trilean x, Trilean y, Trilean z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Trilean3(int x, int y, int z)
        {
            X = (Trilean)x;
            Y = (Trilean)y;
            Z = (Trilean)z;
        }

        public static Boolean3 IsValued(Trilean3 value)
        {
            return new Boolean3(
                Trilean.IsValued(value.X),
                Trilean.IsValued(value.Y),
                Trilean.IsValued(value.Z)
                );
        }

        public static Trilean3 Overflow(Trilean3 left, Trilean3 right)
        {
            return new Trilean3(
                Trilean.Overflow(left.X, right.X),
                Trilean.Overflow(left.Y, right.Y),
                Trilean.Overflow(left.Z, right.Z)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Trilean3(Boolean3 value)
        {
            return new Trilean3((Trilean)value.X, (Trilean)value.Y, (Trilean)value.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly Trilean3[] Cardinals = new Trilean3[]
        {
            new Trilean3(Trilean.Positive, Trilean.Zero, Trilean.Zero),
            new Trilean3(Trilean.Negative, Trilean.Zero, Trilean.Zero),
            new Trilean3(Trilean.Zero, Trilean.Positive, Trilean.Zero),
            new Trilean3(Trilean.Zero, Trilean.Negative, Trilean.Zero),
            new Trilean3(Trilean.Zero, Trilean.Zero, Trilean.Positive),
            new Trilean3(Trilean.Zero, Trilean.Zero, Trilean.Negative),
        };
    }
}
