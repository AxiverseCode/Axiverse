using System;
using System.Collections.Generic;

namespace Axiverse
{
    /// <summary>
    /// 
    /// </summary>
    public struct Trilean : IComparable, IComparable<Trilean>, IEquatable<Trilean>
    {
        private int m_value;

        private Trilean(int value)
        {
            m_value = value;
        }

        // Overrides

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var value = obj as Trilean?;

            if (value == null)
            {
                throw new ArgumentException("obj is not a Trilean.");
            }

            return CompareTo(value.Value);
        }

        public int CompareTo(Trilean value)
        {
            return m_value.CompareTo(value.m_value);
        }

        public override bool Equals(object obj)
        {
            var value = obj as Trilean?;

            return (value.HasValue) ? value.Value.m_value == m_value : false;
        }

        public bool Equals(Trilean obj)
        {
            var value = obj as Trilean?;

            return (value.HasValue) ? value.Value.m_value == m_value : false;
        }

        public override int GetHashCode()
        {
            return m_value;
        }

        public override string ToString()
        {
            switch (m_value)
            {
                case 1:
                    return "+";
                case 0:
                    return "0";
                case -1:
                    return "-";
                default:
                    throw new InvalidOperationException("Internal values should never be ourside 1, 0, or -1.");
            }
        }

        // Utility

