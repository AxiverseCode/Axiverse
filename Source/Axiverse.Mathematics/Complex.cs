using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Axiverse.Mathematics
{
    /// <summary>
    /// Represents a 2-dimensional Cartesian vector
    /// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Complex
    {
        #region Members

        /// <summary> Gets or sets the real component of the complex number.</summary>
        public float A;

        /// <summary> Gets or sets the imaginary component of the complex number.</summary>
        public float B;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component at the given index.
        /// </summary>
        /// <param name="index">The index of the component.</param>
        /// <returns></returns>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return A;
                    case 1: return B;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: A = value; break;
                    case 1: B = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a complex number from the given real component.
        /// </summary>
        /// <param name="a">The real component.</param>
        public Complex(float a)
        {
            A = a;
        }

        /// <summary>
        /// Constructs a complex number from the given real and imaginary components.
        /// </summary>
        /// <param name="a">The real component.</param>
        /// <param name="b">The imaginary component.</param>
        public Complex(float a, float b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Constructs a complex number from a two dimensional vector where x is the real component
        /// and y is the imaginary component.
        /// </summary>
        /// <param name="value"></param>
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Complex(Vector2 value)
        {
            A = value.X;
            B = value.Y;
        }

        #endregion

        public readonly static Complex Zero = new Complex(0f, 0f);
        public readonly static Complex One = new Complex(1f);
        public readonly static Complex ImaginaryOne = new Complex(0f, 1f);
        public readonly static Complex NaN = new Complex(float.NaN, float.NaN);
    }
}
