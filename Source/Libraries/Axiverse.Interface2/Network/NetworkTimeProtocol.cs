using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Network
{
    public static class NetworkTimeProtocol
    {
        private static Stopwatch watch = new Stopwatch();

        public static ulong NtpFrequency = 0x1_0000_0000L;

        private struct Sample
        {
            public DateTime time;
            public long ticks;
        }

        public struct Sync
        {
            public long Before;
            public long After;
            public ulong Network;
        }

        static NetworkTimeProtocol()
        {
            watch.Start();
        }

        public static DateTime GetNetworkTime()
        {
            // https://insights.sei.cmu.edu/sei_blog/2017/04/best-practices-for-ntp-services.html
            // https://stackoverflow.com/questions/15911624/getting-time-from-a-ntp-server-using-c-sharp-in-windows-8-app

            //default Windows time server
            string ntpServer = "time.windows.com";
            ntpServer = "pool.ntp.org";

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //The UDP port number assigned to NTP is 123
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //NTP uses UDP

            var sync = new Sync();

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);

                //Stops code hang if NTP is blocked
                socket.ReceiveTimeout = 3000;

                sync.Before = watch.ElapsedTicks;
                socket.Send(ntpData);
                socket.Receive(ntpData);
                sync.After = watch.ElapsedTicks;
                socket.Close();
            }

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            sync.Network = intPart << 32 | fractPart;
            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / NtpFrequency);

            //**UTC** time
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            Console.WriteLine($"Window {(sync.After - sync.Before) / (Stopwatch.Frequency / 1000 / 1000) / 1000f} ms");

            return networkDateTime.ToLocalTime();
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        public static void Test()
        {
            for (int i = 0; i < 20; i++)
            {
                var b = DateTime.Now;
                var n = GetNetworkTime();
                var l = DateTime.Now;
                var bb = b.Second * 1000 + b.Millisecond;
                var nn = n.Second * 1000 + n.Millisecond;
                var ll = l.Second * 1000 + l.Millisecond;
                Console.WriteLine($"Average: {(nn - bb + nn - ll) / 2} [{nn - bb}, {nn - ll}]");
            }
        }
    }
}
