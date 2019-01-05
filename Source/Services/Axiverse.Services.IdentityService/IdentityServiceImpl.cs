using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Identity;
using Axiverse.Services.Proto;
using Grpc.Core;

namespace Axiverse.Services.IdentityService
{
    public class IdentityServiceImpl : Proto.IdentityService.IdentityServiceBase
    {
        private readonly Registry registry;

        public IdentityServiceImpl(Registry registry)
        {
            this.registry = registry;
        }

        public override async Task<ValidateIdentityResponse> ValidateIdentity(ValidateIdentityRequest request, ServerCallContext context)
        {
            var authorization = registry.Authorize(request.Key, request.Passcode);
            if (authorization != null)
            {
                return new ValidateIdentityResponse { SessionToken = authorization.Key };
            }
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Key and password pair invalid."));
        }

        public override Task<GetIdentityResponse> GetIdentity(GetIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetIdentityResponse() { Value = "hello world" });
        }

        public override Task<CreateIdentityResponse> CreateIdentity(CreateIdentityRequest request, ServerCallContext context)
        {
            if (registry.Register(request.Email, request.Passcode) == null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Already exists."));

            }
            return Task.FromResult(new CreateIdentityResponse());
        }

        public override Task<DeleteIdentityResponse> DeleteIdentity(DeleteIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeleteIdentityResponse());
        }
    }
}
