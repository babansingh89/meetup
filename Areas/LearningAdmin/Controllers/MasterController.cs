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
    public class MasterController : Controller
    {

        #region Specialization
        public ActionResult Specialization()
        {
            return View();
        }
        public ActionResult saveSpecializationMaster(string SCId, string SCName)
        {
            try
            {
                string Type = "";
                if (SCId == "" || SCId == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SC_ID", SCId),
                    new SqlParameter("SC_Name", SCName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Specialization", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewSpecializationMaster(string SCId, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SC_ID", SCId),
                };
                DataTable dt_Class = new SQLHelper().ExecuteDataTable("SP_Specialization", prm1, CommandType.StoredProcedure);
                List<Specialization> classList = Utility.ConvertDataTableToClassObjectList<Specialization>(dt_Class);
                return Json(classList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteSpecializationMaster(string SCId)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SC_ID", SCId)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Specialization", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Subject
        public ActionResult Subject()
        {
            return View();
        }
        public ActionResult saveSubjectMaster(string SM_Id, string SM_SubjectCode, string SM_SubjectName)
        {
            try
            {
                string Type = "";
                if (SM_Id == "" || SM_Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SM_Id", SM_Id),
                    new SqlParameter("SM_SubjectCode", SM_SubjectCode),
                    new SqlParameter("SM_SubjectName", SM_SubjectName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Subject", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewSubjectMaster(string SM_Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SM_Id", SM_Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Subject", prm1, CommandType.StoredProcedure);
                List<Subject> List = Utility.ConvertDataTableToClassObjectList<Subject>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteSubjectMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SM_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Subject", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Unit
        public ActionResult Unit()
        {
            return View();
        }
        public ActionResult saveUnitMaster(string UM_Id, string UM_UnitName)
        {
            try
            {
                string Type = "";
                if (UM_Id == "" || UM_Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UM_Id", UM_Id),
                    new SqlParameter("UM_UnitName", UM_UnitName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Unit", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewUnitMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UM_Id", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Unit", prm1, CommandType.StoredProcedure);
                List<Unit> List = Utility.ConvertDataTableToClassObjectList<Unit>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteUnitMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("UM_Id", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Unit", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Topic
        public ActionResult Topic()
        {
            return View();
        }
        public ActionResult saveTopicMaster(string TM_Id, string TM_Name)
        {
            try
            {
                string Type = "";
                if (TM_Id == "" || TM_Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("TM_Id", TM_Id),
                    new SqlParameter("TM_Name", TM_Name)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Topic", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewTopicMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("TM_Id", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Topic", prm1, CommandType.StoredProcedure);
                List<Topic> List = Utility.ConvertDataTableToClassObjectList<Topic>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteTopicMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("TM_Id", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Topic", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Level
        public ActionResult Level()
        {
            return View();
        }
        public ActionResult saveDLMaster(string DL_ID, string DL_Name)
        {
            try
            {
                string Type = "";
                if (DL_ID == "" || DL_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("DL_ID", DL_ID),
                    new SqlParameter("DL_Name", DL_Name)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Deifficulty_Level", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewDLMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("DL_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Deifficulty_Level", prm1, CommandType.StoredProcedure);
                List<DL> List = Utility.ConvertDataTableToClassObjectList<DL>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteDLMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("DL_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Deifficulty_Level", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Video
        public ActionResult Video()
        {
            return View();
        }
        public ActionResult saveVideoMaster(string V_ID, string V_Name, string V_Description, string V_Path)
        {
            try
            {
                string Type = "";
                if (V_ID == "" || V_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("V_ID", V_ID),
                    new SqlParameter("V_Name", V_Name),
                    new SqlParameter("V_Description", V_Description),
                    new SqlParameter("V_Path", V_Path)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Video_Master", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewVideoMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("V_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Video_Master", prm1, CommandType.StoredProcedure);
                List<Video> List = Utility.ConvertDataTableToClassObjectList<Video>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteVideoMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("V_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Video_Master", prm1, CommandType.StoredProcedure));
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