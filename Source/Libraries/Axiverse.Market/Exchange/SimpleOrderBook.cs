using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Market.Exchange
{
    public class SimpleOrderBook : OrderBook
    {
        public int FillIdentifier { get; private set; }

        protected override void OnAccepted(Order order, int transaction, int quantity)
        {
            var simpleOrder = order as SimpleOrder;

            if (simpleOrder != null)
            {
                simpleOrder.Accept();
            }

            base.OnAccepted(order, transaction, quantity);
        }

        protected override void OnFilled(Order inbound, Order current, int fillQuantity, double crossPrice, bool inboundFilled, bool matchFilled, int transaction)
        {
            var inboundOrder = inbound as SimpleOrder;
            var currentOrder = current as SimpleOrder;
            ++FillIdentifier;

            if (inboundOrder != null && currentOrder != null)
            {
                inboundOrder.Fill(fillQuantity, fillQuantity * crossPrice, FillIdentifier);
                currentOrder.Fill(fillQuantity, fillQuantity * crossPrice, FillIdentifier);
            }

            base.OnFilled(inbound, current, fillQuantity, crossPrice, inboundFilled, matchFilled, transaction);
        }

        protected override void OnCanceled(Order order, int quantity, int transaction)
        {
            var simpleOrder = order as SimpleOrder;

            if (simpleOrder != null)
            {
                simpleOrder.Cancel();
            }

            base.OnCanceled(order, quantity, transaction);
        }

        protected override void OnReplaced(Order order, int open, int deltaSize, double price, int transaction)
        {
            var simpleOrder = order as SimpleOrder;

            if (simpleOrder != null)
            {
                simpleOrder.Replace(deltaSize, price);
            }

            base.OnReplaced(order, open, deltaSize, price, transaction);
        }
    }
}
