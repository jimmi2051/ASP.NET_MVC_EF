using Newtonsoft.Json;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SuperMarketMini.Areas.Admin.Models;
using System;
using PagedList;
using System.Net;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {

        private AdminServices _service = new AdminServices();
        // GET: Admin/Home
        public ActionResult Index()
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
            List<User> _list = _service.listUser().ToList();
            float total = _service.getTotalSell(DateTime.Now.AddDays(-DateTime.Now.Day), DateTime.Now, "--None--") - _service.getTotalBuy(DateTime.Now.AddDays(-DateTime.Now.Day), DateTime.Now, "--None--");
            List<Order> _listOrder = _service.listOrder().ToList();
            ViewBag.CountUsers = _list.Count;
            ViewBag.CountSales = total;
            ViewBag.CountOrders = _listOrder.Count;
            ViewBag.CountVisit = count_visit;
            return View();
        }
        public ActionResult getRevenue()
        {
            Dictionary<DateTime, float> _result = _service.getConvenue();
            List<GraphData> _list = new List<GraphData>();
            foreach (var item in _result)
            {
                GraphData index = new GraphData();
                string Date = item.Key.Year + "-" + item.Key.Month + "-" + item.Key.Day;
                index.label = Date;
                index.value = item.Value;
                _list.Add(index);
            }
            _list = _list.Take(10).ToList();
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Notification()
        {
            List<Order> _listOrderDeadline = _service.listOrderDeadline().ToList();
            ViewBag.ListOrder = _listOrderDeadline;
            Dictionary<string, string> result = _service.getNotification();
            ViewBag.Notification = result;
            List<Message> _listMess = _service.listMessNoneRead().ToList();
            ViewBag.ListMess = _listMess;
            return PartialView();
        }
        public PartialViewResult NotificationDetail()
        {
            List<Order> _listOrderDeadline = _service.listOrderDeadline().ToList();
            ViewBag.ListOrder = _listOrderDeadline;
            Dictionary<string, string> result = _service.getNotification();
            ViewBag.Notification = result;
            List<Message> _listMess = _service.listMessNoneRead().ToList();
            ViewBag.ListMess = _listMess;
            return PartialView();
        }
        public ActionResult Mail(string searchString, string currentFilter, int? page)
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
            List<Message> _list = _service.searchMess(searchString).ToList();
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            int count = _service.listMessage().Where(c=>c.status==1).Count();
            ViewBag.Count = count;
            return View(_list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DetailsMail(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message mess = _service.getMess(int.Parse(id));
            if (mess == null)
            {
                return HttpNotFound();
            }
            int count = _service.listMessage().Where(c => c.status == 1).Count();
            ViewBag.Count = count;
            return View(mess);
        }
        [HttpPost]
        public ActionResult DetailsMail(string Email,string IContent,string ID)
        {
            if(_service.SendMail(Email, IContent,"Reply from Supermarket mini",null,null))
            {
                return RedirectToAction("DetailsMail", "Home", new { @id = ID });
            }
            return RedirectToAction("DetailsMail", "Home", new { @id = ID });
        }
        public ActionResult SendMail()
        {
            int count = _service.listMessage().Where(c => c.status == 1).Count();
            ViewBag.Count = count;
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(string email,string cc,string bcc, string subject,string IContent)
        {
            _service.SendMail(email, IContent, subject, cc, bcc);
            int count = _service.listMessage().Where(c => c.status == 1).Count();
            ViewBag.Count = count;
            ViewBag.Success = "Success";
            return View();
        }
        public ActionResult updateMess(string id)
        {
            Message mess = _service.getMess(int.Parse(id));
            mess.status = 2;
            _service.updateMess(mess);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}