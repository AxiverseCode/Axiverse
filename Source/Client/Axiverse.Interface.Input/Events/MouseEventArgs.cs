using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class MouseEventArgs : RoutedEventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float DeltaX { get; set; }
        public float DeltaY { get; set; }
        public float DeltaZ { get; set; }

        public bool Buttons { get; set; }
    }
}
