using Axiverse.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Identity
{
    public class IdentityModule : Module
    {
        protected override void Initialize()
        {
            Bind(new Registry());
        }
    }
}
