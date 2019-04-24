using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entites
{
    public class ComponentEventArgs : EventArgs
    {
        public Component Component { get; }
        public Entity Entity { get; }

        public ComponentEventArgs(Component component) : this(component.Entity, component)
        {

        }

        public ComponentEventArgs(Entity entity, Component component)
        {
            Entity = entity;
            Component = component;
        }
    }

    public delegate void ComponentEventHandler(object sender, ComponentEventArgs args);
}
