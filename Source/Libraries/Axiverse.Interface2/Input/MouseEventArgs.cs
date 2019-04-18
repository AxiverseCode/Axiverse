using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    public class MouseEventArgs : EventArgs
    {
    }

    public delegate void MouseEventHandler(object sender, MouseEventArgs args);
}
