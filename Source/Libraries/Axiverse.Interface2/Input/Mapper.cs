using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    /// <summary>
    /// Command mapping for hotkeys. Takes the types events from the bridge and maps it to more generic ones.
    /// We also want easy mapping for default.
    /// </summary>
    public class Mapper
    {
        public Bridge Bridge { get; }

        public Mapper(Bridge bridge)
        {
            Bridge = bridge;
        }
    }
}
