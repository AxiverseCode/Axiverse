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

        /// <summary>
        /// Adds two 32 bit numbers together from the stack.
        /// </summary>
        AddI32,

        /// <summary>
        /// Subtracts two 32-bit numbers together from the stack.
        /// </summary>
        SubtractI32,

        MultiplyI32,

        /// <summary>
        /// Branches to target offset. (of what)
        /// 
        /// displacement - offset from the position of the opcode.
        /// </summary>
        Jump,

        JumpIfZero,
        JumpGreaterThanZero,

        JumpCompareEqual,
        JumpCompareNotEqual,

        /// <summary>
        /// Load variable relative to fp.
        /// </summary>
        Load32,
        Load64,

        /// <summary>
        /// Load variable from global memory.
        /// </summary>
        GlobalLoad,

        /// <summary>
        /// Store variable relative to fp.
        /// </summary>
        Store,

        /// <summary>
        /// Store variable into global memory from stack.
        /// </summary>
        GlobalStore,

        /// <summary>
        /// Load a constant variable.
        /// </summary>
        Const,

        /// <summary>
        /// Prints an 32 bit.
        /// </summary>
        Print,
    }
}
