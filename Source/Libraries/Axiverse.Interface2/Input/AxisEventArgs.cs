using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    public class AxisEventArgs : EventArgs
    {
        public float Value { get; set; }

        public Bounds Bounds { get; set; }

        public bool Resolution { get; set; }
    }

    public delegate void AxisEventHandler(object sender, MouseEventArgs args);
}
