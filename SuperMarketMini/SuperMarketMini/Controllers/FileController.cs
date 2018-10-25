using SuperMarketMini.Servies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketMini.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static UserServices _service = new UserServices(new Servies.Validation.ModelStateWrapper(_modelState));
        private void ViewErrors()
        {
            ModelState.Clear();
            foreach (var item in _modelState)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }
        }
        [HttpPost]
        public ActionResult Upload(FormCollection fc)
        {
            string username = fc["username"];
            string size = fc["size"];
            string urlrequest = fc["urlrequest"];
            string type = fc["type"];
            string namefile = "";
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
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\"+type+"\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), DateTime.Now.ToString("yyyy-MM"));
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        string newFileName = file.FileName;
                        namefile = file.FileName;
                        //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                        var newPath = GetNewPathForDupes(path, ref newFileName);
                        string serverPath = string.Format("/{0}/{1}/{2}", "Images\\"+type+"\\", DateTime.Now.ToString("yyyy-MM"), newFileName);
                        //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                        SaveResizeImage(Image.FromStream(file.InputStream), sizeResize, newPath);
                        fileNames.Add("LocalPath: " + newPath + "<br/>ServerPath: " + serverPath);
                    }
                }
            }
            catch
            {
            }
            TempData["file"] = fileNames;
            _service.UpdateImg(username, namefile);
            return Redirect(urlrequest);

        }
        //Hàm resize hình ảnh

        public bool SaveResizeImage(Image img, int width, string path)
        {
            try
            {
                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = img.Width;
                int originalH = img.Height;
                // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                int resizedW = width;
                int resizedH = (originalH * resizedW) / originalW;
                Bitmap b = new Bitmap(resizedW, resizedH);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                g.DrawImage(img, 0, 0, resizedW, resizedH);
                g.Dispose();
                b.Save(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        private string GetNewPathForDupes(string path, ref string fn)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = filename + extension;
            while (System.IO.File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fn = new_file_name;
            return newFullPath;
        }

    }
}
