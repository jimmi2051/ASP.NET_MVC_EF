using System;
using System.Collections.Generic;

namespace SuperMarketMini.Domain
{
    public interface IOrder_DetailRepository
    {
        Order_Detail createOrder_Detail(Order_Detail target);
        Order_Detail updateOrder_Detail(Order_Detail target);
        IEnumerable<Order_Detail> listOrder_DetailByID(String OID);
        Order_Detail getOrder_Detail(String OID,String PID);
        void deleteOrder_Detail(Order_Detail target);
        IEnumerable<Order_Detail> listOrder_Detail();
    }
}
