using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class CollectionModel<T, U> : Model
        where T : CollectionComponent<U>
        where U : T
    {
        // Traversal message

        // Iteration message
    }
}
