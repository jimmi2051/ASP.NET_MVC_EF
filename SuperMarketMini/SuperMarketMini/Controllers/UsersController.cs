using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPSnippets.GoogleAPI;
using Facebook;
using Newtonsoft.Json.Linq;
using SuperMarketMini.Common;
using SuperMarketMini.Domain;
using SuperMarketMini.Models;
using SuperMarketMini.Servies;

namespace SuperMarketMini.Controllers
{
    public class UsersController : Controller
    {
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
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallBack");
                return uriBuilder.Uri;
            }
        }
        // GET: Users
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,ConfirmPassword,Email,Phone,DisplayName,Sex")] RegisterUser user)
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = Infrastructure.Information.GoogleSecret;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            if (!status)
            {
                ViewBag.Message = "Google reCaptcha validation failed";
                return View(user);
            }           
            if (ModelState.IsValid)
            {
                User target = new User();
                target.Username = user.Username;
                target.Password = user.Password;
                target.Email = user.Email;
                target.Phone = user.Phone;
                target.DisplayName = user.DisplayName;
                target.Sex = user.Sex;
                if (_service.registerUser(target))
                {
                    ViewBag.Success = "You have successfully registered your account";
                    user = new RegisterUser();
                    return View(user);
                }
                else
                    ViewErrors();
            }
            return View(user);
        }       
        public ActionResult Login()
        {
            var Error = TempData["Error"];
            if(Error != null)
            {
                ViewBag.Error = Error;
            }
            if(Session[Infrastructure.Information.CommonConstantUsers]==null)
                return View();
            return Redirect("/");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password,RememberMe")]LoginUser target,FormCollection fc)
        {
            int checkreturn =  int.Parse(fc["TypeToReturn"]);
            if (ModelState.IsValid)
            {
                if (_service.loginUser(target.Username, target.Password))
                {
                    var userLogin = new SessionUser();
                    userLogin.Username = target.Username;
                    User iuser = _service.getUser(target.Username);
                    userLogin.DisplayName = iuser.DisplayName;
                    userLogin.Img = iuser.Images;
                    Session.Add(Infrastructure.Information.CommonConstantUsers, userLogin);
                    if (checkreturn == 1)
                    {
                        if (iuser.Address1 != null)
                            TempData["Address"] = iuser.Address1;
                        return RedirectToAction("Payment", "Cart");

                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewErrors();
                    ViewBag.Errors = "Login Failed";
                }
            }
            return View(target);
        }
        public ActionResult LoginFacebook(string TypeReturn)
        {
            TempData["CheckReturn"] = TypeReturn;
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(
                new
                {
                    client_id = Infrastructure.Information.FacebookID,
                    client_secret = Infrastructure.Information.FacebookSecret,
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type="code",
                    scope="email",
               
                }
                );
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallBack(string code)
        {
            int checkreturn = int.Parse(TempData["CheckReturn"].ToString());
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = Infrastructure.Information.FacebookID,
                client_secret = Infrastructure.Information.FacebookSecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            }
            );
            var accessToken = result.access_token;
            if(!string.IsNullOrEmpty(accessToken))
            {               
                fb.AccessToken = accessToken;              
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.id;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                User target = new User();
                target.Username = userName;
                target.Email = email;
                target.DisplayName = firstname + " " + middlename + " " + lastname;
                if(_service.addUserFacebook(target))
                {
                    SessionUser session = new SessionUser();
                    session.Username = target.Username;
                    session.DisplayName = target.DisplayName;
                    Session.Add(Infrastructure.Information.CommonConstantUsers, session);
                }    
            }
            if (checkreturn == 1)
                return RedirectToAction("Payment", "Cart");
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void LoginWithGooglePlus(FormCollection fc)
        {
            TempData["CheckReturn"] = int.Parse(fc["TypeToReturn"]);
            GoogleConnect.ClientId = Infrastructure.Information.GoogleLoginID;
            GoogleConnect.ClientSecret = Infrastructure.Information.GoogleLoginSecret;
            GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];
            GoogleConnect.Authorize("profile", "email");
        }
        [ActionName("LoginWithGooglePlus")]
        public ActionResult LoginWithGooglePlusConfirmed()
        {
            int checkreturn = 0;
            if (TempData["CheckReturn"]!=null)
                 checkreturn= int.Parse(TempData["CheckReturn"].ToString());
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                string json = GoogleConnect.Fetch("me", code);
                GoogleProfile profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);
                User target = new User();
                target.Username = profile.Id;
                target.DisplayName = profile.DisplayName;
                target.Email = profile.Emails.Find(email=>email.Type=="account").Value;
                target.Sex = profile.Gender;
                target.Images = "/Images/Users/"+ profile.Id + System.IO.Path.GetExtension(profile.Image.Url.Split('?')[0]);
                using (var client = new WebClient())
                {
                    client.DownloadFile(profile.Image.Url, Server.MapPath(target.Images));
                }
                User index = _service.getUser(target.Username);
                if (index == null)
                {
                    if (_service.addUserFacebook(target))
                    {
                        SessionUser session = new SessionUser();
                        session.Username = target.Username;
                        session.DisplayName = target.DisplayName;
                        session.Img = target.Images;
                        Session.Add(Infrastructure.Information.CommonConstantUsers, session);
                    }
                }
                else
                {
                    SessionUser session = new SessionUser();
                    session.Username = index.Username;
                    session.DisplayName = index.DisplayName;
                    session.Img = index.Images;
                    Session.Add(Infrastructure.Information.CommonConstantUsers, session);
                }
            }
            if (Request.QueryString["error"] == "access_denied")
            {
                ViewBag.Errors = "Permission deny";
            }
            if (checkreturn == 1)
                return RedirectToAction("Payment", "Cart");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ProfileUser(string id)
        {
            SessionUser session = (SessionUser)Session[Infrastructure.Information.CommonConstantUsers];
            if(session == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id!=session.Username)
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
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileUser([Bind(Include = "Username,Password,Address1,Address2,Email,Images,Phone,Birthday,DisplayName,Point,Trust,Sex,Created,Status,TypeID")] User user, FormCollection fc)
        {
            string username = fc["username"];
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
                    SessionUser session = (SessionUser)Session[Infrastructure.Information.CommonConstantUsers];
                    session.Img = user.Images;
                    return View(user);
                }
                else
                {
                    ViewErrors();
                }
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            Session[Infrastructure.Information.CommonConstantUsers] = null;
            Session[Infrastructure.Information.CommonConstantCard] = null;
            return Redirect("/");
        }
    }
}
