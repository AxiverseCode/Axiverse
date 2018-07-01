using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class RoutedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether the event has been handled.
        /// </summary>
        public bool Handled { get; set; }

        public long Timestamp { get; set; }
    }
}
