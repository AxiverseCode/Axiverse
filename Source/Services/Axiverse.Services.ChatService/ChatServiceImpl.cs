using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Axiverse.Services.Proto;
using Grpc.Core;

namespace Axiverse.Services.ChatService
{
    public class ChatServiceImpl : Proto.ChatService.ChatServiceBase
    {
        ConcurrentDictionary<string, Client> clients = new ConcurrentDictionary<string, Client>();

        public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            var message = new Message();
            message.Text = request.Message;
            
            foreach (var client in clients.Values)
            {
                client.Queue.Enqueue(message);
                client.Trigger.Set();
            }

            return Task.FromResult(new SendMessageResponse());
        }

        public override Task<JoinChannelResponse> JoinChannel(JoinChannelRequest request, ServerCallContext context)
        {
            return base.JoinChannel(request, context);
        }

        public override Task<LeaveChannelResponse> LeaveChannel(LeaveChannelRequest request, ServerCallContext context)
        {
            return base.LeaveChannel(request, context);
        }

        public override async Task Listen(ListenRequest request, IServerStreamWriter<ListenResponse> responseStream, ServerCallContext context)
        {
            var client = new Client();
            client.Session = request.SessionToken;
            clients.TryAdd(client.Session, client);

            var connected = true;
            do
            {
                try
                {
                    await client.Trigger.WaitAsync();
                    while (client.Queue.TryDequeue(out var message))
                    {
                        var messageProto = new ChatMessage
                        {
                            Message = message.Text,
                            Channel = " ",
                        };

                        await responseStream.WriteAsync(new ListenResponse
                        {
                            Message = messageProto
                        });
                    }
                    client.Trigger.Reset();
                }
                catch (Exception e)
                {
                    connected = false;
                }
            }
            while (connected);

            clients.TryRemove(client.Session, out client);
        }
    }
}
