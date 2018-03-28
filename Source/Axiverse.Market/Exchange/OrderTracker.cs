using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    public class OrderTracker
    {
        public Order Order { get; private set; }

        public OrderTracker(Order order, OrderConditions conditions)
        {
            Order = order;
            Conditions = conditions;

            Open = order.Quantity;
        }

        public void ChangeQuantity(int delta)
        {
            if (delta < 0 && (Open < Math.Abs(delta)))
            {
                throw new ArgumentOutOfRangeException("Order reduction size larger than open quantity.");
            }

            Open += delta;
        }

        public void Fill(int quantity)
        {
            if (quantity > Open)
            {
                throw new ArgumentOutOfRangeException("Fill quantity larger than open quantity.");
            }

            Open -= quantity;
        }

        public bool IsFilled
        {
            get
            {
                return Open == 0;
            }
        }

        public int Filled
        {
            get
            {
                return Order.Quantity - Open;
            }
        }

        public int Open { get; private set; }

        /// <summary>
        /// Gets if this order is marked all or none.
        /// </summary>
        public bool IsInseparable
        {
            get
            {
                return (Conditions & OrderConditions.Inseparable) == OrderConditions.Inseparable;
            }

        }

        /// <summary>
        /// Gets if this order is marked immediate or cancel.
        /// </summary>
        public bool IsImmediate
        {
            get
            {
                return (Conditions & OrderConditions.Immediate) == OrderConditions.Immediate;
            }
        }

        public OrderConditions Conditions { get; private set; }
    }
}
