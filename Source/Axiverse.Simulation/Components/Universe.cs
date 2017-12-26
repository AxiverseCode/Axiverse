using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class Universe
    {
        protected void OnEntityAdded()
        {

        }

        protected void OnEntityRemoved()
        {

        }

        protected void OnModelAdded()
        {

        }

        protected void OnModelRemoved()
        {

        }

        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
    }
}
