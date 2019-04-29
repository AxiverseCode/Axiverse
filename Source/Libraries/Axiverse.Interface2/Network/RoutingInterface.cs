using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Axiverse.Interface2.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://ithare.com/64-network-dos-and-donts-for-game-engine-developers-part-i-client-side/
    /// </remarks>
    public class RoutingInterface
    {
        public UdpClient Client { get; set; }

        /// <summary>
        /// Gets the queue of events received.
        /// </summary>
        public ConcurrentQueue<object> IncomingQueue { get; } = new ConcurrentQueue<object>();

        /// <summary>
        /// Gets the queue of events received.
        /// </summary>
        public ConcurrentQueue<object> OutgoingQueue { get; } = new ConcurrentQueue<object>();


        public void Run()
        {
            var task = Client.ReceiveAsync();
            task.Wait(10);
            if (task.IsCompleted)
            {
                var result = task.Result;
            }
        }
    }
}
