using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Axiverse.Threading;

namespace Axiverse.Services.ChatService
{
    public class Client
    {
        public AsyncManualResetEvent Trigger { get; } = new AsyncManualResetEvent();
        public ConcurrentQueue<Message> Queue { get; } = new ConcurrentQueue<Message>();
        public string Session { get; set; }
    }
}
