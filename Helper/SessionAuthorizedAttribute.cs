using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using IOCLWeb.Models;
using System.Configuration;

namespace SchoolERP_System.Helper
{
    public class SessionAuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var sessions = filterContext.HttpContext.Session;
                if (sessions["loggedInAdmin"] != null)
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
            if (session["loggedInAdmin"] != null)
            {
                //var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                //var action = filterContext.ActionDescriptor.ActionName;
                //if (Utility.CheckMenuAccess(action, controller, ((SessionModelClass)session["LoggedinUser"]).Role_ID) == "Yes")
                //{
                //    #region Forefully Redirection To Change Password
                //    if (Convert.ToInt32(((SessionModelClass)session["LoggedinUser"]).LastPwdChngDay) >= Convert.ToInt32(ConfigurationManager.AppSettings["PwdChngDay"]))
                //    {
                //        var Target = new RouteValueDictionary { { "action", "Index" }, { "controller", "ChangePassword" } };
                //        filterContext.Result = new RedirectToRouteResult(Target);
                //    }
                //    else
                //        return;
                //    #endregion
                //}
                //else
                //{
                //    var redirectTarget = new RouteValueDictionary { { "action", "Unauthorized" }, { "controller", "Common" } };
                //    filterContext.Result = new RedirectToRouteResult(redirectTarget);
                //}
                return;
            }
            else
            {
                //Redirect to login page.
                var redirectTarget = new RouteValueDictionary { { "action", "AdminAuthentication" }, { "controller", "Account" }, { "area", "" } };
                filterContext.Result = new RedirectToRouteResult(redirectTarget);
            }
        }
    }
}