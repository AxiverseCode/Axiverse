using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Template for a computation model to advance the state of the components of an entity.
    /// </summary>
    public class Model
    {
        // This is similar to a module, but for a specific line of computation.

        // outputs


        // inputs


        // intermediates


        // installs different producers in different slots. Advance attributed keys represent the
        // components in the next step of the entity. The model can output various things of which
        // a client can choose to compute or not compute depending on their needs.
        //
        // current components must be available on the component. I don't know if we want to auto-
        // generate default values. I suppose not.
    }
}
