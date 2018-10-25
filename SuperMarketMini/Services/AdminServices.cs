using SuperMarketMini.Domain;
using SuperMarketMini.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketMini.Servies
{
    public class AdminServices
    {
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IMessageRepository _messageRepository;
        private IOrder_DetailRepository _DetailRepository;
        public AdminServices()
        {
            _userRepository = new UserRepository();
            _productRepository = new ProductRepository();
            _orderRepository = new OrderRepository();
            _messageRepository = new MessageRepository();
            _DetailRepository = new Order_DetailRepository();
        }
        public IEnumerable<User> listUser()
        {
            return _userRepository.listUser();
        }
        public IEnumerable<Product> listProductSales()
        {
            return _productRepository.listProduct().Where(c => c.Special == 1);
        }
        public IEnumerable<Order> listOrder()
        {
            return _orderRepository.listOrder();
        }
        
        public IEnumerable<Order> listOrderDeadline()
        {
            return _orderRepository.listOrder().Where(c => DateTime.Now > c.RequireDate.AddDays(-5) && DateTime.Now < c.RequireDate && c.Status != 3);
        }
        public Dictionary<string, string> getNotification()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<Order> _list = _orderRepository.listOrder().ToList();
            foreach (var item in _list)
            {
                if (item.RequireDate < DateTime.Now && item.Status != 3)
                {
                    string Date = item.RequireDate.Year + "-" + item.RequireDate.Month + "-" + item.RequireDate.Day;
                    result.Add(item.OrderID, Date);
                }
            }
            List<Product> _listProduct = _productRepository.listProduct().ToList();
            foreach (var item in _listProduct)
            {
                if (item.Quality < 10)
                    result.Add(item.ProductID, item.Quality.ToString());
            }
            Dictionary<string, string> shuffled = Infrastructure.Suff.Shuffle(result);
            return shuffled;
        }
        public Message getMess(int ID)
        {
            return _messageRepository.getMess(ID);
        }
        public IEnumerable<Message> listMessage()
        {
            return _messageRepository.listMess();
        }
        public bool updateMess(Message target)
        {
            try
            {
                _messageRepository.updateMess(target);
            }
            catch
            {

                return false;
            }
            return true;
        }
        public bool delete(Message target)
        {
            try
            {
                _messageRepository.deleteMess(target);
            }
            catch
            {

                return false;
            }
            return true;
        }
        public IEnumerable<Message> listMessNoneRead()
        {
            return _messageRepository.listMess().Where(c => c.status == 1);
        }
        public IEnumerable<Message> searchMess(string key)
        {
            List<Message> _list = _messageRepository.listMess().ToList();
            if (String.IsNullOrEmpty(key))
                return _list;
            return _list.Where(c => c.Sendby.ToUpper().Contains(key.ToUpper()) || c.Displayname.ToUpper().Contains(key.ToUpper()) || c.Content.ToUpper().Contains(key.ToUpper()));
        }
        public bool SendMail(string mail, string content,string subject, string cc,string bcc)
        {
            MailMessage objEmail = new MailMessage();
            objEmail.To.Add(mail);
            objEmail.From = new MailAddress("jimmi2051@gmail.com");
            objEmail.Subject = subject;
            if(!String.IsNullOrEmpty(cc))
            objEmail.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                objEmail.Bcc.Add(bcc);
            objEmail.BodyEncoding = Encoding.UTF8;
            objEmail.Body = content;
            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                smtp.Send(objEmail);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public IEnumerable<Product> listProduct()
        {
            return _productRepository.listProduct();
        }
        public Dictionary<DateTime, float> getConvenue()
        {
            Dictionary<DateTime, float> result = new Dictionary<DateTime, float>();
            List<Order> _list = _orderRepository.listOrder().ToList();
            var query = from Order in _list group Order by Order.OrderDate.Date;
            foreach (var item in query)
            {
                float sum = 0;
                foreach (Order order in item)
                {
                    sum += order.Amount;
                }
                result.Add(item.Key, sum);
            }
            return result;
        }
        public float getTotalSell(DateTime start, DateTime end,string key)
        {
            List<Order_Detail> _orderdetail = _DetailRepository.listOrder_Detail().ToList();
            List<Order> _order = _orderRepository.listOrder().ToList();
            var query = (from post in _order
                         join meta in _orderdetail on post.OrderID equals meta.OrderID
                         where post.OrderDate > start && post.OrderDate < end
                         select meta).ToList();
            if (key != "--None--")
                query = query.Where(c => c.Order.Username.Equals(key)).ToList();
            float result = 0;
            foreach (var item in query)
            {
                result += (item.Quality*item.Product.PriceSell);
            }
            return result;
        }
        public float getTotalBuy(DateTime start, DateTime end, string key)
        {
            List<Order_Detail> _orderdetail = _DetailRepository.listOrder_Detail().ToList();
            List<Order> _order = _orderRepository.listOrder().ToList();
            var query = (from post in _order
                         join meta in _orderdetail on post.OrderID equals meta.OrderID
                         where post.OrderDate > start && post.OrderDate < end
                         select meta).ToList();
            if (key != "--None--")
                query = query.Where(c => c.Order.Username.Equals(key)).ToList();
            float result = 0;
            foreach (var item in query)
            {
                result += (item.Quality * item.Product.PriceBuy);
            }
            return result;
        }
        public int getTotalOrder(DateTime start, DateTime end, string key)
        {
            List<Order_Detail> _orderdetail = _DetailRepository.listOrder_Detail().ToList();
            List<Order> _order = _orderRepository.listOrder().ToList();
            var query = (from post in _order
                         join meta in _orderdetail on post.OrderID equals meta.OrderID
                         where post.OrderDate > start && post.OrderDate < end
                         select meta).ToList();
            if (key != "--None--")
                query = query.Where(c => c.Order.Username.Equals(key)).ToList();
            return query.Count;
        }
        public Dictionary<DateTime, float> getConvenue(DateTime start,DateTime end,string key)
        {
            Dictionary<DateTime, float> result = new Dictionary<DateTime, float>();
            List<Order> _list;
            if (key== "--None--")
                _list = _orderRepository.listOrder().Where(c=>c.OrderDate>start && c.OrderDate<end).ToList();
            else
                _list = _orderRepository.listOrder().Where(c => c.OrderDate > start && c.OrderDate < end &&c.Username.Equals(key)).ToList();
            var query = from Order in _list group Order by Order.OrderDate.Date;
            foreach (var item in query)
            {
                float sum = 0;
                foreach (Order order in item)
                {
                    sum += order.Amount;
                }
                result.Add(item.Key, sum);
            }
            return result;
        }

        public Dictionary<string,int> getProducthot(DateTime start, DateTime end,string key)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<Order_Detail> _orderdetail = _DetailRepository.listOrder_Detail().ToList();
            List<Order> _order = _orderRepository.listOrder().ToList();
            var query = (from post in _order
                        join meta in _orderdetail on post.OrderID equals meta.OrderID
                        where post.OrderDate>start && post.OrderDate<end 
                        select meta).ToList();
            if (key != "--None--")
                query = query.Where(c => c.ProductID.Equals(key)).ToList();
            var iquery = from c in query group c by c.ProductID;
            foreach (var item in iquery)
            {
                int sum = 0;
                foreach (Order_Detail order in item)
                {
                    sum += order.Quality;
                }
                result.Add(item.Key, sum);
            }
            return result;
        }
    }
}

