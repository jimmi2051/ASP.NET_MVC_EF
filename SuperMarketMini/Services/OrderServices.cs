using SuperMarketMini.Domain;
using SuperMarketMini.Repository;
using SuperMarketMini.Servies.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Servies
{
    public class OrderServices
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IValidationDictionary _validationDictionary;
        private IOrder_DetailRepository _orderdetailRepository;
        private IReceipt_NoteRepository _receiptnoteRepository;
        private IReceipt_Note_DetailRepository _receiptnotedetailRepository;
        public OrderServices(IValidationDictionary validationDictionary)
        {
            _validationDictionary = validationDictionary;
            _orderRepository = new OrderRepository();
            _orderdetailRepository = new Order_DetailRepository();
            _receiptnoteRepository = new Receipt_NoteRepository();
            _receiptnotedetailRepository = new Receipt_Note_DetailRepository();
            _productRepository = new ProductRepository();

        }
        public IEnumerable<Order_Detail> listOrderDetailsByID(String ID)
        {
            return _orderdetailRepository.listOrder_DetailByID(ID);
        }
        public bool updateOrder(Order target)
        {
            try
            {
                _orderRepository.updateOrder(target);
                if(target.Status == 3)
                {
                    List<Order_Detail> _list = _orderdetailRepository.listOrder_DetailByID(target.OrderID).ToList();
                    foreach (var item in _list)
                    {
                        Product index = _productRepository.getProduct(item.ProductID);
                        index.Quality -= item.Quality;
                        _productRepository.updateProduct(index);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
 
        public bool deleteOrder(Order target)
        {
            try
            {
                _orderRepository.deleteOrder(target);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public IEnumerable<Order> listOrder()
        {
            return _orderRepository.listOrder();
        }
        public IEnumerable<Order> searchOrder(String key)
        {
            List<Order> _list = _orderRepository.listOrder().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Order, Customer, Date".ToUpper()))
            {
                return _list;
            }
            try
            {
                IEnumerable<Order> result;
                result = _list.FindAll(c => c.OrderID.ToUpper().Contains(key.ToUpper()) || c.User.DisplayName.ToUpper().Contains(key.ToUpper()) || c.Username.ToUpper().Contains(key.ToUpper()) || c.OrderDate.ToString().ToUpper().Contains(key.ToUpper()));
                return result.ToList();
            }
            catch
            {
                return _list;
            }
        }
        public Order getOrder(string ID)
        {
            return _orderRepository.getOrder(ID);
        }
        public IEnumerable<Receipt_Note> searchReceipt(String key)
        {
            List<Receipt_Note> _list = _receiptnoteRepository.listReceipt_Note().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            if (key.ToUpper().Equals("Key: Receipt, Customer, Date".ToUpper()))
            {
                return _list;
            }
            try
            {
                IEnumerable<Receipt_Note> result;
                result = _list.FindAll(c => c.Receipt_NoteID.ToUpper().Contains(key.ToUpper()) || c.User.DisplayName.ToUpper().Contains(key.ToUpper()) || c.Username.ToUpper().Contains(key.ToUpper()) || c.Created.ToString().ToUpper().Contains(key.ToUpper()));
                return result.ToList();
            }
            catch
            {
                return _list;
            }
        }
        public Receipt_Note createNewReceipt()
        {
            string newReceiptID = "NK001";
            var c = _receiptnoteRepository.listReceipt_Note().LastOrDefault();
            if (c != null)
            {
                int Num = int.Parse(c.Receipt_NoteID.Substring(2));
                Num++;
                if (Num < 10)
                {
                    newReceiptID = "NK00" + Num.ToString();
                }
                else if (Num < 100)
                {
                    newReceiptID = "NK0" + Num.ToString();
                }
                else newReceiptID = "NK" + Num.ToString();
            }
            Receipt_Note target = new Receipt_Note();
            target.Receipt_NoteID = newReceiptID;
            target.Status = 1;
            target.Amount = 0;
            target.Modified = DateTime.Now;
            target.Created = DateTime.Now;
            return target;
        }
        public bool createReceipt_Note(Receipt_Note target)
        {
            try
            {
                _receiptnoteRepository.createReceipt_Note(target);
            }
            catch
            {

                return false;
            }
            return true;
        }
        public Receipt_Note getReceipt(string id)
        {
            return _receiptnoteRepository.getReceipt_Note(id);
        }

        public IEnumerable<Product> listProductByKey(string key)
        {
            if (String.IsNullOrEmpty( key) || key.Equals("Key: Product, Category, Supplier"))
                return _productRepository.listProduct();
            return _productRepository.listProduct().Where(
                c => c.Category.GroupName.ToUpper().Contains(key.ToUpper()) ||
                c.Name.ToUpper().Contains(key.ToUpper()) ||
                c.Supplier.Name.ToUpper().Contains(key.ToUpper()) ||
                c.SupplierID.ToUpper().Contains(key.ToUpper()) ||
                c.CategoryID.ToUpper().Contains(key.ToUpper()) ||
                c.ProductID.ToUpper().Contains(key.ToUpper())
                ).ToList();
        }
        public Receipt_Note_Detail GetReceipt_Note_Detail(string id,string pid)
        {
            return _receiptnotedetailRepository.getReceipt_Note_Detail(id, pid);
        }
        public bool createReceipt_Note_Detail(Receipt_Note_Detail index)
        {
            try
            {
                _receiptnotedetailRepository.createReceipt_Note_Detail(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool updateReceipt_Note_Detail(Receipt_Note_Detail index)
        {
            try
            {
                _receiptnotedetailRepository.updateReceipt_Note_Detail(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool deleteReceipt_NoteDetail(Receipt_Note_Detail index)
        {
            try
            {
                _receiptnotedetailRepository.deleteReceipt_Note_Detail(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public Product getProduct(string id)
        {
            return _productRepository.getProduct(id);
        }
        public IEnumerable<Receipt_Note_Detail> listReceiptDetailByID(string ID)
        {
            return _receiptnotedetailRepository.listReceipt_Note_DetailByID(ID);
        }
        public bool updateReceipt(string id,int status)
        {
            try
            {
                float sum = 0;
                List<Receipt_Note_Detail> _list = _receiptnotedetailRepository.listReceipt_Note_DetailByID(id).ToList();
                foreach (var item in _list)
                {
                    sum += item.Quality * item.Price;
                }
                Receipt_Note target = _receiptnoteRepository.getReceipt_Note(id);
                target.Modified = DateTime.Now;
                target.Amount = sum;
                target.Status = status;       
                _receiptnoteRepository.updateReceipt_Note(target);
                if(status==3)
                {
                    foreach (var item in _list)
                    {
                        Product toupdate = _productRepository.getProduct(item.ProductID);
                        toupdate.Quality += item.Quality;
                        _productRepository.updateProduct(toupdate);
                    }
                }
            }
            catch
            {

                return false;
            }
            return true;
        }
        public IEnumerable<Receipt_Note> listReceipt()
        {
            return _receiptnoteRepository.listReceipt_Note();
        }
        public IEnumerable<Product> listProductOut()
        {
            List<Product> _list = _productRepository.listProduct().ToList() ;
            List<Product> result = new List<Product>();
            int count = 0;
            foreach (var item in _list)
            {
                if (count == 6)
                    return result;
                if (item.Quality < 20)
                {
                    result.Add(item);
                    count++;
                }
            }
            return result;
        }
        public bool UpdateReceipt(Receipt_Note index)
        {
            try
            {
                _receiptnoteRepository.updateReceipt_Note(index);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool DeleteReceipt(Receipt_Note index)
        {
            try
            {
                _receiptnoteRepository.deleteReceipt_Note(index);
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}
