using SuperMarketMini.Areas.Admin.Models;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class StatisticalController : BaseController
    {
        private AdminServices _service = new AdminServices();

        public ActionResult getRevenue(string key, string start, string end, string quality, string sort)
        {

            DateTime istart = DateTime.Parse(start);
            DateTime iend = DateTime.Parse(end);
            Dictionary<DateTime, float> _result = _service.getConvenue(istart, iend, key);
            List<GraphData> _list = new List<GraphData>();
            foreach (var item in _result)
            {
                GraphData index = new GraphData();
                string Date = item.Key.Year + "-" + item.Key.Month + "-" + item.Key.Day;
                index.label = Date;
                index.value = item.Value;
                _list.Add(index);
            }
            if (sort == "ASC")
            {
                _list = _list.OrderBy(c => c.value).ToList();
            }
            if (sort == "DESC")
            {
                _list = _list.OrderByDescending(c => c.value).ToList();
            }
            _list = _list.Take(int.Parse(quality)).ToList();
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInformationRevenue(string key, string start, string end, string quality)
        {
            DateTime istart = DateTime.Parse(start);
            DateTime iend = DateTime.Parse(end);
            float TotalSell = _service.getTotalSell(istart, iend, key);
            float TotalBuy = _service.getTotalBuy(istart, iend, key);
            float Revenue = TotalSell - TotalBuy;
            float TotalOrder = _service.getTotalOrder(istart, iend, key);
            var result = new
            {
                TotalSell = TotalSell,
                TotalBuy = TotalBuy,
                Revenue = Revenue,
                TotalOrder = TotalOrder,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProductHot(string key, string start, string end, string quality, string sort)
        {
            DateTime istart = DateTime.Parse(start);
            DateTime iend = DateTime.Parse(end);
            Dictionary<string, int> _result = _service.getProducthot(istart, iend, key);
            List<GraphData> _list = new List<GraphData>();
            foreach (var item in _result)
            {
                GraphData index = new GraphData();
                index.label = item.Key;
                index.value = item.Value;
                _list.Add(index);
            }
            if (sort == "ASC")
            {
                _list = _list.OrderBy(c => c.value).ToList();
            }
            if (sort == "DESC")
            {
                _list = _list.OrderByDescending(c => c.value).ToList();
            }
            _list = _list.Take(int.Parse(quality)).ToList();
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            List<User> _list = _service.listUser().ToList();
            User none = new User();
            none.Username = "--None--";
            _list.Insert(0, none);
            IEnumerable result = _list;
            List<Product> _listPoduct = _service.listProduct().ToList();
            Product nonePro = new Product();
            nonePro.ProductID = "--None--";
            _listPoduct.Insert(0, nonePro);
            IEnumerable resultp = _listPoduct;
            ViewBag.ListUser = new SelectList(result, "Username", "Username");
            ViewBag.ListProduct = new SelectList(resultp, "ProductID", "ProductID");          
            return View();
        }
    }
}