using SuperMarketMini.Domain;
using SuperMarketMini.Servies.Validation;
using SuperMarketMini.Repository;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace SuperMarketMini.Servies
{
    public class ProductServices
    {
        private IValidationDictionary _validationDictionary;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private ISupplierRepository _supplierRepository;
        private IOrderRepository _orderRepository;
        private IOrder_DetailRepository _orderdetailRepository;
        private IMessageRepository _messageRepository;
        private List<Product> _list;
        public ProductServices(IValidationDictionary validationDictionary)
        {
            _validationDictionary = validationDictionary;
            _productRepository = new ProductRepository();
            _categoryRepository = new CategoryRepository();
            _supplierRepository = new SupplierRepository();
            _orderRepository = new OrderRepository();
            _orderdetailRepository = new Order_DetailRepository();
            _messageRepository = new MessageRepository();
            _list = _productRepository.listProduct().ToList();
        }
        public bool ValidateOrderDetail(string pid ,int index)
        {
            _validationDictionary.Clear();
            if(index > _productRepository.getProduct(pid).Quality)
                _validationDictionary.AddError("Quality", "We don't have enough item. ");
            return _validationDictionary.IsValid;
        }
        public Order createNewOrder()
        {
            string newOrderID = "HD001";
            var c = _orderRepository.listOrder().LastOrDefault();
            if (c != null)
            {
                int Num = int.Parse(c.OrderID.Substring(2));
                Num++;
                if (Num < 10)
                {
                    newOrderID = "HD00" + Num.ToString();
                }
                else if (Num < 100)
                {
                    newOrderID = "HD0" + Num.ToString();
                }
                else newOrderID = "HD" + Num.ToString();
            }           
            Order target = new Order();
            target.OrderID = newOrderID;
            target.Status = 1;
            target.OrderDate = DateTime.Now;
            target.RequireDate = DateTime.Now.AddDays(7);
            return target;
        }
        public bool Payment(Order payment, List<Infrastructure.ItemToPayment> ilist)
        {
            float sum = 0;
            foreach (var item in ilist)
            {
                if (!ValidateOrderDetail(item.Product_ID, item.quality))
                    return false;
                Order_Detail target = new Order_Detail();
                target.OrderID = payment.OrderID;
                target.ProductID = item.Product_ID;
                target.Quality = item.quality;
                Product index = _productRepository.getProduct(item.Product_ID);
                if (index.Special == 1)
                    target.Price = item.priceSell - (item.priceSell * index.Discount) / 100;
                else
                    target.Price = item.priceSell;
                _orderdetailRepository.createOrder_Detail(target);
                sum += target.Quality * target.Price;
            }
            payment.Amount = sum;
            payment.Status = 2;
            _orderRepository.updateOrder(payment);
            return true;
        }
        public bool ValidateProduct(Product index)
        {
            _validationDictionary.Clear();
            if (index.PriceBuy < 0)
                _validationDictionary.AddError("PriceBuy", "Price buy isn't valid");
            if (index.PriceSell < 0)
                _validationDictionary.AddError("PriceSell", "Price sell isn't valid");
            if (index.Quality < 0)
                _validationDictionary.AddError("Quality", "Quality isn't valid");
            return _validationDictionary.IsValid;
        }
        public Product CreateNewProduct()
        {
            string newOrderID = "SP001";
            var c = _productRepository.listProduct().LastOrDefault();
            if (c != null)
            {
                int Num = int.Parse(c.ProductID.Substring(2));
                Num++;
                if (Num < 10)
                {
                    newOrderID = "SP00" + Num.ToString();
                }
                else if (Num < 100)
                {
                    newOrderID = "SP0" + Num.ToString();
                }
                else newOrderID = "SP" + Num.ToString();
            }
            Product index = new Product();
            index.ProductID = newOrderID;
 
            index.Product_Hot = 0;
            index.Special = 0;
            index.Discount = 0;
            index.Status = 1;
            return index;
        }
        public bool createProduct(Product target)
        {
            if (!ValidateProduct(target))
                return false;
            try
            {
                target.Created = DateTime.Now;
                target.Modified = DateTime.Now;
                _productRepository.createProduct(target);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool updateProduct(Product target)
        {
            Product index = _productRepository.getProduct(target.ProductID);
            if (String.IsNullOrEmpty(target.Images) && !String.IsNullOrEmpty(index.Images))
            {
                target.Images = index.Images;               
            }
            target.Created = index.Created;
            target.Modified = DateTime.Now;
            _productRepository.updateProduct(target);
            return true;

        }
        public bool deleteProduct(string ID)
        {
            try
            {
                Product target = _productRepository.getProduct(ID);
                _productRepository.deleteProduct(target);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public IEnumerable<Product> searchProduct(String key)
        {
            _list = _productRepository.listProduct().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Product_ID, Product name, Supplier, Category".ToUpper()))
            {
                return _list;
            }
            try
            {
                IEnumerable<Product> result;
                result = _list.FindAll(c => (c.ProductID.ToUpper().Contains(key.ToUpper()) 
                || c.Name.ToUpper().Contains(key.ToUpper()) 
                || c.SupplierID.ToUpper().Contains(key.ToUpper()) 
                || c.Supplier.Name.ToUpper().Contains(key.ToUpper()) 
                || c.CategoryID.ToUpper().Contains(key.ToUpper()) 
                || c.Category.Name.ToUpper().Contains(key.ToUpper()))
                && c.Quality>0
                );
                return result.ToList();
            }
            catch
            {
                return _list;
            }
        }
        public Product getProduct(string id)
        {
            return _productRepository.getProduct(id);
        }
        public IEnumerable<Category> listCategory()
        {
            return _categoryRepository.listCategory().ToList();
        }
        public IEnumerable<Product> listProduct()
        {
            return _productRepository.listProduct().Where(c=>c.Quality>0).ToList();
        }
        public IEnumerable<Supplier> listSupplier()
        {
            return _supplierRepository.listSupplier().ToList();
        }
        public IEnumerable<Product> listRelateProducts(string cid,string sid)
        {
            IEnumerable<Product> result ;
            result = _list.FindAll(c => (c.CategoryID.Equals(cid) || c.SupplierID.Equals(sid))&& c.Quality > 0);
            return result.ToList();
        }
        public IEnumerable<Product> listHotProduct()
        {
            IEnumerable<Product> result;
            result = _list.FindAll(c => c.Product_Hot==1 && c.Quality > 0);
            return result.ToList();
        }
        public IEnumerable<Product> listSpecialProduct()
        {
            IEnumerable<Product> result;
            result = _list.FindAll(c => c.Special == 1 && c.Quality>0);
            return result.ToList();
        }
        public bool createOrder(Order index)
        {
            try
            {
                _orderRepository.createOrder(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public Order getOrder(String PID)
        {
            return _orderRepository.getOrder(PID);
        }
        public IEnumerable<Order_Detail> GetOrder_Details(String OID)
        {
            return _orderdetailRepository.listOrder_DetailByID(OID).ToList();
        }
        public IEnumerable<Product> listProductByKey(string key,string type)
        {
            if (type == "Cat")
                return _productRepository.listProduct().Where(c => c.CategoryID.Contains(key)).ToList();
            if (type == "Name")
                return _productRepository.listProduct().Where(c => c.Name.Contains(key)).ToList();
            if (type == "Sup")
                return _productRepository.listProduct().Where(c => c.SupplierID.Contains(key)).ToList();
            if (type == "Group")
                return _productRepository.listProduct().Where(c => c.Category.GroupName.Contains(key)).ToList();
            return _productRepository.listProduct();
        }
        public IEnumerable<Product> listProductByKey(string key)
        {
            if (key == "Keyword: Name, Category, Supplier")
                return _productRepository.listProduct();
            return _productRepository.listProduct().Where(
                c => c.Category.GroupName.ToUpper().Contains(key.ToUpper()) || 
                c.Name.ToUpper().Contains(key.ToUpper()) || 
                c.Supplier.Name.ToUpper().Contains(key.ToUpper())||
                c.SupplierID.ToUpper().Contains(key.ToUpper())||
                c.CategoryID.ToUpper().Contains(key.ToUpper())||
                c.ProductID.ToUpper().Contains(key.ToUpper())
                ).ToList();
        }
        public IEnumerable<String> listGroupCat()
        {
            List<string> result = new List<string>();
            foreach (var item in _categoryRepository.listCategory())
            {
                int check = 0;
                foreach (var index in result)
                {
                    if (index.Equals(item.GroupName))
                        check = 1;
                }
                if (check == 0)
                    result.Add(item.GroupName);
            }
            return result;
        }
        public Category getCategory(string ID)
        {
            return _categoryRepository.getCategory(ID);
        }    
        public Supplier getSupplier(String ID)
        {
            return _supplierRepository.getSupplier(ID);
        }
        public bool createMess(Message target)
        {
            try
            {
                _messageRepository.createMess(target);
            }
            catch
            {

                return false;
            }
            return true;
        }
        public IEnumerable<Order> listOrderByUser(string ID)
        {
            return _orderRepository.listOrder().Where(c => c.Username.Equals(ID));
        }

    }
}
