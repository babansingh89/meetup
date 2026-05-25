using SchoolERP_System.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Areas.LearningAdmin.Controllers
{
    [SessionAuthorizedAttribute]
    public class AdminDashboardController : Controller
    {
        // GET: LearningAdmin/AdminDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}