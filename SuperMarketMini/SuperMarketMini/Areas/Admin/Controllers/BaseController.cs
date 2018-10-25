using SuperMarketMini.Areas.Admin.Common;
using System.Web.Mvc;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (SessionAdmin)Session[Infrastructure.Information.CommonConstantAdmin];
            if(session ==null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new {controller="Login",action="Login",Area ="Admin"})
                    );               
            }
            base.OnActionExecuting(filterContext);
        }
    }
}