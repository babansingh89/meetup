using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using SchoolERP_System.Models;
using SchoolERP_System.Helper;
using System.IO;
using System.Configuration;

namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class DashboardController : Controller
    {
        #region SuperAdmin
        [ActionName("SuperAdmin")]
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult viewDashboard(string ID, string Type)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ID",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Dashboard", prm1, CommandType.StoredProcedure);
                List<Dashboard> list = Utility.ConvertDataTableToClassObjectList<Dashboard>(dt.Tables[0]);
                List<Notice> listNotice = Utility.ConvertDataTableToClassObjectList<Notice>(dt.Tables[1]);
                mixArray[0] = list;
                mixArray[1] = listNotice;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewDashboardParents(string ID, string Type)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ID",((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).StudentID),
                    new SqlParameter("ClassID",((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).ClassID),
                    new SqlParameter("SectionID",((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).SectionID),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Dashboard", prm1, CommandType.StoredProcedure);
                List<Dashboard> list = Utility.ConvertDataTableToClassObjectList<Dashboard>(dt.Tables[0]);
                List<Notice> listNotice = Utility.ConvertDataTableToClassObjectList<Notice>(dt.Tables[1]);
                mixArray[0] = list;
                mixArray[1] = listNotice;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewAttendance(string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Dashboard", prm1, CommandType.StoredProcedure);
                List<DashboardAttendance> list = Utility.ConvertDataTableToClassObjectList<DashboardAttendance>(dt);               
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Teacher
        public ActionResult Teacher()
        {
            return View();
        }
        #endregion

        #region Parents
        public ActionResult Parents()
        {
            return View();
        }
        #endregion

        #region Student
        public ActionResult Student()
        {
            return View();
        }
        #endregion

        #region Accountant
        public ActionResult Accountant()
        {
            return View();
        }
        #endregion

        #region Librarian
        public ActionResult Librarian()
        {
            return View();
        }
        #endregion
    }
}