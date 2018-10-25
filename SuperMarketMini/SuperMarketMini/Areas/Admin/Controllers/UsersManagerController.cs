using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using PagedList;
using System.Web;
using System.IO;
using System.Drawing;
using SuperMarketMini.Areas.Admin.Common;
using System.Linq;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class UsersManagerController : BaseController
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static UserServices _service = new UserServices(new Servies.Validation.ModelStateWrapper(_modelState));
        IEnumerable _list = _service.listType();
        private void ViewErrors()
        {
            ModelState.Clear();
            foreach (var item in _modelState)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }
        }
        // GET: Admin/Users
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            var B = TempData["Delete"];
            var C = TempData["Create"];
            if (B != null)
            {
                ViewBag.Delete = "Delete Success";
            }
            if (C != null)
            {
                ViewBag.Create = "Create Success";
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
            List<User> list=_service.SearchUsers(searchString).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            list.Remove(_service.getUser("admin"));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName");
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Email,Phone,Birthday,DisplayName,Sex,TypeID")] User user)
        {
            if (ModelState.IsValid)
            {
                if (_service.createUser(user))
                {
                    TempData["Create"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                    ViewErrors();
            }

            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }
        public ActionResult ProfileUser(string id)
        {
            SessionAdmin session = (SessionAdmin)Session[Infrastructure.Information.CommonConstantAdmin];
            if (id != session.Username)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _service.getUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileUser([Bind(Include = "Username,Password,Address1,Address2,Email,Images,Phone,Birthday,DisplayName,Point,Trust,Sex,Created,Status,TypeID")] User user, FormCollection fc)
        {
            string username = fc["username"];
            string size = fc["size"];
            string urlrequest = fc["urlrequest"];
            string type = fc["type"];
            int sizeResize = 200;
            if (!string.IsNullOrEmpty(size))
            {
                int.TryParse(size, out sizeResize);
            }
            List<string> fileNames = new List<string>();
            try
            {
                // Duyệt qua các file được gởi lên phía client
                foreach (string fileName in Request.Files)
                {
                    //Lấy ra file trong list các file gởi lên
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here

                    if (file != null && file.ContentLength > 0)
                    {
                        //Định nghĩa đường dẫn lưu file trên server
                        //ở đây mình lưu tại đường dẫn yourdomain.com/Uploads/
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\" + type + "\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString());
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        string newFileName = file.FileName;
                        //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                        var newPath = Infrastructure.Images.GetNewPathForDupes(path, ref newFileName);
                        string serverPath = string.Format("/{0}/{1}/", "Images\\" + type + "\\", newFileName);
                        string name = file.FileName;
                        user.Images = "/Images/" + type + "/" + name;
                        //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                        Infrastructure.Images.SaveResizeImage(Image.FromStream(file.InputStream), sizeResize, newPath);               
                    }
                }
            }
            catch
            {
            }
            if (ModelState.IsValid)
            {
                if (_service.updateUser(user))
                {
                    ViewBag.Success = "Successful Change";
                    SessionAdmin adminLogin = (SessionAdmin)Session[Infrastructure.Information.CommonConstantAdmin];
                    adminLogin.Img = user.Images;
                    ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
                    return View(user);
                }
                else
                {
                    ViewErrors();
                }
            }
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }
        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _service.getUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,Email,Images,Phone,Birthday,DisplayName,Point,Trust,Sex,Created,Status,TypeID")] User user)
        {
            if (ModelState.IsValid)
            {
                if(_service.updateUser(user))
                     return RedirectToAction("Index");
                else
                {
                    ViewErrors();
                }
            }
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }
        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            SessionAdmin session = (SessionAdmin)Session[Infrastructure.Information.CommonConstantAdmin];
            if (id != session.Username)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _service.getUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(_list, "TypeID", "DisplayName", user.TypeID);
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = _service.getUser(id);
            if (_service.deleteUser(user))
            {
                TempData["Delete"] = "Success";
                return RedirectToAction("Index");
            }
            else
                return View(user);
        }
        public ActionResult Logout()
        {
            Session[Infrastructure.Information.CommonConstantAdmin] = null;
            return Redirect("/Admin");
        }
    }
}
