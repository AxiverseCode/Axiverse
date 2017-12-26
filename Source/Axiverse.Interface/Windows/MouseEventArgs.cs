using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class MouseEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MouseButtons Button { get; set; }

        public MouseEventArgs(int x, int y, MouseButtons button)
        {
            X = x;
            Y = y;
            Button = button;
        }
    }
}
