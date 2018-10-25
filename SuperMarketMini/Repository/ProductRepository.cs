
using System.Collections.Generic;
using System.Linq;
using SuperMarketMini.Domain;
namespace SuperMarketMini.Repository
{
     public   class ProductRepository : IProductRepository
    {
        private SuperMarketMini_Context db = new SuperMarketMini_Context();
        public Product createProduct(Product target)
        {
            db.Products.Add(target);
            db.SaveChanges();
            return target;
        }

        public void deleteProduct(Product target)
        {
            db.Products.Remove(target);
            db.SaveChanges();
        }

        public Product getProduct(string SID)
        {
            return db.Products.Where(c => c.ProductID.Equals(SID)).FirstOrDefault();
        }

        public IEnumerable<Product> listProduct()
        {
            return db.Products.ToList();
        }

        public Product updateProduct(Product target)
        {
            Product current = getProduct(target.ProductID);
            db.Entry(current).CurrentValues.SetValues(target);
            db.SaveChanges();
            return target;
        }
    }
}
