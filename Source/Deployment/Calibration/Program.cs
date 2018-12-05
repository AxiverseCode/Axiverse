using Axiverse.Services.Proto;
using Grpc.Core;
using System;
using System.Windows.Forms;

namespace Calibration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Channel channel = new Channel("127.0.0.1:32000", ChannelCredentials.Insecure);
            var client = new IdentityService.IdentityServiceClient(channel);
            var response = client.GetIdentity(new GetIdentityRequest());
            Console.WriteLine(response.Value);
            var r2 = client.GetIdentityAsync(new GetIdentityRequest());

            while (!r2.ResponseAsync.IsCompleted)
            {

            }

            response = r2.ResponseAsync.Result;
            Console.WriteLine(response.Value);

            Console.ReadKey();
            channel.ShutdownAsync().Wait();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }
    }
}
