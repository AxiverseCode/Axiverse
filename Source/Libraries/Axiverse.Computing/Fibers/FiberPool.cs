using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Axiverse.Computing.Fibers
{
    public class FiberPool
    {
        internal readonly ConcurrentDictionary<Fiber, FiberThread> FiberThreadAffinities = 
            new ConcurrentDictionary<Fiber, FiberThread>();
        internal readonly ConcurrentQueue<Message> MessageQueue = new ConcurrentQueue<Message>();

        public FiberPool()
        {

        }

        public void Start()
        {
            
        }

        public void PostMessage(Fiber target, object message)
        {
            MessageQueue.Enqueue(new Message(target, null, message));
        }

        public void Post(Message message)
        {
            if (message.Target.HasState)
            {
                // If the fiber has state and is being handled by a thread, enqueue the message there.
                if (FiberThreadAffinities.TryGetValue(message.Target, out var thread)) {
                    thread.MessageQueue.Enqueue(message);
                    return;
                }
            }

            MessageQueue.Enqueue(message);
        }

        public Message Dispatch(FiberThread thread)
        {
            Message message; 

            if (thread.Fiber != null)
            {
                if (!FiberThreadAffinities.TryRemove(thread.Fiber, out var value) || value != thread) {
                    throw new InvalidOperationException();
                }
            }

            while (MessageQueue.TryDequeue(out message))
            {
                if (!FiberThreadAffinities.TryGetValue(message.Target, out var affinity))
                {
                    break;
                }
                affinity.MessageQueue.Enqueue(message);
            }

            if (FiberThreadAffinities.TryAdd(message.Target, thread))
            {
                thread.Fiber = message.Target;
                return message;
            }
            // TODO: Figure out how to manage last minute affinity changes.
            throw new InvalidOperationException();
        }

        private static int FindCores()
        {
            int count = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                count += int.Parse(item["NumberOfCores"].ToString());
            }
            return count;
        }
    }
}
