using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderList = System.Collections.Generic.SortedList<double, Axiverse.Market.Exchange.OrderTracker>;

namespace Axiverse.Market.Exchange
{
    public class OrderBook
    {

        public virtual bool Add(Order order, OrderConditions conditions = OrderConditions.None)
        {
            ++Transaction;

            bool matched = false;

            if (!ValidateOrder(order, conditions))
            {
                // rejected
            }
            else
            {

                double price = SortPrice(order);
                OrderTracker tracker = new OrderTracker(order, conditions);

                matched = AddOrder(tracker, price);
                if (matched)
                {
                    // add tracker.Filled to OnAccepted
                    OnAccepted(order, Transaction, tracker.Filled);
                }
                else
                {
                    OnAccepted(order, Transaction, 0);
                }

                if (tracker.IsImmediate && ! tracker.IsFilled)
                {
                    // cancel order, transaction
                    OnCanceled(order, 0, Transaction);
                }

                // update this, transaction
                OnBookUpdated(Transaction);
            }

            return matched;
        }

        public virtual void Cancel(Order order)
        {
            ++Transaction;

            bool found = false;
            int open = 0;

            if (order.IsBuy)
            {
                int index = FindBid(order);

                if (index != -1)
                {
                    open = Bids.Values[index].Open;
                    Bids.RemoveAt(index);

                    found = true;
                }
            }
            else
            {
                int index = FindAsk(order);
                
                if (index != -1)
                {
                    open = Asks.Values[index].Open;
                    Asks.RemoveAt(index);

                    found = true;
                }
            }

            if (found)
            {
                OnCanceled(order, open, Transaction);
                OnBookUpdated(Transaction);

            }
            else
            {
                OnCancelRejected(order, "Order not found.", Transaction);
            }
        }

        public static readonly double PriceUnchanged = 0;

        public virtual void Replace(Order order, int deltaSize, double newPrice)
        {
            ++Transaction;

            bool matched = false;
            bool found = false;
            bool priceChanged = newPrice != 0 && newPrice != order.Price;
            double price = (newPrice == PriceUnchanged) ? order.Price : newPrice;

            if (order.IsBuy)
            {
                int index = FindBid(order);

                if (index != -1)
                {
                    found = true;

                    if (ValidateReplace(Bids.Values[index], deltaSize, price))
                    {
                        OrderTracker tracker = Bids.Values[index];

                        OnReplaced(order, tracker.Open, deltaSize, price, Transaction);
                        OnBookUpdated(Transaction);

                        int newOpen = tracker.Open + deltaSize;
                        tracker.ChangeQuantity(newOpen);

                        if (newOpen == 0)
                        {
                            OnCanceled(order, 0, Transaction);
                            Bids.RemoveAt(index);
                        }
                        else
                        {
                            matched = AddOrder(tracker, price);
                            Bids.RemoveAt(index);
                        }
                    }
                }
            }
            else
            {
                int index = FindAsk(order);

                if (index != -1)
                {
                    found = true;

                    if (ValidateReplace(Asks.Values[index], deltaSize, price))
                    {
                        OrderTracker tracker = Asks.Values[index];

                        OnReplaced(order, tracker.Open, deltaSize, price, Transaction);
                        OnBookUpdated(Transaction);

                        int newOpen = tracker.Open + deltaSize;
                        tracker.ChangeQuantity(newOpen);

                        if (newOpen == 0)
                        {
                            OnCanceled(order, 0, Transaction);
                            Asks.RemoveAt(index);
                        }
                        else
                        {
                            matched = AddOrder(tracker, price);
                            Asks.RemoveAt(index);
                        }
                    }
                }
            }

            if (!found)
            {
                OnReplaceRejected(order, "Order not found.", Transaction);
            }
        }

        public OrderList Asks { get; private set; }
        public OrderList Bids { get; private set; }


        public OrderBook()
        {
            Asks = new OrderList(Comparer<double>.Create((x, y) => x.CompareTo(y)));
            Bids = new OrderList(Comparer<double>.Create((x, y) => -x.CompareTo(y)));

            Transaction = 0;

            DeferredBidCrosses = new List<int>();
            DeferredAskCrosses = new List<int>();
        }


        
        public event Action Order;
        public event Action Trade;


