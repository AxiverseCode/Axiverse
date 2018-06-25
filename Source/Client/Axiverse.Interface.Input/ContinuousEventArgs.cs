using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class ContinuousEventArgs : EventArgs
    {
    }

    public delegate void ContinuousEventHandler(object sender, ContinuousEventArgs args);
}
