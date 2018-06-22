namespace Axiverse.Mathematics
{
    /// <summary>
    /// Represents a three-dimensional trilean.
    /// </summary>
    public struct Trilean3
    {
        /// <summary>
        /// Gets or set the X component.
        /// </summary>
        public Trilean X;

        /// <summary>
        /// Gets or sets the Y component.
        /// </summary>
        public Trilean Y;

        /// <summary>
        /// Gets or sets the Z component.
        /// </summary>
        public Trilean Z;

        /// <summary>
        /// Constructs a three dimensional trilen with the given components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Trilean3(Trilean x, Trilean y, Trilean z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Constructs a three dimensional trilen with the given components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Trilean3(int x, int y, int z)
        {
            X = (Trilean)x;
            Y = (Trilean)y;
            Z = (Trilean)z;
        }

        /// <summary>
        /// Determines if the trileans are valued (not ambiguous).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean3 IsValued(Trilean3 value)
        {
            return new Boolean3(
                Trilean.IsValued(value.X),
                Trilean.IsValued(value.Y),
                Trilean.IsValued(value.Z)
                );
        }

        /// <summary>
        /// Computes the overflow operator of the trilean.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Trilean3 Overflow(Trilean3 left, Trilean3 right)
        {
            return new Trilean3(
                Trilean.Overflow(left.X, right.X),
                Trilean.Overflow(left.Y, right.Y),
                Trilean.Overflow(left.Z, right.Z)
                );
        }

        /// <summary>
        /// Converts this trilean into a vector with the values -1, 0, or 1.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// Converts a three dimensional boolean into a three dimensional trilean.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Trilean3(Boolean3 value)
        {
            return new Trilean3((Trilean)value.X, (Trilean)value.Y, (Trilean)value.Z);
        }

        /// <summary>
        /// An array of the cardinals of trileans.
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
