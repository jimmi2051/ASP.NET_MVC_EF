
using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
    public class Order_DetailRepository : IOrder_DetailRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Order_Detail createOrder_Detail(Order_Detail target)
        {
            db.Order_Detail.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteOrder_Detail(Order_Detail target)
        {
            db.Order_Detail.Remove(target);
            db.SaveChanges();
        }

        public Order_Detail getOrder_Detail(string OID, string PID)
        {
            return db.Order_Detail.Where(c => c.OrderID.Equals(OID)&&c.ProductID.Equals(PID)).FirstOrDefault();
        }

        public IEnumerable<Order_Detail> listOrder_Detail()
        {
            return db.Order_Detail.ToList();
        }

        public IEnumerable<Order_Detail> listOrder_DetailByID(string OID)
        {
            return db.Order_Detail.Where(c => c.OrderID.Equals(OID)).ToList();
        }

        public Order_Detail updateOrder_Detail(Order_Detail target)
        {
            var current = getOrder_Detail(target.OrderID,target.ProductID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
    }
}
