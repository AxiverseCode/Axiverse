using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class ButtonEventArgs : RoutedEventArgs
    {
        public bool Pressed { get; set; }

        public int Value { get; set; }

        public int Offset { get; set; }
    }
}
