using System.Web.Mvc;

namespace SuperMarketMini.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Login",
                url: "Admin-login",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                );
            context.MapRoute(
                name:"Search_default",
                url:"Admin-UsersManager-{action}",
                defaults: new { controller = "UsersManager", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                );
            context.MapRoute(
                name: "Product_default",
                url: "Admin-Product-{action}",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                );
            context.MapRoute(
                name: "Profile_default",
                url: "admin/profile-{id}",
                defaults: new { controller = "UsersManager", action = "ProfileUser", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                );
            context.MapRoute(
                name: "Logout_default",
                url: "admin/logout",
                defaults: new { controller = "UsersManager", action = "Logout", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Areas.Admin.Controllers" }
                    );
        }
    }
}