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

        public override async Task Listen(IAsyncStreamReader<ListenRequest> requestStream, IServerStreamWriter<ListenResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("new sub");

            string id = Guid.NewGuid().ToString();
            b.AddOrUpdate(id, responseStream, (k, v) => v);

            await responseStream.WriteAsync(new ListenResponse { Message = new ChatMessage { Message = "Hello World" } });
            while (await requestStream.MoveNext())
            {
                if (requestStream.Current.Command == "end")
                {
                    break;
                }
            }

            b.TryRemove(id, out var vv);
            Console.WriteLine("exit sub");
        }
    }
}
