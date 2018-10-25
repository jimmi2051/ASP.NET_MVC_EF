using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace Infrastructure
{
     public  class Images
    {
        public static bool SaveResizeImage(Image img, int width, string path)
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
        public static string GetNewPathForDupes(string path, ref string fn)
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
