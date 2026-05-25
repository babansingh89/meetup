using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using SchoolERP_System.Areas.ERPAdmin.Helper;
using SchoolERP_System.Areas.ERPAdmin.Models;
namespace SchoolERP_System.Areas.ERPAdmin.Controllers
{
    public class LoginERPController : Controller
    {
        public ActionResult LoginERPAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckAdminAuthentication(string Email, string Password)
        {
            SessionModelClass smc = new SessionModelClass();
            try
            {
                object[] mixedArray = new object[2];
                SqlParameter[] prm = new SqlParameter[] {
                  new SqlParameter("@UserID", Email),
                  new SqlParameter("@UserPassword", Password),
            };
                DataTable dt_login = new SQLHelper().ExecuteDataTable("SP_Login", prm, CommandType.StoredProcedure);
                if (dt_login.Rows.Count > 0)
                {
                    string output = Convert.ToString(dt_login.Rows[0]["output"]);
                    if (output != "Yes")
                    {
                        return Json(output, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        smc.UserName = Convert.ToString(dt_login.Rows[0]["UserID"]);
                        System.Web.HttpContext.Current.Session["ERPUser"] = smc;
                        return Json(output, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Error = Convert.ToString(ex.Message);
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session["ERPUser"] = null;
            System.Web.HttpContext.Current.Session.Abandon();
            return RedirectToAction("LoginERPAdmin", "LoginERP");
        }
    }
}