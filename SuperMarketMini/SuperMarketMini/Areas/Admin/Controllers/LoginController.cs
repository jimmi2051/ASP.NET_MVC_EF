using System.Web.Mvc;
using SuperMarketMini.Servies;
using SuperMarketMini.Areas.Admin.Common;
using SuperMarketMini.Models;
namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class LoginController : Controller
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
        // GET: Admin/Login
        public ActionResult Login()
        {
            if(Session[Infrastructure.Information.CommonConstantAdmin]==null)
                 return View();
            return Redirect("/");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")]LoginUser target)
        {
            if (ModelState.IsValid)
            {
                if(!_service.CheckPermissionAdmin(target.Username))
                {   
                    ViewBag.Errors = "Permission Denied";
                    return View(target);
                }
                if (_service.loginUser(target.Username, target.Password))
                {
                    SessionAdmin adminLogin = new SessionAdmin();
                    adminLogin.Username = target.Username;
                    adminLogin.DisplayName = _service.getUser(target.Username).DisplayName;
                    adminLogin.Img= _service.getUser(target.Username).Images;
                    Session.Add(Infrastructure.Information.CommonConstantAdmin, adminLogin);
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
        public ActionResult Logout()
        {
            Session[Infrastructure.Information.CommonConstantAdmin] = null;
            return Redirect("/Admin-login");
        }
    }
}