namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// Virtual machine operation codes.
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Java_bytecode_instruction_listings
    /// https://en.wikipedia.org/wiki/List_of_CIL_instructions
    /// </remarks>
    public enum Opcode : byte
    {
        /// <summary>
        /// No operation.
        /// </summary>
        Nop = 0x00,

        /// <summary>
        /// Calls a method.
        /// 
        /// displacement [2] - relative address of the call to the opcode.
        /// </summary>
        Call16,

        /// <summary>
        /// Calls a method.
        /// 
        /// displacement [4] - relative address of the call to the opcode.
        /// </summary>
        Call32,

        /// <summary>
        /// Returns without parameters.
        /// </summary>
        Return,

        /// <summary>
        /// Returns a 32 bit value from the stack.
        /// </summary>
        Return32,

        /// <summary>
        /// Returns a 64 bit value from the stack.
        /// </summary>
        Return64,

        /// <summary>
        /// Halts the program.
        /// </summary>
        Halt,

        Complement32,
        Not32,

        NegateI32,
        NegateF32,

        And32,
        Or32,
        Xor32,

        LeftShiftI32,
        LeftShiftU32,
        RightShiftI32,
        RightShiftU32,

        /// <summary>
        /// Adds two 32 bit numbers together from the stack.
        /// </summary>
        AddI32,
        AddF32,

        /// <summary>
        /// Subtracts two 32-bit numbers together from the stack.
        /// </summary>
        SubtractI32,
        SubtractF32,

        MultiplyI32,
        MultiplyF32,

        DivideI32,
        DivideF32,

        CastI32ToI64,
        CastI32ToU32,
        CastI32ToU64,
        CastI32ToF32,
        CastI32ToF64,

        CastU32ToF32,

        CastF32ToI32,
        CastF32ToU32,

        /// <summary>
        /// Branches to target offset. (of what)
        /// 
        /// displacement - offset from the position of the opcode.
        /// </summary>
        Jump,

        JumpIfZeroI32,
        JumpIfPositiveI32,
        JumpIfNegativeI32,

        JumpCompareEqualI32,
        JumpCompareNotEqualI32,
        JumpCompareGreaterI32,
        JumpCompareLesserI32,
        JumpCompareGreaterOrEqualI32,
        JumpCompareLesserOrEqualI32,

        /// <summary>
        /// Load variable relative to fp.
        /// </summary>
        LocalLoad32,
        LocalLoad64,

        /// <summary>
        /// Load variable from global memory.
        /// </summary>
        GlobalLoad32,

        /// <summary>
        /// Store variable relative to fp.
        /// </summary>
        LocalStore32,

        /// <summary>
        /// Store variable into global memory from stack.
        /// </summary>
        GlobalStore32,

        /// <summary>
        /// Load a constant variable.
        /// </summary>
        Const32,
        Const64,

        /// <summary>
        /// Prints an 32 bit.
        /// </summary>
        Print,
    }
}
