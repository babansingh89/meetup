using SchoolERP_System.Helper;
using SchoolERP_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Controllers
{
   // [SessionAuthorizedAttribute]
    public class ErrorERPController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedirectDashboard()
        {
           return RedirectToAction(((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType, "Dashboard");
        }
        public ActionResult CommingSoon()
        {
            return View();
        }
    }
}