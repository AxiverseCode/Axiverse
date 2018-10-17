using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Axiverse
{
    /// <summary>
    /// System floating point manipulation.
    /// 
    /// https://msdn.microsoft.com/en-us/library/c9676k6h.aspx
    /// </summary>
    public static class FloatingPoint
    {
        const uint EM_DENORMAL = 0x00080000;
        const uint EM_INVALID = 0x00000010;
        const uint EM_ZERODIVIDE = 0x00000008;
        const uint EM_OVERFLOW = 0x00000004;
        const uint EM_UNDERFLOW = 0x00000002;
        const uint EM_INEXACT = 0x00000001;
        const uint MCW_EM = 0x0008001f; // From float.h

        const uint EM_DEFAULT = EM_INVALID | EM_ZERODIVIDE | EM_OVERFLOW | EM_UNDERFLOW;

        [DllImport("msvcrt.dll")]
        static extern uint _controlfp(uint a, uint b);

        [DllImport("msvcrt.dll")]
        static extern uint _clearfp();

        // Clearing a bit sets the interrupts.
        static uint cw;
        static uint Control
        {
            get { return cw; }
            set
            {
                _controlfp(cw, MCW_EM);
                cw = value;
            }
        }

        public static bool ThrowOnDenormal
        {
            get => (Control & EM_DENORMAL) == 0;
            set => Control = (value) ? (Control & ~EM_DENORMAL) : (Control | EM_DENORMAL);
        }

        /// <summary>
        /// Gets or sets whether to throw on an invalid operation. This is when a calculation is
        /// mathematically underfined and would return NaN by default.
        /// </summary>
        public static bool ThrowOnInvalidOperation
        {
            get => (Control & EM_INVALID) == 0;
            set => Control = (value) ? (Control & ~EM_INVALID) : (Control | EM_INVALID);
        }

        /// <summary>
        /// Gets or sets whether to throw on a divide by zero operation or any finite operation
        /// which gives an exact infinite result. For example 1/0 or log(0). Would return ±infinity
        /// by default.
        /// </summary>
        public static bool ThrowOnDivideByZero
        {
            get => (Control & EM_ZERODIVIDE) == 0;
            set => Control = (value) ? (Control & ~EM_ZERODIVIDE) : (Control | EM_ZERODIVIDE);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number too large to be
        /// represented. Would return ±infinity by default.
        /// </summary>
        public static bool ThrowOnOverflow
        {
            get => (Control & EM_OVERFLOW) == 0;
            set => Control = (value) ? (Control & ~EM_OVERFLOW) : (Control | EM_OVERFLOW);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number too small to be
        /// represented. Would return 0 or a subnormal by default.
        /// </summary>
        public static bool ThrowOnUnderflow
        {
            get => (Control & EM_UNDERFLOW) == 0;
            set => Control = (value) ? (Control & ~EM_UNDERFLOW) : (Control | EM_UNDERFLOW);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number which is not
        /// representable exactly. Returns the correctly rounded number by default.
        /// </summary>
        public static bool ThrowOnInexact
        {
            get => (Control & EM_INEXACT) == 0;
            set => Control = (value) ? (Control & ~EM_INEXACT) : (Control | EM_INEXACT);
        }

        public static bool ThrowOnSevere
        {
            get => (Control & EM_DEFAULT) == EM_DEFAULT;
            set => Control = (value) ? (Control & ~EM_DEFAULT) : (Control | EM_DEFAULT);
        }

        /// <summary>
        /// Clears the interrupt flag.
        /// </summary>
        public static void ClearInterrupt()
        {
            _clearfp();
        }

        static FloatingPoint()
        {
            cw = _controlfp(0, 0);
        }
    }
}
