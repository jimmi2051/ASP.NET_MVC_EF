using SuperMarketMini.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SuperMarketMini.Domain;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class ProductController : BaseController
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
        // GET: Admin/Product
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            var A = TempData["Update"];
            var B = TempData["Delete"];
            var C = TempData["Create"];
            if (A != null)
            {
                ViewBag.Update = "Update Success";
            }
            if (B != null)
            {
                ViewBag.Delete = "Delete Success";
            }
            if(C!=null)
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
            IEnumerable<Product> list = _service.searchProduct(searchString);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            Product target = _service.CreateNewProduct();
            ViewBag.CategoryID = new SelectList(_listCategory, "CategoryID", "Name");
            ViewBag.SupplierID = new SelectList(_listSupplier, "SupplierID", "Name");
            return View(target);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Detail,Quality,PriceBuy,PriceSell,CategoryID,SupplierID,UnitBrief")] Product product,FormCollection fc)
        {
            string size = fc["size"];
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
                        string pathString = Path.Combine(originalDirectory.ToString());
                        bool isExists = Directory.Exists(pathString);
                        if (!isExists) Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        string newFileName = file.FileName;
                        //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                        var newPath = Infrastructure.Images.GetNewPathForDupes(path, ref newFileName);
                        string serverPath = string.Format("/{0}/{1}/", "Images\\" + type + "\\", newFileName);
                        string name = file.FileName;
                        product.Images = "/Images/" + type + "/" + name;
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
                if (_service.createProduct(product))
                {
                    TempData["Create"] = "Success";
                    return RedirectToAction("Index");               
                }
                else
                    ViewErrors();
            }

            ViewBag.CategoryID = new SelectList(_listCategory, "CategoryID", "Name");
            ViewBag.SupplierID = new SelectList(_listSupplier, "SupplierID", "Name");
            return View();
        }
        public ActionResult Edit(string id)
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
            ViewBag.CategoryID = new SelectList(_listCategory, "CategoryID", "Name",product.CategoryID);
            ViewBag.SupplierID = new SelectList(_listSupplier, "SupplierID", "Name",product.SupplierID);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Detail,Quality,PriceBuy,PriceSell,CategoryID,SupplierID,Status,UnitBrief,Product_hot,Special,Discount")] Product product, FormCollection fc)
        {
            string size = fc["size"];
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
                        string pathString = Path.Combine(originalDirectory.ToString());
                        bool isExists = Directory.Exists(pathString);
                        if (!isExists) Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        string newFileName = file.FileName;
                        //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                        var newPath = Infrastructure.Images.GetNewPathForDupes(path, ref newFileName);
                        string serverPath = string.Format("/{0}/{1}/", "Images\\" + type + "\\", newFileName);
                        string name = file.FileName;
                        product.Images = "/Images/" + type + "/" + name;
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
                if (_service.updateProduct(product))
                {
                    TempData["Update"] = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewErrors();
                }
            }
            ViewBag.CategoryID = new SelectList(_listCategory, "CategoryID", "Name", product.CategoryID);
            ViewBag.SupplierID = new SelectList(_listSupplier, "SupplierID", "Name", product.SupplierID);
            return View(product);
        }
        public ActionResult Delete(string id)
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
            ViewBag.CategoryID = new SelectList(_listCategory, "CategoryID", "Name", product.CategoryID);
            ViewBag.SupplierID = new SelectList(_listSupplier, "SupplierID", "Name", product.SupplierID);
            return View(product);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product target = _service.getProduct(id);
            if (_service.deleteProduct(id))
            {
                TempData["Delete"] = "Success";
                return RedirectToAction("Index");
            }
            else
                return View(target);
        }
    }
}