﻿using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Resources;
using Axiverse.Services.Proto;
using Grpc.Core;
using System;
using System.Windows.Forms;

namespace Calibration
{
    [Dependency(typeof(ResourceModule))]
    [Dependency(typeof(EngineModule))]
    public class Program : ProgramModule
    {
        [Bind]
        Engine engine;

        [Inject]
        public Program(Library library)
        {
            library.BasePath = @"..\..\..\..\..\";
        }

        public override void Execute(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:32000", ChannelCredentials.Insecure);
            var client = new IdentityService.IdentityServiceClient(channel);
            var connecting = channel.ConnectAsync();
            connecting.Wait(1000);
            if (connecting.IsFaulted || !connecting.IsCompleted)
            {
                Console.ReadKey();
            }


            var response = client.GetIdentity(new GetIdentityRequest());
            Console.WriteLine(response.Value);

            engine.Initialize();
            engine.Run();

            channel.ShutdownAsync().Wait();
        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
