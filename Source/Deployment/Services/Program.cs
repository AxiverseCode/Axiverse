using Axiverse.Injection;
using Axiverse.Modules;
using Axiverse.Services.ChatService;
using Axiverse.Services.EntityService;
using Axiverse.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [Dependency(typeof(ChatServiceModule))]
    [Dependency(typeof(IdentityServiceModule))]
    [Dependency(typeof(EntityServiceModule))]
    public class Program : ProgramModule
    {
        [Inject]
        public Program()
        {

        }

        public override void Execute(string[] args)
        {

        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
