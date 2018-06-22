using Axiverse.Modules;
using Axiverse.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Engine
{
    [Dependency(typeof(ResourceModule))]
    public class EngineModule : Module
    {
        protected override void Initialize()
        {
            Bind<Engine>();
        }
    }
}
