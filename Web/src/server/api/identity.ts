import {ValidateIdentityRequest} from '../../../proto/Admin/IdentityService_pb';
import {IdentityServiceClient} from '../../../proto/Admin/IdentityService_grpc_pb';

import grpc from 'grpc';

function main() {
    let client = new IdentityServiceClient('identity:32000',
                                            grpc.credentials.createInsecure());
    let request = new ValidateIdentityRequest();
    //request.setName(user);

    client.validateIdentity(request, function(err, response) {
        console.log(err);
        console.log('Greeting:', response.getSession());
    });
  }