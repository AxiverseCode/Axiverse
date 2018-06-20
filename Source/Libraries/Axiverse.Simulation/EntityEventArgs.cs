using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class EntityEventArgs : EventArgs
    {
        public Entity Entity { get; }

        public EntityEventArgs(Entity entity)
        {
            Entity = entity;
        }
    }

    public delegate void EntityEventHandler(object sender, EntityEventArgs e);
}
