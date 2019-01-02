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
        ConcurrentDictionary<string, Client> clients;

        public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            var message = new Message();
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
                        };

                        await responseStream.WriteAsync(new ListenResponse
                        {
                            Message = messageProto
                        });
                    }
                    client.Trigger.Reset();
                }
                catch (Exception)
                {
                    connected = false;
                }
            }
            while (connected);
        }
    }
}
