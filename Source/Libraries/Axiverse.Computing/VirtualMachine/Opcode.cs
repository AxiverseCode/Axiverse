using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// Virtual machine operation codes.
    /// </summary>
    public enum Opcode : byte
    {
        Nop = 0x00,

        /// <summary>
        /// 
        /// Takes argument count.
        /// </summary>
        Call,

        Return,
        Halt,

        /// <summary>
        /// 
        /// Adds two 32 bit numbers together from the stack.
        /// </summary>
        Add,

        /// <summary>
        /// Load variable relative to fp.
        /// </summary>
        Load,

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

        Print,
    }
}
