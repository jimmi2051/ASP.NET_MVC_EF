using PagedList;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SuperMarketMini.Controllers
{
    public class ProductsController : Controller
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
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _service.getProduct(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            List<Product> _list = _service.listRelateProducts(product.CategoryID,product.SupplierID).ToList();
            Infrastructure.Suff.Shuffle(_list);
            _list.Remove(_service.getProduct(id));
            ViewBag.ListRelate = _list;
            return View(product);
        }
        public ActionResult listProductbyCat(string sup,string cat,string group, string searchString,string currentFilter, int? page,string keyfilter,string pricestart,string priceend )
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
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            List<Product> _list = _service.listProduct().ToList();
            if (cat == null && group == null && searchString == null && sup==null)
            {
                ViewBag.Cat = "All product"; 
                
            }
            if (!String.IsNullOrEmpty(sup) )
            {
                ViewBag.Cat = _service.getSupplier(sup).Name;
                _list = _service.listProductByKey(sup, "Sup").ToList();
                ViewBag.SID = sup;
            }
            if(!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Cat = "Search item";
                _list = _service.listProductByKey(searchString).ToList();
            }
            if(!String.IsNullOrEmpty(group))
            {
                ViewBag.Cat = group;
                _list = _service.listProductByKey(group, "Group").ToList();
                ViewBag.GID = group;
            }
            if(!String.IsNullOrEmpty(cat))
            {
                ViewBag.Cat = _service.getCategory(cat).Name;
                _list = _service.listProductByKey(cat, "Cat").ToList();
                ViewBag.CID = cat;
            }
            if (_list.Count == 0 )
            {
                ViewBag.Error = "No have item ";                
            }

            if(keyfilter=="4")
            {
                _list = _list.OrderBy(c => c.PriceSell).ToList();
            }
            if(keyfilter=="2")
            {
                _list = _list.OrderByDescending(c => c.Product_Hot).ToList();
            }
            if(keyfilter=="3")
            {
                _list = _list.OrderByDescending(c => c.Discount).ToList();
            }
            if(keyfilter=="5")
            {
                _list = _list.OrderByDescending(c => c.PriceSell).ToList();
            }
            if(!String.IsNullOrEmpty(pricestart))
            {
                float istart = float.Parse(pricestart);
                float iend = float.MaxValue;
                if (!String.IsNullOrEmpty(priceend))
                    iend = float.Parse(priceend);
                _list = _list.Where(c => c.PriceSell >= istart && c.PriceSell <= iend).ToList();
                ViewBag.pricestart = pricestart;
                ViewBag.priceend = priceend;
            }
            ViewBag.CurrentFilter2 = keyfilter;
            return View(_list.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult leftMenu()
        {
            List<String> _listMenu = _service.listGroupCat().ToList();
            ViewBag.ListMenu = _listMenu;
            IEnumerable<Category> _listCat = _service.listCategory();
            return PartialView(_listCat);
        }
    }
}