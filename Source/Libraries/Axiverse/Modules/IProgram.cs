using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{
    /// <summary>
    /// Interface for the primary program loop.
    /// </summary>
    public interface IProgram
    {
        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args"></param>
        void Run(string[] args);
    }
}
