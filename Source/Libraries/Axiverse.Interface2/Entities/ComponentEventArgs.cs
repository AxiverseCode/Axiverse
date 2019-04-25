using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entities
{
    public class ComponentEventArgs : EventArgs
    {
        public Type Binding { get; }
        public Component Component { get; }
        public Entity Entity { get; }

        public ComponentEventArgs(Type binding, Component component)
            : this(binding, component, component.Entity)
        {

        }

        public ComponentEventArgs(Type binding, Component component, Entity entity)
        {
            Binding = binding;
            Entity = entity;
            Component = component;
        }
    }

    public delegate void ComponentEventHandler(object sender, ComponentEventArgs args);
}
