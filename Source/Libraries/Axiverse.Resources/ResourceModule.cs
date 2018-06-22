using Axiverse.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class ResourceModule : Module
    {
        protected override void Initialize()
        {
            Bind(new Cache());
            Bind(new Library());
        }
    }
}