        /// <summary>
        /// Matches a new ask order to current bids.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="price"></param>
        /// <param name="bids"></param>
        /// <returns></returns>
        protected virtual bool MatchAskOrder(OrderTracker order, double price, OrderList bids)
        {
            bool matched = false;
            int matchedQuantity = 0;
            int orderQuantity = order.Open;

            for (int i = 0; i < bids.Count; i++)
            {
                if (Matches(order, price, order.Open - matchedQuantity, bids.Values[i], bids.Keys[i], false))
                {
                    if (order.IsInseparable)
                    {
                        matchedQuantity += bids.Values[i].Open;

                        if (matchedQuantity >= orderQuantity)
                        {
                            matched = true;

                            // unwind deferred crosses
                            int adjustment = 0;
                            for (int j = 0; j < DeferredBidCrosses.Count; j++)
                            {
                                int index = DeferredBidCrosses[j] - adjustment;

                                CrossOrders(order, bids.Values[index]);

                                if (bids.Values[index].IsFilled)
                                {
                                    bids.RemoveAt(index);
                                    ++adjustment;

                                    DeferredBidCrosses.RemoveAt(j--);
                                }
                                else
                                {
                                    DeferredBidCrosses[j] = index;
                                }
                            }
                        }
                        else
                        {
                            DeferredBidCrosses.Add(i);
                        }
                    }
                    else
                    {
                        matched = true;
                    }

                    if (matched)
                    {
                        CrossOrders(order, bids.Values[i]);

                        if(bids.Values[i].IsFilled)
                        {
                            bids.RemoveAt(i--);
                        }

                        if (order.IsFilled)
                        {
                            break;
                        }
                    }
                }
                else if (bids.Keys[i] < price)
                {
                    break;
                }
            }

            return matched;
        }

        /// <summary>
        /// Matches a new bid order to current asks.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="price"></param>
        /// <param name="asks"></param>
        /// <returns></returns>
        protected virtual bool MatchBidOrder(OrderTracker order, double price, OrderList asks)
        {
            bool matched = false;
            int matchedQuantity = 0;
            int orderQuantity = order.Open;

            for (int i = 0; i < asks.Count; i++)
            {
                if (Matches(order, price, order.Open - matchedQuantity, asks.Values[i], asks.Keys[i], true))
                {
                    if (order.IsInseparable)
                    {
                        matchedQuantity += asks.Values[i].Open;

                        if (matchedQuantity >= orderQuantity)
                        {
                            matched = true;

                            // unwind deferred crosses
                            int adjustment = 0;
                            for (int j = 0; j < DeferredAskCrosses.Count; j++)
                            {
                                int index = DeferredAskCrosses[j] - adjustment;

                                CrossOrders(order, asks.Values[index]);

                                if (asks.Values[index].IsFilled)
                                {
                                    asks.RemoveAt(index);
                                    ++adjustment;

                                    DeferredAskCrosses.RemoveAt(j--);
                                }
                                else
                                {
                                    DeferredAskCrosses[j] = index;
                                }
                            }
                        }
                        else
                        {
                            DeferredAskCrosses.Add(i);
                        }
                    }
                    else
                    {
                        matched = true;
                    }

                    if (matched)
                    {
                        CrossOrders(order, asks.Values[i]);

                        if (asks.Values[i].IsFilled)
                        {
                            asks.RemoveAt(i--);
                        }

                        if (order.IsFilled)
                        {
                            break;
                        }
                    }
                }
                else if (asks.Keys[i] > price)
                {
                    break;
                }
            }

            return matched;
        }

        public static readonly double MarketOrderPrice = 0;
        public static readonly double MarketOrderBidSortPrice = -1;
        public static readonly double MarketOrderAskSortPrice = 0;

