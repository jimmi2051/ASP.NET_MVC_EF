
using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace  SuperMarketMini.Repository
{
     public   class SupplierRepository : ISupplierRepository
    {
        private    SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Supplier createSupplier(Supplier target)
        {
            db.Suppliers.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteSupplier(Supplier target)
        {
            db.Suppliers.Remove(target);
            db.SaveChanges();
        }

        public Supplier getSupplier(string SID)
        {
            return db.Suppliers.Where(c => c.SupplierID.Equals(SID)).FirstOrDefault();
        }

        public IEnumerable<Supplier> listSupplier()
        {
            return db.Suppliers.ToList();
        }

        public Supplier updateSupplier(Supplier target)
        {
            var current = getSupplier(target.SupplierID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
