using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class DiscreteEventArgs : EventArgs
    {
    }

    public delegate void DiscreteEventHandler(object sender, DiscreteEventArgs e);
}
