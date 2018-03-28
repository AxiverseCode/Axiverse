using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    public class SimpleOrder : Order
    {
        public OrderState State { get; private set; }

        public double FilledCost { get; private set; }

        public int Filled { get; private set; }

        public int Open
        {
            get
            {
                if (Filled < Quantity)
                {
                    return Quantity - Filled;
                }
                return 0;
            }
        }

        public SimpleOrder(bool isBuy, double price, int quantity)
        {
            IsBuy = isBuy;
            Price = price;
            Quantity = quantity;

            State = OrderState.New;
            Identifier = ++lastIdentifier;
        }

        public void Fill(int quantity, double cost, int id)
        {
            Filled += quantity;
            FilledCost += cost;

            if (Open == 0)
            {
                State = OrderState.Complete;
            }
        }

        public void Accept()
        {
            if (State == OrderState.New)
            {
                State = OrderState.Accepted;
            }
        }

        public void Cancel()
        {
            if (State == OrderState.New)
            {
                State = OrderState.Cancelled;
            }
        }

        public void Replace(int sizeDelta, double newPrice)
        {
            if (State == OrderState.Accepted)
            {
                Quantity += sizeDelta;
                Price = newPrice;
            }
        }

        public int Identifier { get; private set; }

        private static int lastIdentifier;
    }
}
