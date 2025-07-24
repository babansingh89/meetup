using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SchoolERP_System.Areas.LearningAdmin.Helper
{
    public class SessionAuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var sessions = filterContext.HttpContext.Session;
                if (sessions["LearningUser"] != null)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            status = "401"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    //xhr status code 401 to redirect
                    filterContext.HttpContext.Response.StatusCode = 401;

                    return;
                }
            }

            var session = filterContext.HttpContext.Session;
            if (session["LearningUser"] != null)
            {
                return;
            }
            else
            {
                //Redirect to login page.
                var redirectTarget = new RouteValueDictionary { { "action", "LoginAdmin" }, { "controller", "Login" } };
                filterContext.Result = new RedirectToRouteResult(redirectTarget);
            }
        }
    }
}