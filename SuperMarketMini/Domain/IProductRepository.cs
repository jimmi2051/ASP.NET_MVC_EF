using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Domain
{
    public interface IProductRepository
    {
        Product createProduct(Product target);
        Product updateProduct(Product target);
        Product getProduct(String SID);
        void deleteProduct(Product target);
        IEnumerable<Product> listProduct();
    }
}
