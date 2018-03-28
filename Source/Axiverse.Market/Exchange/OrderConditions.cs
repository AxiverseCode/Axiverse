using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    [Flags]
    public enum OrderConditions
    {
        None = 0,
        Inseparable = 1,
        Immediate = 2,
    }
}
