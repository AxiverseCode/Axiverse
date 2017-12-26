using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public interface IEntityDynamic : IDynamic
    {
        Entity Entity { get; set; }
    }
}
