using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.Fibers
{
    public abstract class Fiber
    {
        public bool HasState { get; }
        public FiberPool Pool { get; internal set; }
        public FiberThread Thread { get; internal set; }

        public Fiber(bool hasState)
        {
            HasState = hasState;
        }

        protected void Send(Fiber target, object message)
        {
            Pool.Post(new Message(target, this, message));
        }

        public abstract void Process(Message message);

        protected internal void OnMessageUndeliverable()
        {

        }
    }
}