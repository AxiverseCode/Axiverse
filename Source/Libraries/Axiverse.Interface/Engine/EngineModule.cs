using Axiverse.Modules;
using Axiverse.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Engine
{
    /// <summary>
    /// Module for engine.
    /// </summary>
    [Dependency(typeof(ResourceModule))]
    public class EngineModule : Module
    {
        /// <inheritdoc />
        protected override void Initialize()
        {
            Bind<Engine>();
        }
    }
}
