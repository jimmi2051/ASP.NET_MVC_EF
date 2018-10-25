using PagedList;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperMarketMini.Areas.Admin.Common;
using System.Net;
using Newtonsoft.Json;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class Receipt_NoteController : BaseController
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
        public ActionResult Index(string searchString, string currentFilter, int? page, string Type)
        {           
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            IEnumerable<Receipt_Note> list = _service.searchReceipt(searchString);
            if (Type != null)
            {
                list = list.Where(c => c.Status.Equals(int.Parse(Type)));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            Receipt_Note receipt_Note = _service.createNewReceipt();
            SessionAdmin index = (SessionAdmin)Session[Infrastructure.Information.CommonConstantAdmin];
            receipt_Note.Username = index.Username;
            _service.createReceipt_Note(receipt_Note);
            return Redirect("/Admin/Receipt_Note/InsertItemReceipt?id="+receipt_Note.Receipt_NoteID);
        }
        public ActionResult InsertItemReceipt(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt_Note receipt = _service.getReceipt(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            List<Product> list = _service.listProductOut().ToList();
            ViewBag.ListProduct = list.OrderBy(c => c.Quality).ToList() ;
            return View(receipt);
        }
        public ActionResult SearchProduct(string key)
        {
            List<Product> _list = _service.listProductByKey(key).ToList();
            if (key.Equals("Key: Product, Category, Supplier") || String.IsNullOrEmpty(key))
                _list = _service.listProductOut().ToList();
            var results = _list.Select(e => new
            {
                ProductID = e.ProductID,
                ProductName=e.Name,
                Quality = e.Quality,
            }).ToList();
            var result = JsonConvert.SerializeObject(results, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddItem(string receiptnote_id, string product_id, int quality)
        {
            Receipt_Note_Detail index = _service.GetReceipt_Note_Detail(receiptnote_id, product_id);
            Product product = _service.getProduct(product_id);
            if (index==null)
            {
                index = new Receipt_Note_Detail();
                index.Receipt_NoteID = receiptnote_id;
                index.ProductID = product.ProductID;
                index.Price = product.PriceBuy;
                index.Quality = quality;
                _service.createReceipt_Note_Detail(index);
            }
            else
            {
                index.Quality = quality;
                _service.updateReceipt_Note_Detail(index);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteItem(string receiptnote_id,string product_id)
        {
            Receipt_Note_Detail index = _service.GetReceipt_Note_Detail(receiptnote_id, product_id);
            if(index!=null)
            {
                _service.deleteReceipt_NoteDetail(index);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReceiptDetail(string id)
        {
            List<Receipt_Note_Detail> _list = _service.listReceiptDetailByID(id).ToList();
            var results = _list.Select(e => new
            {
                ProductID = e.ProductID,
                ProductPrice = e.Price,
                Quality = e.Quality,
            }).ToList();
            var result = JsonConvert.SerializeObject(results, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveReceipt(string id,string Status)
        {
            if(_service.updateReceipt(id,int.Parse(Status)))
            {
                return RedirectToAction("Index", "Receipt_Note");
            }
            else
            {
                return Redirect("/Admin/Receipt_Note/InsertItemReceipt?id=" + id);
            }
        }
        public ActionResult detailsReceipt(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt_Note receipt = _service.getReceipt(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            if(receipt.Status == 1 )
                return Redirect("/Admin/Receipt_Note/InsertItemReceipt?id=" + id);

            List<Receipt_Note_Detail> list = _service.listReceiptDetailByID(id).ToList();
            ViewBag.ListReceiptDetail = list;
            return View(receipt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult detailsReceipt([Bind(Include = "Receipt_NoteID,Username,Amount,Created,Modified,Status")] Receipt_Note receipt)
        {
            if (ModelState.IsValid)
            {
                if (_service.UpdateReceipt(receipt))
                {                 
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewErrors();
                }
            }
            List<Receipt_Note_Detail> list = _service.listReceiptDetailByID(receipt.Receipt_NoteID).ToList();
            ViewBag.ListReceiptDetail = list;
            return View(receipt);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt_Note receipt = _service.getReceipt(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            List<Receipt_Note_Detail> list = _service.listReceiptDetailByID(receipt.Receipt_NoteID).ToList();
            ViewBag.ListReceiptDetail = list;
            return View(receipt);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Receipt_Note receipt = _service.getReceipt(id);
            if (_service.DeleteReceipt(receipt))
            {            
                return RedirectToAction("Index", "Receipt_Note");
            }
            else
            {
                List<Receipt_Note_Detail> list = _service.listReceiptDetailByID(receipt.Receipt_NoteID).ToList();
                ViewBag.ListReceiptDetail = list;
                return View(receipt);
            }
        }
    }
}