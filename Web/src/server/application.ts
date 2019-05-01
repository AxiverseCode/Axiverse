import express from "express";
import path from "path";
import api from "./api"
// Create Express server
const app = express();

// Express configuration
app.set("port", process.env.WEB_SERVER_PORT || 8080);
app.set("views", path.join(__dirname, "../views"));
//app.set("view engine", "pug");
//app.use(bodyParser.json());
//app.use(bodyParser.urlencoded({ extended: true }));

app.use(express.static(path.join(__dirname, "static")));
app.use(express.static(path.join(__dirname, "../../dist")));

app.use('/api', api);

/*
import {ValidateIdentityRequest} from '../../proto/Admin/IdentityService_pb';
import {IdentityServiceClient} from '../../proto/Admin/IdentityService_grpc_pb';
import grpc from 'grpc';
import dns from 'dns';

let IDENTITY_SERVICE = process.env.IDENTITY_SERVICE_HOST || 'localhost'

dns.resolve(IDENTITY_SERVICE, (err, records) => {
    console.log(err);
    console.log(records);
    IDENTITY_SERVICE = records[0];
})


app.get("/test", (req, res, next) => {
    console.log('creating client ' + IDENTITY_SERVICE + ':32000');
    let client = new IdentityServiceClient(IDENTITY_SERVICE + ':32000',
                                            grpc.credentials.createInsecure());
    let request = new ValidateIdentityRequest();
    //request.setName(user);

    console.log('sending request');
    client.validateIdentity(request, (err, response) => {
        console.log(err);        
        if (err) {
            res.send("Error " + err);
            return;
        }
        console.log('Greeting:', response.getSession());
        res.send(response.getSession());
    });
});
*/
export default app;
