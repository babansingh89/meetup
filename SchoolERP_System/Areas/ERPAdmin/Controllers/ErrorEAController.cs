using SchoolERP_System.Areas.ERPAdmin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Areas.ERPAdmin.Controllers
{
    [SessionAuthorizedAttribute]
    public class ErrorEAController : Controller
    {
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