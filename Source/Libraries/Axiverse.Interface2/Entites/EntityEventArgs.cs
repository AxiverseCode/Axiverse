using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entites
{
    public class EntityEventArgs : EventArgs
    {
        public Entity Entity { get; }

        public EntityEventArgs(Entity entity)
        {
            Entity = entity;
        }
    }

    public delegate void EntityEventHandler(object sender, EntityEventArgs args);
}
