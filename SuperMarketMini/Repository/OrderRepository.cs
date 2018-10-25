
using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Order createOrder(Order target)
        {
            db.Orders.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteOrder(Order target)
        {
            db.Orders.Remove(target);
            db.SaveChanges();
        }

        public Order getOrder(string SID)
        {
            return db.Orders.Where(c => c.OrderID.Equals(SID)).FirstOrDefault();
        }

        public IEnumerable<Order> listOrder()
        {
            return db.Orders.ToList();
        }

        public Order updateOrder(Order target)
        {
            var current = getOrder(target.OrderID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
    }
}
