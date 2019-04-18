using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class MouseEventArgs : EventArgs
    {
        public Vector2 Position { get; }
        public Vector2 Movement { get; }

        public MouseEventArgs(Vector2 position, Vector2 movement)
        {
            Position = position;
            Movement = movement;
        }
    }
}
