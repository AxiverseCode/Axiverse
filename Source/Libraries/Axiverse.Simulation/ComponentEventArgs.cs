using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    public class ComponentEventArgs : EventArgs
    {
        public Key Key { get; }
        public Component Component { get; }

        public ComponentEventArgs(Key key, Component component)
        {
            Key = key;
            Component = component;
        }
    }

    public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);
}
