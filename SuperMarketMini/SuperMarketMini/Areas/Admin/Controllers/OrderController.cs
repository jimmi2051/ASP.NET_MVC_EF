using PagedList;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static OrderServices _service = new OrderServices(new Servies.Validation.ModelStateWrapper(_modelState));
        private void ViewErrors()
        {
            ModelState.Clear();
            foreach (var item in _modelState)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }
        }
        public ActionResult Index(string searchString, string currentFilter, int? page,string Type)
        {
            var A = TempData["Update"];
            var B = TempData["Delete"];
            if(A!=null)
            {
                ViewBag.Update = "Update Success";
            }
            if(B!=null)
            {
                ViewBag.Delete = "Delete Success";
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            IEnumerable<Order> list = _service.searchOrder(searchString);
            if(Type !=null)
            {
                list = list.Where(c => c.Status.Equals(int.Parse(Type)));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _service.getOrder(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = _service.listOrderDetailsByID(id);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "OrderID,Username,Amount,Address,Description,OrderDate,RequireDate,Status,Contact")] Order order)
        {
            if (ModelState.IsValid)
            {
                if (_service.updateOrder(order))
                {
                    TempData["Update"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewErrors();
                }
            }
            ViewBag.ListOrder = _service.listOrderDetailsByID(order.OrderID);
            return View(order);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _service.getOrder(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = _service.listOrderDetailsByID(order.OrderID);
            return View(order);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Order target = _service.getOrder(id);
            if (_service.deleteOrder(target))
            {
                TempData["Delete"] = "Success";
                return RedirectToAction("Index", "Order");
            }
            else
            {
                ViewBag.ListOrder = _service.listOrderDetailsByID(id);
                return View(target);
            }
        }
    }
}