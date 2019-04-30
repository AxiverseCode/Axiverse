using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        public bool Running { get; set; }

        /// <summary>
        /// Gets the queue of events received.
        /// </summary>
        public ConcurrentQueue<Packet> IncomingQueue { get; } = new ConcurrentQueue<Packet>();

        /// <summary>
        /// Gets the queue of events received.
        /// </summary>
        public ConcurrentQueue<Packet> OutgoingQueue { get; } = new ConcurrentQueue<Packet>();

        private TaskCompletionSource<bool> CompletionSource;
        
        private void Start()
        {
            Running = true;
        }

        private void Stop()
        {
            Running = false;
        }

        public void Run()
        {
            Task<UdpReceiveResult> receiveResult;

            while (Running)
            {
                receiveResult = Client.ReceiveAsync();

                Task.WaitAny(receiveResult, CompletionSource.Task);

                Packet packet;
                IPEndPoint endpoint = default;
                while(Client.Available != 0)
                {
                    var buffer = Client.Receive(ref endpoint);
                    // Process and quick route each of the data packets.
                }

                while(OutgoingQueue.TryDequeue(out packet))
                {
                    var buffer = packet.ToByteArray();
                    Client.Send(buffer, buffer.Length);
                }
                
            }

            Client.Close();
        }
    }
}
