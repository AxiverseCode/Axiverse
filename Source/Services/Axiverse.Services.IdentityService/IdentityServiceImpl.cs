using System;
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
        public override Task<ValidateIdentityResponse> ValidateIdentity(ValidateIdentityRequest request, ServerCallContext context)
        {
            Console.WriteLine("Validating Identity");
            var response = new ValidateIdentityResponse
            {
                Session = "Hello World"
            };

            return Task.FromResult(response);
        }

        public override Task<GetIdentityResponse> GetIdentity(GetIdentityRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetIdentityResponse());
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
