using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.Fibers
{
    public struct Message
    {
        public long Timestamp { get; }
        public Fiber Target { get; }
        public Fiber Sender { get; }
        public object Value { get; }

        public Message(Fiber target, Fiber sender, object value)
        {
            Timestamp = stopwatch.ElapsedTicks;
            Target = target;
            Sender = sender;
            Value = value;
        }

        static Stopwatch stopwatch = new Stopwatch();
        static Message()
        {
            stopwatch.Start();
        }
    }
}
