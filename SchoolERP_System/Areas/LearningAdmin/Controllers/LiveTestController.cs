using SchoolERP_System.Areas.LearningAdmin.Helper;
using SchoolERP_System.Areas.LearningAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SchoolERP_System.Areas.LearningAdmin.Controllers
{
    [SessionAuthorizedAttribute]
    public class LiveTestController : Controller
    {
        public ActionResult LiveTestMaster()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SpecializationList = Utility.GetDropDownList("SP_Specialization", "SC_ID", "SC_Name", prm1, "", "", "Select");
         
            return View();
        }
        public ActionResult BindSubject(string ID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Subject"),
                    new SqlParameter("ID", ID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_BindingDropdown", prm1, CommandType.StoredProcedure);
                List<Subject> List = Utility.ConvertDataTableToClassObjectList<Subject>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult saveLiveTest(string LT_ID, string LT_TestName,string LT_Date,string LT_FromTime,string LT_ToTime,string LT_SC_ID,string LT_SubjectID,string LT_Description)
        {
            try
            {
                string Type = "";
                if (LT_ID == "" || LT_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("LT_ID", LT_ID),
                    new SqlParameter("LT_TestName", LT_TestName),
                    new SqlParameter("LT_Date", LT_Date),
                    new SqlParameter("LT_FromTime", LT_FromTime),
                    new SqlParameter("LT_ToTime", LT_ToTime),
                    new SqlParameter("LT_SC_ID", LT_SC_ID),
                    new SqlParameter("LT_SubjectID", LT_SubjectID),
                    new SqlParameter("LT_Description", LT_Description)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_LiveTest", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewLiveTest(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("LT_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_LiveTest", prm1, CommandType.StoredProcedure);
                List<LT> List = Utility.ConvertDataTableToClassObjectList<LT>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteLiveTest(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("LT_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_LiveTest", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LiveTestMap()
        {
            return View();
        }
    }
}