using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc
{
    public class AuthorizedAction: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session.GetString("ID") == null)
            {
                filterContext.HttpContext.Session.Clear();
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                return;
            }

            var menus = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(filterContext.HttpContext.Session, "AssignedMenu");
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            //string url1 = "/" + controllerName + "/" + actionName;
            string url = "/" + controllerName ;

            if (!menus.Where(s => s.pageurl == url.ToLowerInvariant() || "/"+s.menuname.Replace(" ","").ToLowerInvariant() == url.ToLowerInvariant()).Any())
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Error" }, { "action", "Index" } });
                return;
            }
        }
    }
}
