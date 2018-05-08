using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grpc.Core;
using Axiverse.Services.Proto;

namespace Axiverse.Utilities.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            host = "34.204.96.156";

            Channel idc = new Channel(host + ":32000", ChannelCredentials.Insecure);
            var id = new IdentityService.IdentityServiceClient(idc);

            Console.WriteLine("Key:");
            var k = Console.ReadLine();
            Console.WriteLine("Password:");
            var v = Console.ReadLine();

            var validate = id.ValidateIdentity(new ValidateIdentityRequest { Key = k, Passcode = v });
            Console.WriteLine(validate);

            Channel channel = new Channel(host + ":32002", ChannelCredentials.Insecure);
            var client = new ChatService.ChatServiceClient(channel);

            Console.WriteLine("listening");
            using (var call = client.Listen())
            {
                var receiverTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        var message = call.ResponseStream.Current;
                        Console.WriteLine(message);
                    }
                });

                string nm = "";
                do
                {
                    nm = Console.ReadLine();
                    client.SendMessage(new SendMessageRequest { Message = nm });
                }
                while (nm != "");
                call.RequestStream.WriteAsync(new ListenRequest { Command = "end" });
            }

            // axicli identity login
            // axicli chat [channel] [message ...]

            channel.ShutdownAsync().Wait();
        }
    }
}
