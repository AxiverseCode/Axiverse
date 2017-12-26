using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Defines methods of a dynamic model.
    /// </summary>
    public interface IDynamic
    {
        /// <summary>
        /// Increments the dynamic model by the delta.
        /// </summary>
        /// <param name="delta">The amount of change.</param>
        void Step(float delta);
    }
}
