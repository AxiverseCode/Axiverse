using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.Fibers
{
    public class FiberThread
    {
        public FiberPool Pool { get; }
        public readonly ConcurrentQueue<Message> MessageQueue = new ConcurrentQueue<Message>();
        public int Identifier { get; }
        public Fiber Fiber { get; internal set; }

        public FiberThread(FiberPool pool, int identifier)
        {
            Pool = pool;
            Identifier = identifier;
        }

        public void Run()
        {
            Message message;
            while (true)
            {
                while (MessageQueue.TryDequeue(out message))
                {
                    message.Target.Process(message);
                }

                message = Pool.Dispatch(this);
                message.Target.Pool = Pool;
                message.Target.Thread = this;
                message.Target.Process(message);
            }
        }
    }
}
