using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Services.Proto;
using Grpc.Core;

namespace Axiverse.Services.ChatService
{
    public class ChatServiceImpl : Proto.ChatService.ChatServiceBase
    {
        public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            return base.SendMessage(request, context);
        }

        public override Task Listen(IAsyncStreamReader<ListenRequest> requestStream, IServerStreamWriter<ListenResponse> responseStream, ServerCallContext context)
        {
            return base.Listen(requestStream, responseStream, context);
        }
    }
}
