using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SuperMarketMini.Domain;
using SuperMarketMini.Models;
using SuperMarketMini.Servies;
using SuperMarketMini.Common;
using System.Net;

namespace SuperMarketMini.Controllers
{
    public class CartController : Controller
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static ProductServices _service = new ProductServices(new Servies.Validation.ModelStateWrapper(_modelState));
        IEnumerable _listSupplier = _service.listSupplier();
        IEnumerable _listCategory = _service.listCategory();
        private void ViewErrors()
        {
            ModelState.Clear();
            foreach (var item in _modelState)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }
        }
        public ActionResult Index()
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }           
            return View(list);
        }
        [HttpPost]
        public ActionResult AddItem(string product_id, int quality)
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            Product target = _service.getProduct(product_id);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(c => c.Product.Equals(target)))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.Equals(target))
                        {
                            item.Quality += quality;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = target;
                    item.Quality = quality;
                    list.Add(item);
                }
            }
            else
            {
                var item = new CartItem();
                item.Product = target;
                item.Quality = quality;
                var list = new List<CartItem>();
                list.Add(item);
                Session[Infrastructure.Information.CommonConstantCard] = list;
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult getData()
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                var results = list.Select(e => new
                {
                    ProductID = e.Product.ProductID,
                    ProductName = e.Product.Name,
                    ProductPrice = (e.Product.PriceSell - (e.Product.PriceSell* e.Product.Discount) /100),
                    ProductImages = e.Product.Images,
                    Quality = e.Quality,
                }).ToList();
                var result = JsonConvert.SerializeObject(results, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangeItem(string product_id, int quality)
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            Product target = _service.getProduct(product_id);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(c => c.Product.Equals(target)))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.Equals(target))
                        {
                            item.Quality = quality;
                        }
                    }
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveItem(string product_id)
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            Product target = _service.getProduct(product_id);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(c => c.Product.Equals(target)))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.Equals(target))
                        {
                            list.Remove(item);
                        }
                    }
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        private float AmountPay()
        {
            float sum = 0;
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            if (cart != null)
            {
                
                var list = (List<CartItem>)cart;
                foreach (var item in list)
                {
                    sum += item.Quality * (item.Product.PriceSell - (item.Product.PriceSell*item.Product.Discount)/100)  ;
                }
            }
            return sum;
        }
        public ActionResult Payment()
        {
            SessionUser user = (SessionUser)Session[Infrastructure.Information.CommonConstantUsers];
            if (user == null)
            {
                TempData["Error"] = "Please login to pay. Thank you";
                return RedirectToAction("Login", "Users");
            }
            float sumAmount = AmountPay();
            Order index = _service.createNewOrder();
            index.Username = user.Username;
            if (TempData["Address"] != null)
                index.Address = TempData["Address"].ToString();
            index.Amount = AmountPay();
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.List = list;
            return View(index);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment([Bind(Include = "OrderID,Username,Amount,Address,Description,Contact,OrderDate,RequireDate")]Order index)
        {
            var list = getCartItems();
            List<Infrastructure.ItemToPayment> _list = new List<Infrastructure.ItemToPayment>();
            foreach (var item in list)
            {
                Infrastructure.ItemToPayment Iitem = new Infrastructure.ItemToPayment();
                Iitem.Product_ID = item.Product.ProductID;
                Iitem.priceSell = item.Product.PriceSell;
                Iitem.quality = item.Quality;
                _list.Add(Iitem);
            }
            if (ModelState.IsValid)
            {
                index.Status = 1;
                if (!_service.createOrder(index))
                    return View(index);
                if (_service.Payment(index, _list))
                {
                    TempData["Order"] = index;
                    Session[Infrastructure.Information.CommonConstantCard] = null;
                    return RedirectToAction("PaymentSuccess");
                }
                else
                {
                    ViewErrors();                 
                }
            }
            return View(index);
        }
        public List<CartItem> getCartItems()
        {
            var cart = Session[Infrastructure.Information.CommonConstantCard];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return list;
        }
        public ActionResult PaymentSuccess()
        {
            Order index = (Order)TempData["Order"];
            return View(index);
        }
        public ActionResult ViewHistory()
        {
            SessionUser cart = (SessionUser)Session[Infrastructure.Information.CommonConstantUsers];
            if(cart == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<Order> list = _service.listOrderByUser(cart.Username).ToList();
            return View(list);
        }
        public ActionResult DetailsOrder(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Order_Detail> list_order = _service.GetOrder_Details(id).ToList();
            if (list_order.Count == 0 )
            {
                return HttpNotFound();
            }
            Order index = _service.getOrder(id);
            ViewBag.Order = index;
            return View(list_order);
        }
    }
}