        public static Trilean Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value is null.");
            }

            switch (value)
            {
                case "+":
                    return Positive;
                case "0":
                    return Zero;
                case "-":
                    return Negative;

                default:
                    throw new FormatException("Value is not equivalent to PositiveString, ZeroString, or NegativeString");
            }
        }

        public static bool TryParse(string value, out Trilean result)
        {
            switch (value)
            {
                case "+":
                    result = Positive;
                    return true;
                case "0":
                    result = Zero;
                    return true;
                case "-":
                    result = Negative;
                    return true;

                default:
                    result = Zero;
                    return false;
            }
        }

        // Checks

        public static bool IsPositive(Trilean value)
        {
            // In  | + 0 -
            // Out | + - -

            return value.m_value > 0;
        }

        public static bool IsZero(Trilean value)
        {
            // In  | + 0 -
            // Out | - + -

            return value.m_value == 0;
        }

        public static bool IsNegative(Trilean value)
        {
            // In  | + 0 -
            // Out | - - +

            return value.m_value < 0;
        }

        /// <summary>
        /// Checks if the value is non-zero.
        /// </summary>
        /// <param name="value">Value.</param>
        public static bool IsValued(Trilean value)
        {
            // In  | + 0 -
            // Out | + - +
            return value.m_value != 0;
        }

        /// <summary>
        /// Casts to bool with Zero returning true.
        /// </summary>
        /// <param name="value">Value.</param>
        public static bool Positivize(Trilean value)
        {
            // In  | + 0 -
            // Out | + + -
            return value.m_value >= 0;
        }

        /// <summary>
        /// Casts to bool with Zero returning false.
        /// </summary>
        /// <param name="value">Value.</param>
        public static bool Negativize(Trilean value)
        {
            // In  | + 0 -
            // Out | - - +
            return value.m_value > 0;
        }

        // Unary Operations

        public static Trilean Negate(Trilean value)
        {
            // In  | + 0 -
            // Out | - 0 +
            return new Trilean(-value.m_value);
        }

        // Binary Operations

        public static Trilean Max(Trilean left, Trilean right)
        {
            // Not commutitive, associative

            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | + + + + 0 0 + 0 -

            return new Trilean(Math.Max(left.m_value, right.m_value));
        }

        public static Trilean Min(Trilean left, Trilean right)
        {
            // Not commutitive, associative

            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | + 0 - 0 0 - - - -

            return new Trilean(Math.Min(left.m_value, right.m_value));
        }

        public static Trilean Add(Trilean left, Trilean right)
        {
            // Commutitve

            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | + + 0 + 0 - 0 - -

            return new Trilean(Math.Sign(left.m_value + right.m_value));
        }

        public static Trilean Subtract(Trilean left, Trilean right)
        {
            // Commutitive

            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | 0 + + - 0 + - - 0

            return new Trilean(Math.Sign(left.m_value - right.m_value));
        }

        public static Trilean Multiply(Trilean left, Trilean right)
        {
            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | + 0 - 0 0 0 - 0 +

            return new Trilean(left.m_value * right.m_value);
        }

        public static Trilean Gravitate(Trilean value, Trilean gravity)
        {
            // Not commutitive, associative

            // Value   |  +  +  +  0  0  0  -  -  -
            // Gravity |  +  0  -  +  0  -  +  0  -
            // A (g) B |  +  +  +  +  0  -  -  -  -
            // B (g) A |  +  +  -  +  0  -  +  -  -

            // double  | +2 +2 +2  0  0  0 -2 -2 -2
            // w/ grav | +3 +2 +1 +1  0 -1 -1 -2 -3

            return new Trilean(Math.Sign(value.m_value * 2 + gravity.m_value));
        }

        public static Trilean Shift(Trilean value, Trilean shift)
        {
            // Not commutitive, associative?

            // Value   |  +  +  +  0  0  0  -  -  -
            // Gravity |  +  0  -  +  0  -  +  0  -
            // A (s) B |  +  +  0  0  0  0  0  -  -
            // B (s) A |  +  +  0  +  0  -  0  -  -

            // double  | +2 +2 +2  0  0  0 -2 -2 -2
            // w/ grav | +3 +2 +1 +1  0 -1 -1 -2 -3

            return new Trilean((value.m_value * 2 + shift.m_value) / 2);
        }

        public static Trilean Overflow(Trilean left, Trilean right)
        {
            // Left  | + + + 0 0 0 - - -
            // Right | + 0 - + 0 - + 0 -
            // Out   | + 0 0 0 0 0 0 0 -

            return new Trilean((left.m_value + right.m_value) / 2);
        }

        // Equality

        public static bool operator ==(Trilean left, Trilean right)
        {
            return left.m_value == right.m_value;
        }

        public static bool operator !=(Trilean left, Trilean right)
        {
            return left.m_value != right.m_value;
        }

        public static bool operator true(Trilean value)
        {
            return IsPositive(value);
        }

        public static bool operator false(Trilean value)
        {
            return IsNegative(value);
        }

        // Operators

        public static Trilean operator !(Trilean value)
        {
            return Negate(value);
        }

        public static bool operator ~(Trilean value)
        {
            return IsValued(value);
        }

        public static bool operator +(Trilean value)
        {
            return Positivize(value);
        }

        public static bool operator -(Trilean value)
        {
            return Negativize(value);
        }

        public static Trilean operator ++(Trilean value)
        {
            return new Trilean(Math.Sign(value.m_value + 1));
        }

        public static Trilean operator --(Trilean value)
        {
            return new Trilean(Math.Sign(value.m_value - 1));
        }

        public static Trilean operator |(Trilean left, Trilean right)
        {
            return Max(left, right);
        }

        public static Trilean operator &(Trilean left, Trilean right)
        {
            return Min(left, right);
        }

        public static Trilean operator +(Trilean left, Trilean right)
        {
            return Add(left, right);
        }

        public static Trilean operator -(Trilean left, Trilean right)
        {
            return Subtract(left, right);
        }

        public static Trilean operator *(Trilean left, Trilean right)
        {
            return Multiply(left, right);
        }

        public static Trilean operator /(Trilean left, Trilean right)
        {
            return Shift(left, right);
        }

        public static Trilean operator ^(Trilean left, Trilean right)
        {
            return Gravitate(left, right);
        }

        // Casts

        public static implicit operator Trilean(bool value)
        {
            return (value) ? Positive : Negative;
        }

        public static implicit operator Trilean(bool? value)
        {
            return (value.HasValue) ? value.Value : Zero;
        }

        public static implicit operator int(Trilean value)
        {
            return value.m_value;
        }

        public static explicit operator Trilean(byte value)
        {
            return (value == 0) ? Zero : Positive;
        }

        public static explicit operator Trilean(ushort value)
        {
            return (value == 0) ? Zero : Positive;
        }

        public static explicit operator Trilean(uint value)
        {
            return (value == 0) ? Zero : Positive;
        }

        public static explicit operator Trilean(ulong value)
        {
            return (value == 0) ? Zero : Positive;
        }

        public static explicit operator Trilean(sbyte value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(short value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(int value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(long value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(float value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(double value)
        {
            return new Trilean(Math.Sign(value));
        }

        public static explicit operator Trilean(decimal value)
        {
            return new Trilean(Math.Sign(value));
        }

        // Constants

        public static readonly IList<Trilean> Domain = new Trilean[] { Negative, Zero, Positive };
        public static readonly IList<Trilean> Range = new Trilean[] { Negative, Positive };

        public static readonly IList<Trilean> NotPositive = new Trilean[] { Negative, Zero };
        public static readonly IList<Trilean> NotNegative = new Trilean[] { Zero, Positive };

        public static readonly Trilean Positive = new Trilean(1);
        public static readonly Trilean Zero = new Trilean(0);
        public static readonly Trilean Negative = new Trilean(-1);

        public const string PositiveString = "+";
        public const string ZeroString = "0";
        public const string NegativeString = "-";

    }
}