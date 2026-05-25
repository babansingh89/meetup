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
    public class SchoolController : Controller
    {
        //public static string strConnectionString = "Password=sa@123;Persist Security Info=True;User ID=sa;Initial Catalog=SchoolERP_Master_DB;Data Source=HP";
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConStrERPAdmin"]);
        //SqlConnection con = new SqlConnection(strConnectionString);
        #region Profile
        public ActionResult SchoolProfile()
        {
           return View();            
        }
        public ActionResult UpdateSchool()
        {
            try
            {
                string SchName = Request.Form.Get("SchName");
                string SchAddress = Request.Form.Get("SchAddress");
                string Contact = Request.Form.Get("Contact");
                string EmailID = Request.Form.Get("EmailID");
                string UserPassword = Request.Form.Get("UserPassword");
                var SchoolLogo = System.Web.HttpContext.Current.Request.Files["SchoolLogo"];
                string imgpath = null;
                if (SchoolLogo != null)
                {
                    var fileName = Path.GetFileName(SchoolLogo.FileName);
                    var extention = Path.GetExtension(SchoolLogo.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(SchoolLogo.FileName);
                    string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath = Path.Combine(Server.MapPath("/SchImage/" + FileName));
                    SchoolLogo.SaveAs(imgpath);
                    imgpath = FileName;
                }

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "UpdateSchool"),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("SchName", SchName),
                    new SqlParameter("SchAddress", SchAddress),
                    new SqlParameter("Contact", Contact),
                    new SqlParameter("EmailID ", EmailID ),
                    new SqlParameter("UserPassword", UserPassword),             
                    new SqlParameter("SchoolLogo", imgpath),
                };
                string Output = Convert.ToString(new SQLHelperDynamic().ExecuteScalar(con,"SP_AppInfo", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewSchoolList(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "SelectByIDSchool"),
                    new SqlParameter("AppID",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                };
                
                        DataTable dt = new SQLHelperDynamic().ExecuteDataTable(con, "SP_AppInfo", prm1, CommandType.StoredProcedure);
                        List<SchoolModel> List = Utility.ConvertDataTableToClassObjectList<SchoolModel>(dt);
                        return Json(List, JsonRequestBehavior.AllowGet);
                   
               
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Support
        public ActionResult Support()
        {
           return View();
        }

        public ActionResult saveSupport(string SupportID,string IssueTitle,string IssueDesc)
        {
            try
            {
                string Type = "";
                if (SupportID == "" || SupportID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", Type),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("SupportID", SupportID),
                    new SqlParameter("IssueTitle", IssueTitle),
                    new SqlParameter("IssueDesc", IssueDesc),
                    new SqlParameter("IssueBy",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("UserID",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)
                };
                string Output = Convert.ToString(new SQLHelperDynamic().ExecuteScalar(con, "SP_Support", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewSupport(string SupportID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", Type),
                    new SqlParameter("SupportID",SupportID),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelperDynamic().ExecuteDataTable(con, "SP_Support", prm1, CommandType.StoredProcedure);
                List<SchoolSupport> list = Utility.ConvertDataTableToClassObjectList<SchoolSupport>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSupport(string SupportID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "Delete"),
                    new SqlParameter("SupportID", SupportID)
                };
                string Output = Convert.ToString(new SQLHelperDynamic().ExecuteScalar(con,"SP_Support", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}