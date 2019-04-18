using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    public class KeyboardEventArgs : EventArgs
    {
    }

    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs args);
}
