using System.Web.Mvc;
using System.Web.Routing;

namespace SuperMarketMini
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            routes.MapRoute(
                name:"DefaultRegister",
                url:"register",
                defaults: new {controller="Users",action="Register",id=UrlParameter.Optional},
                namespaces: new string[] { "SuperMarketMini.Controllers" }
                );
            routes.MapRoute(
              name: "DefautLogin",
              url: "login",
              defaults: new { controller = "Users", action = "Login", id = UrlParameter.Optional },
              namespaces: new string[] { "SuperMarketMini.Controllers" }
              );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SuperMarketMini.Controllers" }
            );

        }
    }
}
