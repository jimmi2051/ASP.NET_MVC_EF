using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SuperMarketMini.Controllers
{
    public class HomeController : Controller
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
            List<Product> _list = _service.listProduct().ToList();
            Infrastructure.Suff.Shuffle(_list);
            ViewBag.List = _list;
            return View();
        }
        public PartialViewResult Offer()
        {
            List<Product> _listRelate = _service.listHotProduct().ToList();
            List<Product> _listSpecial = _service.listSpecialProduct().ToList();
            Infrastructure.Suff.Shuffle(_listRelate);
            Infrastructure.Suff.Shuffle(_listSpecial);
            ViewBag.ListSpecial = _listSpecial;
            ViewBag.ListHot = _listRelate;
            return PartialView();
        }
        public ActionResult NewOffer()
        {
            return View();
        }
        public PartialViewResult topMenu()
        {
            List<String> _listMenu = _service.listGroupCat().ToList();
            ViewBag.ListMenu = _listMenu;
            IEnumerable<Category> _listCat = _service.listCategory();
            return PartialView(_listCat);
        }
        public PartialViewResult footerMenu()
        {
            List<String> _listMenu = _service.listGroupCat().ToList();
            ViewBag.ListMenu = _listMenu;
            IEnumerable<Category> _listCat = _service.listCategory();
            return PartialView(_listCat);
        }
        public PartialViewResult Brand()
        {
            List<Supplier> _listMenu = _service.listSupplier().ToList();
            return PartialView(_listMenu);
        }
        public ActionResult Counter()
        {
            int count_visit = 0;
            if (System.IO.File.Exists(Server.MapPath("~/count_visit.txt")) == false)
            {
                count_visit = 1;
            }
            // Ngược lại thì
            else
            {
                // Đọc dử liều từ file count_visit.txt
                System.IO.StreamReader read = new System.IO.StreamReader(Server.MapPath("~/count_visit.txt"));
                count_visit = int.Parse(read.ReadLine());
                read.Close();
                // Tăng biến count_visit thêm 1
                count_visit++;
            }
            System.IO.StreamWriter writer = new System.IO.StreamWriter(Server.MapPath("~/count_visit.txt"));
            writer.WriteLine(count_visit);
            writer.Close();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            Message index = new Message();
            index.Created = DateTime.Now;
            index.status = 1;
            return View(index);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ID,Sendby,Displayname,Content,Created,status")] Message mess)
        {
            if (ModelState.IsValid)
            {
                _service.createMess(mess);
                ViewBag.Success = "Success";
                return View(mess);
            }
            return View(mess);
        }
        public ActionResult FAQ()
        {
            return View();
        }

    }
}