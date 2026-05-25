using SchoolERP_System.Areas.ERPAdmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Areas.ERPAdmin.Controllers
{
    [SessionAuthorizedAttribute]
    public class AdminDashboardController : Controller
    {
        // GET: ERPAdmin/AdminDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}