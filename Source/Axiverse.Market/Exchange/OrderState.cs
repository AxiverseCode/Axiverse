using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    public enum OrderState
    {
        New,
        Accepted,
        Complete,
        Cancelled,
        Rejected
    }
}
