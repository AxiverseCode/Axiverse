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
            var response = new ValidateIdentityResponse();


            return Task.FromResult(response);
        }
    }
}
