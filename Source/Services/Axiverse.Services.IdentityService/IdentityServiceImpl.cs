using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Services.Proto;
using Grpc.Core;

namespace Axiverse.Services.IdentityService
{
    public class IdentityServiceImpl : Proto.IdentityService.IdentityServiceBase
    {
        private ConcurrentDictionary<string, Identity> users = new ConcurrentDictionary<string, Identity>();

        public override Task<ValidateIdentityResponse> ValidateIdentity(ValidateIdentityRequest request, ServerCallContext context)
        {
            Console.WriteLine("Validating Identity");

            if (users.TryGetValue(request.Key, out var identity))
            {
                if (identity.Verify(request.Passcode))
                {
                    // passed, assign session
                    return Task.FromResult(new ValidateIdentityResponse { SessionToken = "pass" });
                }
                else
                {
                    return Task.FromResult(new ValidateIdentityResponse { SessionToken = "invalid" });
                }
            }

            identity = new Identity(request.Key, request.Passcode);
            if (users.TryAdd(request.Key, identity))
            {
                return Task.FromResult(new ValidateIdentityResponse { SessionToken = "created" });
            }

            return Task.FromResult(new ValidateIdentityResponse { SessionToken = "server error" });
        }

        public override Task<GetIdentityResponse> GetIdentity(GetIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetIdentityResponse() { Value = "hello world" });
        }

        public override Task<CreateIdentityResponse> CreateIdentity(CreateIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreateIdentityResponse());
        }

        public override Task<DeleteIdentityResponse> DeleteIdentity(DeleteIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeleteIdentityResponse());
        }
    }
}
