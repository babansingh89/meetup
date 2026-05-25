using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Areas.LearningAdmin.Controllers
{
    public class ErrorLAController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CommingSoon()
        {
            return View();
        }
    }
}