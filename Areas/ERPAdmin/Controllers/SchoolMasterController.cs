using SchoolERP_System.Areas.ERPAdmin.Helper;
using SchoolERP_System.Areas.ERPAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace SchoolERP_System.Areas.ERPAdmin.Controllers
{
    public class SchoolMasterController : Controller
    {
        // GET: ERPAdmin/SchoolMaster
        #region School
        public ActionResult ViewSchool()
        {
            return View();
        }

        public ActionResult saveSchool(string AppID, string SchName, string SchAddress, string Contact, string EmailID, string LicenceFrom, string LicenceTo, string UserID,string UserPassword,string chkA,string chkP,string SchoolLogo)
        {
            try
            {
                string Type = "";
                if (AppID == "" || AppID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("AppID", AppID),
                    new SqlParameter("SchName", SchName),
                    new SqlParameter("SchAddress", SchAddress),
                    new SqlParameter("Contact", Contact),
                    new SqlParameter("EmailID ", EmailID ),
                    new SqlParameter("LicenceFrom", LicenceFrom),
                    new SqlParameter("LicenceTo", LicenceTo),
                    new SqlParameter("UserID", UserID),
                    new SqlParameter("UserPassword", UserPassword),
                    new SqlParameter("chkA", chkA),
                    new SqlParameter("chkP", chkP),
                   // new SqlParameter("SchoolLogo", SchoolLogo),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_AppInfo", prm1, CommandType.StoredProcedure));
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
                    new SqlParameter("Type", Type),
                    new SqlParameter("AppID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_AppInfo", prm1, CommandType.StoredProcedure);
                List<APPInfo> List = Utility.ConvertDataTableToClassObjectList<APPInfo>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSchool(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("AppID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_AppInfo", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        #endregion


        #region Support
        public ActionResult Support()
        {
            return View();
        }

        public ActionResult saveSupport(string SupportID, string ResolveDesc)
        {
            try
            {
               
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "UpdateAdmin"),
                    new SqlParameter("SupportID", SupportID),
                    new SqlParameter("ResolveDesc", ResolveDesc),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Support", prm1, CommandType.StoredProcedure));
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
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Support", prm1, CommandType.StoredProcedure);
                List<SupportAdmin> list = Utility.ConvertDataTableToClassObjectList<SupportAdmin>(dt);
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
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Support", prm1, CommandType.StoredProcedure));
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