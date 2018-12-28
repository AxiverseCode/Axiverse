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
        const uint EM_DEFAULT = EM_INVALID | EM_ZERODIVIDE | EM_OVERFLOW | EM_UNDERFLOW;
        const uint EM_INVALID = 0x00000010;
        const uint EM_ZERODIVIDE = 0x00000008;
        const uint EM_OVERFLOW = 0x00000004;
        const uint EM_UNDERFLOW = 0x00000002;
        const uint EM_INEXACT = 0x00000001;
        const uint MCW_EM = 0x0008001F;

        const uint RC_CHOP = 0x00000300;
        const uint RC_UP = 0x00000200;
        const uint RC_DOWN = 0x00000100;
        const uint RC_NEAR = 0x00000000;
        const uint MCW_RC = 0x00000300;

        /// <summary>
        /// Gets and sets the floating-point control word.
        /// 
        /// https://docs.microsoft.com/en-us/cpp/c-runtime-library/reference/control87-controlfp-control87-2
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern uint _controlfp(uint value, uint mask);

        /// <summary>
        /// Gets and clears the floating-point status word.
        /// 
        /// https://docs.microsoft.com/en-us/cpp/c-runtime-library/reference/clear87-clearfp
        /// </summary>
        /// <returns></returns>
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern uint _clearfp();

        // Clearing a bit sets the interrupts.
        static uint controlFlags;
        static uint InterruptFlags
        {
            get { return controlFlags; }
            set
            {
                controlFlags = _controlfp(controlFlags, MCW_EM);
            }
        }

        /// <summary>
        /// Gets or sets whether to throw on an invalid operation. This is when a calculation is
        /// mathematically underfined and would return NaN by default.
        /// </summary>
        public static bool ThrowOnInvalidOperation
        {
            get => (InterruptFlags & EM_INVALID) == 0;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_INVALID) : (InterruptFlags | EM_INVALID);
        }

        /// <summary>
        /// Gets or sets whether to throw on a divide by zero operation or any finite operation
        /// which gives an exact infinite result. For example 1/0 or log(0). Would return ±infinity
        /// by default.
        /// </summary>
        public static bool ThrowOnDivideByZero
        {
            get => (InterruptFlags & EM_ZERODIVIDE) == 0;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_ZERODIVIDE) : (InterruptFlags | EM_ZERODIVIDE);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number too large to be
        /// represented. Would return ±infinity by default.
        /// </summary>
        public static bool ThrowOnOverflow
        {
            get => (InterruptFlags & EM_OVERFLOW) == 0;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_OVERFLOW) : (InterruptFlags | EM_OVERFLOW);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number too small to be
        /// represented. Would return 0 or a subnormal by default.
        /// </summary>
        public static bool ThrowOnUnderflow
        {
            get => (InterruptFlags & EM_UNDERFLOW) == 0;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_UNDERFLOW) : (InterruptFlags | EM_UNDERFLOW);
        }

        /// <summary>
        /// Gets or sets whether to throw when an operation results in a number which is not
        /// representable exactly. Returns the correctly rounded number by default.
        /// </summary>
        public static bool ThrowOnInexact
        {
            get => (InterruptFlags & EM_INEXACT) == 0;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_INEXACT) : (InterruptFlags | EM_INEXACT);
        }

        /// <summary>
        /// Gets or sets whether to throw on severe errors including invalid, overflow, underflow,
        /// and divide by zero.
        /// </summary>
        public static bool ThrowOnSevere
        {
            get => (InterruptFlags & EM_DEFAULT) == EM_DEFAULT;
            set => InterruptFlags = (value) ? (InterruptFlags & ~EM_DEFAULT) : (InterruptFlags | EM_DEFAULT);
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
            controlFlags = _controlfp(0, 0);
        }
    }
}
