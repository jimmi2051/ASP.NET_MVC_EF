using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface IOrderRepository
    {
        Order createOrder(Order target);
        Order updateOrder(Order target);
        Order getOrder(String SID);
        void deleteOrder(Order target);
        IEnumerable<Order> listOrder();
    }
}
