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
using SelectPdf;
using System.Drawing;


namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class LibraryController : Controller
    {
        #region BookList
        public ActionResult BookList()
        {
            return View();
        }
        public ActionResult saveBookListMaster(string Id, string BookName, string BookAuthor, string ISBN, string Stock)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("BookID", Id),
                    new SqlParameter("BookName", BookName),
                    new SqlParameter("BookAuthor", BookAuthor),
                    new SqlParameter("ISBN", ISBN),
                    new SqlParameter("Stock", Stock)

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Book", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewBookListMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("BookID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Book", prm1, CommandType.StoredProcedure);
                List<Book> list = Utility.ConvertDataTableToClassObjectList<Book>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteBookListMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("BookID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Book", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region BookIssue
        public ActionResult BookIssue()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectForIssue"),
            };
            ViewBag.BookList = Utility.GetDropDownList("SP_Book", "BookID", "BookName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveBookIssueMaster(string Id, string IssueDate, string StudentID, string BookID)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Save";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("IssueID", Id),
                    new SqlParameter("IssueDate", IssueDate),
                    new SqlParameter("StudentID", StudentID),
                    new SqlParameter("BookID", BookID)

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_IssueBook", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewBookIssueMaster(string Id, string Type, string SearchID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("IssueID",Id),
                    new SqlParameter("BookStatus",SearchID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_IssueBook", prm1, CommandType.StoredProcedure);
                List<IssueBook> list = Utility.ConvertDataTableToClassObjectList<IssueBook>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BindStudent(string CID, string SID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "BindStudent"),
                    new SqlParameter("CID",CID),
                    new SqlParameter("SID",SID)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_IssueBook", prm1, CommandType.StoredProcedure);
                List<SR> list = Utility.ConvertDataTableToClassObjectList<SR>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteBookIssueMaster(string Id, string type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", type),
                    new SqlParameter("IssueID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_IssueBook", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Student
        public ActionResult BookManger()
        {
            return View();
        }
        public ActionResult BookRetrun()
        {
            return View();
        }
        #endregion

    }
}