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
        ManualResetEvent received = new ManualResetEvent(false);
        ConcurrentDictionary<string, IServerStreamWriter<ListenResponse>> b =  new ConcurrentDictionary<string, IServerStreamWriter<ListenResponse>>();
        // Channels
        // Clients

        public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            Console.WriteLine("received " + request.Message); 
            foreach(var v in b)
            {
                v.Value.WriteAsync(new ListenResponse { Message = new ChatMessage { Message = request.Message } });
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

        public override Task Listen(ListenRequest request, IServerStreamWriter<ListenResponse> responseStream, ServerCallContext context)
        {
            return base.Listen(request, responseStream, context);
        }
    }
}