        protected void CrossOrders(OrderTracker inbound, OrderTracker current)
        {
            int fillQuantity = Math.Min(inbound.Open, current.Open);
            double crossPrice = current.Order.Price;

            if (MarketOrderPrice == crossPrice)
            {
                crossPrice = inbound.Order.Price;
            }

            inbound.Fill(fillQuantity);
            current.Fill(fillQuantity);

            bool inboundFilled = inbound.IsFilled;
            bool matchedFilled = current.IsFilled;

            OnFilled(inbound.Order, current.Order, fillQuantity, crossPrice, inboundFilled, matchedFilled, Transaction);
        }


        protected virtual bool ValidateOrder(Order order, OrderConditions conditions)
        {
            if (order.Quantity == 0)
            {
                OnRejected(order, "Quantity must be positive", Transaction);
                return false;
            }
            return true;
        }


        protected virtual bool ValidateReplace(OrderTracker order, int deltaSize, double price)
        {
            bool sizeDecreased = deltaSize < 0;

            if (sizeDecreased && (order.Open < Math.Abs(deltaSize)))
            {
                OnReplaceRejected(order.Order, "Insufficient open quantity", Transaction);
                return false;
            }
            return true;
        }

        protected int FindBid(Order order)
        {
            double searchPrice = SortPrice(order);
            int index = Bids.IndexOfKey(searchPrice);

            do
            {
                if (Bids.Values[index].Order == order)
                {
                    return index;
                }
            }
            while (Bids.Keys[++index] == searchPrice);

            return -1;
        }

        protected int FindAsk(Order order)
        {
            double searchPrice = SortPrice(order);
            int index = Asks.IndexOfKey(searchPrice);

            do
            {
                if (Asks.Values[index].Order == order)
                {
                    return index;
                }
            }
            while (Asks.Keys[++index] == searchPrice);

            return -1;
        }

        public int Transaction { get; private set; }

        protected virtual bool Matches(OrderTracker inbound, double inboundPrice, int inboundOpen, OrderTracker current, double currentPrice, bool isInboundBuy)
        {
            if (isInboundBuy)
            {
                if (inboundPrice < currentPrice)
                {
                    return false;
                }
            }
            else
            {
                if (inboundPrice > currentPrice)
                {
                    return false;
                }
            }

            if (current.IsInseparable)
            {
                if (current.Open > inboundOpen)
                {
                    return false;
                }
            }

            return true;
        }

        private List<int> DeferredBidCrosses { get; set; }
        private List<int> DeferredAskCrosses { get; set; }

        private double SortPrice(Order order)
        {
            double resultPrice = order.Price;

            if (MarketOrderPrice == resultPrice)
            {
                resultPrice = (order.IsBuy) ? MarketOrderBidSortPrice : MarketOrderAskSortPrice;
            }

            return resultPrice;
        }

        private bool AddOrder(OrderTracker tracker, double price)
        {
            bool matched = false;
            Order order = tracker.Order;

            if (order.IsBuy)
            {
                matched = MatchBidOrder(tracker, price, Asks);
            }
            else
            {
                matched = MatchAskOrder(tracker, price, Bids);
            }

            if (tracker.Open != 0 && !tracker.IsImmediate)
            {
                if (order.IsBuy)
                {
                    Bids.Add(price, tracker);
                }
                else
                {
                    Asks.Add(price, tracker);
                }
            }

            return matched;
        }

        protected virtual void OnCanceled(Order order, int quantity, int transaction)
        {

        }

        protected virtual void OnBookUpdated(int transaction)
        {

        }

        protected virtual void OnCancelRejected(Order order, string reason, int transaction)
        {

        }

        protected virtual void OnAccepted(Order order, int transaction, int quantity)
        {

        }

        protected virtual void OnReplaced(Order order, int open, int deltaSize, double price, int transaction)
        {

        }

        protected virtual void OnFilled(Order inbound, Order current, int fillQuantity, double crossPrice, bool inboundFilled, bool matchFilled, int transaction)
        {

        }
        protected virtual void OnRejected(Order order, string message, int transaction)
        {

        }
        protected virtual void OnReplaceRejected(Order order, string message, int transaction)
        {

        }
    }
}
