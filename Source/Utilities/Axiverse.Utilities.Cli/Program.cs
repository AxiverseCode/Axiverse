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
            //host = "34.229.211.196";

            Channel channel = new Channel(host + ":32000", ChannelCredentials.Insecure);


            var id = new IdentityService.IdentityServiceClient(channel);

            Console.WriteLine("Key:");
            var k = Console.ReadLine();
            Console.WriteLine("Password:");
            var v = Console.ReadLine();

            try
            {
                var register = id.CreateIdentity(new CreateIdentityRequest { Email = k, Passcode = v });
            }
            catch (RpcException)
            {
                Console.WriteLine("Already exists.");
            }

            var authorization = id.ValidateIdentity(new ValidateIdentityRequest { Key = k, Passcode = v });
            Console.WriteLine(authorization);
            

            var client = new ChatService.ChatServiceClient(channel);

            Console.WriteLine("listening");
            using (var call = client.Listen(new ListenRequest() { SessionToken = Guid.NewGuid().ToString()}))
            {
                var receiverTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        var message = call.ResponseStream.Current;
                        Console.WriteLine(message);
                    }
                });

                string newMessage;
                do
                {
                    newMessage = Console.ReadLine();
                    client.SendMessage(new SendMessageRequest {
                        Message = newMessage,
                        SessionToken = authorization.SessionToken
                    });
                }
                while (!string.IsNullOrEmpty(newMessage));
            }

            // axicli identity login
            // axicli chat [channel] [message ...]

            channel.ShutdownAsync().Wait();
        }
    }
}
