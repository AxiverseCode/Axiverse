using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class Universe
    {

        public Universe()
        {

        }

        public void Post(Guid target)
        {

        }

        public void Step()
        {
            foreach(var entity in entities.Values)
            {
                entity.Model?.Process(entity);
            }
        }

        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
    }
}
