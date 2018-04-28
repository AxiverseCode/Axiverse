using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    public class Order
    {
        public bool IsLimit
        {
            get
            {
                return Price > 0;
            }
        }

        public virtual int Quantity { get; protected set; }
        public virtual bool IsBuy { get; protected set; }

        public bool IsSell
        {
            get
            {
                return !IsBuy;
            }
        }

        public virtual double Price { get; protected set; }
    }
}
