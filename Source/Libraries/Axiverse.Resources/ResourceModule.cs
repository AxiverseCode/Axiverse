using Axiverse.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// Module for resource classes.
    /// </summary>
    public sealed class ResourceModule : Module
    {
        /// <summary>
        /// Initializes the module.
        /// </summary>
        protected override void Initialize()
        {
            var library = new Library();
            Bind(new Cache(library));
            Bind(library);
        }
    }
}
