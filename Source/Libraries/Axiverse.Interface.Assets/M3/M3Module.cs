using Axiverse.Injection;
using Axiverse.Modules;
using Axiverse.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Assets.M3
{
    [Dependency(typeof(ResourceModule))]
    public class M3Module : Module
    {
        [Bind]
        protected TypeCache<object> Cache;

        protected override void Initialize()
        {
            var deserializer = Bind<M3Deserializer>();
            Cache.Register(deserializer);
        }
    }
}
